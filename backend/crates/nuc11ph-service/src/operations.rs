//! Bounded, process-local reconciliation; never a persistent hardware command queue.
use nuc11ph_protocol::{OperationOutcome, OperationStatus};
use std::collections::VecDeque;

pub struct OperationBook {
    entries: VecDeque<(String, String, OperationStatus)>,
}
impl Default for OperationBook {
    fn default() -> Self {
        Self {
            entries: VecDeque::new(),
        }
    }
}
impl OperationBook {
    pub fn get(&self, id: &str) -> Option<OperationStatus> {
        self.entries
            .iter()
            .find(|(key, _, _)| key == id)
            .map(|(_, _, state)| state.clone())
    }
    pub fn begin(
        &mut self,
        id: &str,
        fingerprint: &str,
        instance: &str,
    ) -> Result<Option<OperationStatus>, &'static str> {
        if id.is_empty()
            || id.len() > 64
            || !id.bytes().all(|b| b.is_ascii_alphanumeric() || b == b'-')
        {
            return Err("Invalid operation ID");
        }
        if let Some((_, previous, status)) = self.entries.iter().find(|(key, _, _)| key == id) {
            return if previous == fingerprint {
                Ok(Some(status.clone()))
            } else {
                Err("Operation ID reused with a different command")
            };
        }
        if self.entries.len() == 128 {
            self.entries.pop_front();
        }
        self.entries.push_back((
            id.into(),
            fingerprint.into(),
            OperationStatus {
                operation_id: id.into(),
                backend_instance: instance.into(),
                outcome: OperationOutcome::Pending,
                message: "Executing; disconnect does not cancel".into(),
            },
        ));
        Ok(None)
    }
    pub fn finish(&mut self, status: OperationStatus) {
        if let Some((_, _, saved)) = self
            .entries
            .iter_mut()
            .find(|(id, _, _)| id == &status.operation_id)
        {
            *saved = status;
        }
    }
}

#[cfg(test)]
mod tests {
    use super::*;
    #[test]
    fn duplicate_conflict_pending_terminal_and_eviction() {
        let mut book = OperationBook::default();
        assert!(book.begin("one", "fan-cool", "instance").unwrap().is_none());
        assert_eq!(
            book.begin("one", "fan-cool", "instance")
                .unwrap()
                .unwrap()
                .outcome,
            OperationOutcome::Pending
        );
        assert!(book.begin("one", "fan-balanced", "instance").is_err());
        let mut result = book.get("one").unwrap();
        result.outcome = OperationOutcome::PartialFailure;
        book.finish(result);
        assert_eq!(
            book.get("one").unwrap().outcome,
            OperationOutcome::PartialFailure
        );
        for i in 0..128 {
            book.begin(&format!("op-{i}"), "read", "instance").unwrap();
        }
        assert!(book.get("one").is_none());
        assert!(book.begin("bad/id", "x", "instance").is_err());
    }
}
