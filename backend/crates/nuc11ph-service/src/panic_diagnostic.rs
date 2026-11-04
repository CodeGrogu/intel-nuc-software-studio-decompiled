//! Diagnostic-only panic evidence before the existing release panic-abort path.
//! The parent folder must be provisioned for SYSTEM/administrators before deployment.
//! This module performs no hardware operation and does not recover from a panic.
use std::backtrace::Backtrace;
use std::fs::{File, OpenOptions};
use std::io::{self, Write};
use std::panic::{self, PanicHookInfo};
use std::sync::atomic::{AtomicBool, Ordering};
use std::time::{SystemTime, UNIX_EPOCH};

const LOG_PATH: &str = r"C:\ProgramData\Nuc11PhStudio-Diagnostics\service-panic.log";
const FILE_CAP: u64 = 1024 * 1024;
const RECORD_CAP: usize = 64 * 1024;
static RECORDING: AtomicBool = AtomicBool::new(false);

pub(crate) fn install() {
    let previous = panic::take_hook();
    panic::set_hook(Box::new(move |info| {
        if RECORDING
            .compare_exchange(false, true, Ordering::Acquire, Ordering::Relaxed)
            .is_ok()
        {
            // No mutex/unwrap or logger recursion in this hook. Evidence failures are ignored.
            let _ = record(info);
            RECORDING.store(false, Ordering::Release);
        }
        previous(info);
    }));
}

pub(crate) fn record_resume_failure(message: &str) {
    let _ = (|| -> io::Result<()> {
        let file = OpenOptions::new()
            .create(true)
            .append(true)
            .open(LOG_PATH)?;
        let existing = file.metadata()?.len();
        if existing >= FILE_CAP {
            return Ok(());
        }
        let mut output = BoundedWriter {
            file,
            remaining: ((FILE_CAP - existing) as usize).min(RECORD_CAP),
        };
        let epoch_ms = SystemTime::now()
            .duration_since(UNIX_EPOCH)
            .map(|v| v.as_millis())
            .unwrap_or(0);
        let bounded: String = message.chars().take(4096).collect();
        writeln!(
            output,
            "\nRESUME REJECTED unix_ms={epoch_ms} pid={} error={bounded}",
            std::process::id()
        )?;
        output.flush()?;
        output.file.sync_data()
    })();
}

fn record(info: &PanicHookInfo<'_>) -> io::Result<()> {
    // Do not create directories or alter access controls from a failing process.
    let file = OpenOptions::new()
        .create(true)
        .append(true)
        .open(LOG_PATH)?;
    let existing = file.metadata()?.len();
    if existing >= FILE_CAP {
        return Ok(());
    }
    let budget = ((FILE_CAP - existing) as usize).min(RECORD_CAP);
    let mut output = BoundedWriter {
        file,
        remaining: budget,
    };
    let epoch_ms = SystemTime::now()
        .duration_since(UNIX_EPOCH)
        .map(|v| v.as_millis())
        .unwrap_or(0);
    let thread = std::thread::current();
    writeln!(
        output,
        "\nBEGIN PANIC unix_ms={epoch_ms} pid={} thread={:?} name={:?}",
        std::process::id(),
        thread.id(),
        thread.name()
    )?;
    let message = if let Some(s) = info.payload().downcast_ref::<String>() {
        s.as_str()
    } else if let Some(s) = info.payload().downcast_ref::<&str>() {
        s
    } else {
        "<non-string panic payload>"
    };
    let bounded_message: String = message.chars().take(4096).collect();
    writeln!(output, "message={bounded_message}")?;
    if let Some(location) = info.location() {
        writeln!(
            output,
            "location={}:{}:{}",
            location.file(),
            location.line(),
            location.column()
        )?;
    }
    // Force capture without requiring RUST_BACKTRACE or executing any target callback.
    writeln!(output, "backtrace:\n{:#}", Backtrace::force_capture())?;
    writeln!(output, "END PANIC")?;
    output.flush()?;
    output.file.sync_data()
}

struct BoundedWriter {
    file: File,
    remaining: usize,
}
impl Write for BoundedWriter {
    fn write(&mut self, buffer: &[u8]) -> io::Result<usize> {
        let count = self.remaining.min(buffer.len());
        if count != 0 {
            self.file.write_all(&buffer[..count])?;
            self.remaining -= count;
        }
        // Discard excess output while reporting it consumed, keeping formatting bounded.
        Ok(buffer.len())
    }
    fn flush(&mut self) -> io::Result<()> {
        self.file.flush()
    }
}
