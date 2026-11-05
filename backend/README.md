# Windows backend source

This is a portable source snapshot of the repaired diagnostic backend used during
the NUC investigation. It adds fan validation before writes, verified-state guards
for resume replay, and bounded panic logging. Rebuilding this source does not prove
parity with the older deployed executable. No driver or installer is bundled here.

Use Windows x64, the MSVC C++ build tools, and Rust installed through rustup.
`rust-toolchain.toml` pins the compiler; `Cargo.lock` pins dependencies.

```sh
cd backend
cargo fmt --all -- --check
cargo test --locked -p nuc11ph-protocol -p nuc11ph-hw --lib --tests
cargo build --locked --release -p nuc11ph-service
```

The tests exercise protocol serialization and pure preflight decisions;
they do not instantiate a hardware driver or invoke WMI. Building produces an
executable but does not install or run it. Deployment and live hardware testing
require a separate reviewed procedure with saved state and recovery evidence.

The 94-byte fixture is a captured readable fan profile. Its provenance and reduced
numeric state are next to the fixture; raw machine captures are intentionally local.
The fan writer has a 90-byte packet and three writable bytes outside the known read
window, so round-trip fixture checks do not establish complete hardware recovery.

The root research bundle version is independent of inherited Cargo package
versions and Intel package versions. Package manifests retain their existing
metadata; see the root NOTICE before reusing vendor-derived material.

Protocol revision 5 adds [per-field LED readback](../docs/LED_FIELD_READBACK.md)
so readable RGB values remain available when a sibling brightness read fails.
Check the running service's protocol revision before using the new request.
