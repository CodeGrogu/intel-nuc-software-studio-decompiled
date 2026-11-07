//! Bounded admission and reconciliation. All methods run under the same registry mutex.
//! Accepted work is never evicted/cancelled; epochs fence only work not yet admitted.
use nuc11ph_protocol::{OperationOutcome, OperationStatus};
use std::collections::VecDeque;

#[derive(Debug)]
pub enum Admission {
    Execute,
    Reply(OperationStatus),
}
struct Entry {
    fingerprint: String,
    status: OperationStatus,
}
pub struct OperationBook {
    entries: VecDeque<Entry>,
    epoch: u64,
    capacity: usize,
}
impl Default for OperationBook {
    fn default() -> Self {
        Self::with_capacity(128)
    }
}
impl OperationBook {
    fn with_capacity(capacity: usize) -> Self {
        assert!(capacity > 0);
        Self {
            entries: VecDeque::new(),
            epoch: 1,
            capacity,
        }
    }
    pub fn epoch(&self) -> u64 {
        self.epoch
    }
    fn rotate(&mut self) {
        // MAX is permanently closed to admission; never wrap and revive an old epoch.
        self.epoch = self.epoch.saturating_add(1);
    }
    fn validate_id(id: &str) -> Result<(), &'static str> {
        if id.is_empty()
            || id.len() > 64
            || !id.bytes().all(|b| b.is_ascii_alphanumeric() || b == b'-')
        {
            Err("Invalid operation ID")
        } else {
            Ok(())
        }
    }
    fn status(
        id: &str,
        instance: &str,
        outcome: OperationOutcome,
        message: &str,
    ) -> OperationStatus {
        OperationStatus {
            operation_id: id.into(),
            backend_instance: instance.into(),
            outcome,
            message: message.into(),
        }
    }
    pub fn get(&self, id: &str) -> Option<OperationStatus> {
        self.entries
            .iter()
            .find(|e| e.status.operation_id == id)
            .map(|e| e.status.clone())
    }
    pub fn admit(
        &mut self,
        id: &str,
        fingerprint: &str,
        expected_instance: &str,
        expected_epoch: u64,
        instance: &str,
    ) -> Result<Admission, &'static str> {
        Self::validate_id(id)?;
        if expected_instance != instance {
            return Ok(Admission::Reply(Self::status(
                id,
                instance,
                OperationOutcome::Rejected,
                "Service instance changed; command was not admitted.",
            )));
        }
        // A duplicate of accepted work is a lookup, even after its epoch closes.
        if let Some(entry) = self.entries.iter().find(|e| e.status.operation_id == id) {
            return if entry.fingerprint == fingerprint {
                Ok(Admission::Reply(entry.status.clone()))
            } else {
                Err("Operation ID reused with a different command")
            };
        }
        if expected_epoch != self.epoch || self.epoch == u64::MAX {
            return Ok(Admission::Reply(Self::status(id, instance, OperationOutcome::Rejected, "Admission epoch closed or invalid; command was not admitted. Refresh before a new operation.")));
        }
        if self.entries.len() == self.capacity {
            // Preserve queued/running entries and fence this rejected request.
            self.rotate();
            return Ok(Admission::Reply(Self::status(id, instance, OperationOutcome::Rejected, "Operation capacity reached; command was not admitted. Refresh before a new operation.")));
        }
        self.entries.push_back(Entry {
            fingerprint: fingerprint.into(),
            status: Self::status(
                id,
                instance,
                OperationOutcome::Pending,
                "Accepted and pending; disconnect does not cancel this operation.",
            ),
        });
        Ok(Admission::Execute)
    }
    pub fn finish(&mut self, status: OperationStatus) {
        if let Some(entry) = self
            .entries
            .iter_mut()
            .find(|e| e.status.operation_id == status.operation_id)
        {
            entry.status = status;
        }
        if self.entries.len() == self.capacity {
            if let Some(index) = self
                .entries
                .iter()
                .position(|e| e.status.outcome != OperationOutcome::Pending)
            {
                // Fence forgotten IDs in the same critical section as eviction.
                self.rotate();
                self.entries.remove(index);
            }
        }
    }
    pub fn reconcile(
        &mut self,
        id: &str,
        expected_instance: &str,
        expected_epoch: u64,
        instance: &str,
    ) -> Result<OperationStatus, &'static str> {
        Self::validate_id(id)?;
        if expected_instance != instance {
            return Ok(Self::status(id, instance, OperationOutcome::Uncertain, "Service restarted. Previous outcome is unknown; old-instance frames cannot execute. Refresh actual state."));
        }
        if expected_epoch > self.epoch || expected_epoch == 0 {
            return Err("Invalid admission epoch");
        }
        if let Some(status) = self.get(id) {
            return Ok(status);
        }
        if expected_epoch == self.epoch {
            self.rotate();
        }
        Ok(Self::status(id, instance, OperationOutcome::Uncertain, "Outcome unavailable. The old admission epoch is closed; this operation cannot execute later. Refresh actual state; no command was replayed."))
    }
}

#[cfg(test)]
mod tests {
    use super::*;
    fn admit(book: &mut OperationBook, id: &str, epoch: u64) -> Admission {
        book.admit(id, "command", "service", epoch, "service")
            .unwrap()
    }
    fn finish(book: &mut OperationBook, id: &str) {
        book.finish(OperationBook::status(
            id,
            "service",
            OperationOutcome::Verified,
            "done",
        ));
    }
    #[test]
    fn pending_is_visible_before_execution_and_duplicate_executes_once() {
        let mut book = OperationBook::default();
        assert!(matches!(admit(&mut book, "one", 1), Admission::Execute));
        assert_eq!(book.get("one").unwrap().outcome, OperationOutcome::Pending);
        assert!(matches!(admit(&mut book, "one", 1), Admission::Reply(_)));
        assert!(book
            .admit("one", "different", "service", 1, "service")
            .is_err());
        finish(&mut book, "one");
        assert_eq!(book.get("one").unwrap().outcome, OperationOutcome::Verified);
    }
    #[test]
    fn fence_rejects_delayed_frame_but_preserves_accepted_old_epoch_work() {
        let mut book = OperationBook::default();
        assert!(matches!(admit(&mut book, "queued", 1), Admission::Execute));
        assert_eq!(
            book.reconcile("late", "service", 1, "service")
                .unwrap()
                .outcome,
            OperationOutcome::Uncertain
        );
        assert_eq!(book.epoch(), 2);
        assert!(matches!(
            admit(&mut book, "late", 1),
            Admission::Reply(OperationStatus {
                outcome: OperationOutcome::Rejected,
                ..
            })
        ));
        assert!(matches!(
            admit(&mut book, "queued", 1),
            Admission::Reply(OperationStatus {
                outcome: OperationOutcome::Pending,
                ..
            })
        ));
        finish(&mut book, "queued");
        assert_eq!(
            book.get("queued").unwrap().outcome,
            OperationOutcome::Verified
        );
        book.reconcile("late", "service", 1, "service").unwrap();
        assert_eq!(book.epoch(), 2);
    }
    #[test]
    fn eviction_fences_forgotten_id_and_never_evicts_pending() {
        let mut book = OperationBook::with_capacity(2);
        admit(&mut book, "queued", 1);
        admit(&mut book, "done", 1);
        finish(&mut book, "done");
        assert!(book.get("done").is_none());
        assert_eq!(
            book.get("queued").unwrap().outcome,
            OperationOutcome::Pending
        );
        assert!(matches!(
            admit(&mut book, "done", 1),
            Admission::Reply(OperationStatus {
                outcome: OperationOutcome::Rejected,
                ..
            })
        ));
        assert_eq!(
            book.reconcile("done", "service", 1, "service")
                .unwrap()
                .outcome,
            OperationOutcome::Uncertain
        );
    }
    #[test]
    fn saturation_and_lost_rejection_reply_are_reconcilable() {
        let mut book = OperationBook::with_capacity(1);
        admit(&mut book, "running", 1);
        assert!(matches!(
            admit(&mut book, "rejected", 1),
            Admission::Reply(OperationStatus {
                outcome: OperationOutcome::Rejected,
                ..
            })
        ));
        assert_eq!(
            book.reconcile("rejected", "service", 1, "service")
                .unwrap()
                .outcome,
            OperationOutcome::Uncertain
        );
        assert_eq!(
            book.get("running").unwrap().outcome,
            OperationOutcome::Pending
        );
        finish(&mut book, "running");
        assert!(matches!(
            admit(&mut book, "rejected", 1),
            Admission::Reply(_)
        ));
    }
    #[test]
    fn instance_future_epoch_and_counter_exhaustion_fail_closed() {
        let mut book = OperationBook::default();
        assert!(matches!(
            book.admit("one", "x", "old", 1, "service").unwrap(),
            Admission::Reply(_)
        ));
        assert!(matches!(admit(&mut book, "future", 2), Admission::Reply(_)));
        assert!(book.reconcile("future", "service", 2, "service").is_err());
        assert_eq!(
            book.reconcile("one", "old", 1, "service").unwrap().outcome,
            OperationOutcome::Uncertain
        );
        book.epoch = u64::MAX - 1;
        book.reconcile("close", "service", u64::MAX - 1, "service")
            .unwrap();
        assert!(matches!(
            admit(&mut book, "closed", u64::MAX),
            Admission::Reply(_)
        ));
        assert!(book.admit("bad/id", "x", "service", 1, "service").is_err());
    }
    #[test]
    fn concurrent_admission_fence_and_queued_execution() {
        use std::sync::{mpsc, Arc, Mutex};
        let book = Arc::new(Mutex::new(OperationBook::default()));
        let (accepted_tx, accepted_rx) = mpsc::channel();
        let (release_tx, release_rx) = mpsc::channel();
        let worker_book = book.clone();
        let worker = std::thread::spawn(move || {
            assert!(matches!(
                admit(&mut worker_book.lock().unwrap(), "queued", 1),
                Admission::Execute
            ));
            accepted_tx.send(()).unwrap();
            release_rx.recv().unwrap(); // Worker/hardware gate, after admission.
            finish(&mut worker_book.lock().unwrap(), "queued");
        });
        accepted_rx.recv().unwrap();
        let mut registry = book.lock().unwrap();
        assert_eq!(
            registry
                .reconcile("queued", "service", 1, "service")
                .unwrap()
                .outcome,
            OperationOutcome::Pending
        );
        registry.reconcile("late", "service", 1, "service").unwrap();
        drop(registry);
        release_tx.send(()).unwrap();
        worker.join().unwrap();
        assert_eq!(
            book.lock().unwrap().get("queued").unwrap().outcome,
            OperationOutcome::Verified
        );
    }
}
