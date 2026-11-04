//! RAII synchronization guard for the `Global\Access_ISABUS.HTP.Method` named mutex.
//! Prevents bus contention between `PerformanceDriver.sys` EC operations and other
//! hardware monitoring tools.

use thiserror::Error;
use windows::core::PCWSTR;
use windows::Win32::Foundation::{CloseHandle, HANDLE, WAIT_ABANDONED, WAIT_OBJECT_0};
use windows::Win32::System::Threading::{
    CreateMutexW, OpenMutexW, ReleaseMutex, WaitForSingleObject, MUTEX_MODIFY_STATE,
    SYNCHRONIZATION_SYNCHRONIZE,
};

const MUTEX_NAME_UTF16: &[u16] = &[
    b'G' as u16,
    b'l' as u16,
    b'o' as u16,
    b'b' as u16,
    b'a' as u16,
    b'l' as u16,
    b'\\' as u16,
    b'A' as u16,
    b'c' as u16,
    b'c' as u16,
    b'e' as u16,
    b's' as u16,
    b's' as u16,
    b'_' as u16,
    b'I' as u16,
    b'S' as u16,
    b'A' as u16,
    b'B' as u16,
    b'U' as u16,
    b'S' as u16,
    b'.' as u16,
    b'H' as u16,
    b'T' as u16,
    b'P' as u16,
    b'.' as u16,
    b'M' as u16,
    b'e' as u16,
    b't' as u16,
    b'h' as u16,
    b'o' as u16,
    b'd' as u16,
    0,
];

#[derive(Debug, Error)]
pub enum MutexError {
    #[error("Failed to create or open Global\\Access_ISABUS.HTP.Method mutex: {0}")]
    OpenFailed(String),
    #[error("Timed out waiting for Global\\Access_ISABUS.HTP.Method mutex after {0} ms")]
    Timeout(u32),
}

pub struct IsaBusMutex {
    handle: HANDLE,
}

unsafe impl Send for IsaBusMutex {}
unsafe impl Sync for IsaBusMutex {}

impl IsaBusMutex {
    pub fn new() -> Result<Self, MutexError> {
        unsafe {
            let name_ptr = PCWSTR::from_raw(MUTEX_NAME_UTF16.as_ptr());
            let handle = match CreateMutexW(None, false, name_ptr) {
                Ok(h) if !h.is_invalid() => h,
                _ => OpenMutexW(
                    SYNCHRONIZATION_SYNCHRONIZE | MUTEX_MODIFY_STATE,
                    false,
                    name_ptr,
                )
                .map_err(|e| MutexError::OpenFailed(e.to_string()))?,
            };
            Ok(Self { handle })
        }
    }

    pub fn lock(&self, timeout_ms: u32) -> Result<IsaBusMutexGuard<'_>, MutexError> {
        unsafe {
            let status = WaitForSingleObject(self.handle, timeout_ms);
            if status == WAIT_OBJECT_0 || status == WAIT_ABANDONED {
                Ok(IsaBusMutexGuard { mutex: self })
            } else {
                Err(MutexError::Timeout(timeout_ms))
            }
        }
    }
}

impl Drop for IsaBusMutex {
    fn drop(&mut self) {
        unsafe {
            if !self.handle.is_invalid() {
                let _ = CloseHandle(self.handle);
            }
        }
    }
}

pub struct IsaBusMutexGuard<'a> {
    mutex: &'a IsaBusMutex,
}

impl<'a> Drop for IsaBusMutexGuard<'a> {
    fn drop(&mut self) {
        unsafe {
            let _ = ReleaseMutex(self.mutex.handle);
        }
    }
}
