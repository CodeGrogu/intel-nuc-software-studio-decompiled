# NUC Control implementation validation

## Completed locally

- WinUI 3 Debug build: zero warnings and zero errors.
- Frontend Release publish and ZIP creation with locked dependencies and repository VERSION.
- 12 C# tests: framing, short reads, invalid lengths, truncation, cancellation, zero/unknown/stale readings, backend errors, protocol-5 gating, no startup writes, lost acknowledgements, demo power and selected-zone lighting.
- 37 Rust tests across protocol, hardware, service library, pipe security, and pure resume preflight. New cases cover stock projections in both directions, invalid power discovery, selected-zone lighting write scope/conflicts, missing fan records, and operation status/eviction.
- Six publication-policy tests, maintained PowerShell syntax checks, repository checks, and Rust formatting.
- Native demo inspection: compact layout and accessibility labels; Cool selection changed the selected control; brightness remained an unsaved draft until Apply, then refreshed to the applied value. Periodic refresh preserves preset controls instead of recreating focused elements.
- A read-only candidate inspection confirmed the reviewed model/board/BIOS identity. The unelevated process could not acquire driver/CISD WMI readings; control eligibility was correctly withheld.
- Release backend staging preserves hash-checked revision-5 rollback files and checks the candidate executable/PDB CodeView identity. Staging does not install the candidate or change hardware settings.

## Release acceptance still required

- Elevated protocol-6 inspection and controlled power, stock cooling, lighting, reconnect, and resume acceptance on the physical device.
- Clean Windows runtime smoke test; a Windows Sandbox attempt is tracked locally, and absence of a completed result is not a pass.
- Keyboard navigation through every dialog, tray/single-instance/startup integration, and full light/dark/high-contrast plus 150/200% scaling coverage.

The installed service remains revision 5. This implementation is a reviewable candidate, not a claim that live hardware or clean-machine release acceptance has passed. The three unreadable fan writer-tail fields remain outside full verification and recovery.
