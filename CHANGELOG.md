# Changelog

Release PRs update this file from Conventional Commits. Repository bundle versions
are independent of Intel package versions and inherited Rust crate versions.

## [0.2.0](https://github.com/CodeGrogu/intel-nuc-software-studio-decompiled/compare/v0.1.0...v0.2.0) (2026-11-07)


### ⚠ BREAKING CHANGES

* **desktop:** add NUC Control and guarded protocol 6 controls ([#12](https://github.com/CodeGrogu/intel-nuc-software-studio-decompiled/issues/12))

### Features

* **ci:** add safe validation and automated research releases ([#1](https://github.com/CodeGrogu/intel-nuc-software-studio-decompiled/issues/1)) ([b2484c0](https://github.com/CodeGrogu/intel-nuc-software-studio-decompiled/commit/b2484c090bf2b231171b505d77c19a0e0827ff4b))
* **desktop:** add NUC Control and guarded protocol 6 controls ([#12](https://github.com/CodeGrogu/intel-nuc-software-studio-decompiled/issues/12)) ([ac7dad7](https://github.com/CodeGrogu/intel-nuc-software-studio-decompiled/commit/ac7dad741090d75835fd7b64210ea57ba0e611ae))
* **led:** add partial readback and guard firmware setter defects ([#10](https://github.com/CodeGrogu/intel-nuc-software-studio-decompiled/issues/10)) ([45a5e88](https://github.com/CodeGrogu/intel-nuc-software-studio-decompiled/commit/45a5e88946c3815c249155d9d6d316be85326ffa))


### Bug Fixes

* **led:** preserve unreadable Software brightness in fallback writes ([#9](https://github.com/CodeGrogu/intel-nuc-software-studio-decompiled/issues/9)) ([0702c89](https://github.com/CodeGrogu/intel-nuc-software-studio-decompiled/commit/0702c890de7cecfc62ed7343eb382b6c5a305188))

## 0.1.0 (2026-11-05)

### Added

- Repository governance, CI, reviewed release automation and dependency updates.
- Portable repaired backend source and pure fan-packet/resume regression tests.
- Curated hardware findings and explicit validation boundaries.

### Preserved

- Existing decompiled application/service source and repository history.

No vendor driver, installer, deployed service executable or raw machine capture is
distributed by this release process. Existing legacy driver resources are retained
unchanged in the historical source tree and are excluded from CI execution.
