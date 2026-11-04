# Intel NUC Software Studio research

[![CI](https://github.com/CodeGrogu/intel-nuc-software-studio-decompiled/actions/workflows/ci.yml/badge.svg)](https://github.com/CodeGrogu/intel-nuc-software-studio-decompiled/actions/workflows/ci.yml)

Reverse engineering notes and a Windows Rust backend for the Intel NUC11PH
(Phantom Canyon). Research bundle version: see [VERSION](VERSION). This is an
independent research project; Intel has not endorsed it.

## Repository layout

| Path | Purpose |
| --- | --- |
| `backend/` | Maintained Rust source snapshot, protocol fixtures, and offline regression tests |
| `docs/HARDWARE_FINDINGS.md` | Curated findings and explicit validation boundaries |
| `decompiled_app/`, `decompiled_service/` | Historical decompiler output, preserved from the original commit |
| `NSS_*.md` | Historical architecture and specification documents; findings may be superseded by the curated notes |
| `.github/`, `tools/` | CI, release automation, and publication checks |

Original installers, raw machine captures, local deployment backups, generated
`decomplied/` output, and `runtime/` remain local and are ignored by Git. Historical
tracked vendor resources remain in the original source tree; CI never executes them.
The C# decompiler projects are reference material, not a verified reproducible build.

## Validate changes

Repository checks require Python 3.12+ and PowerShell 7:

```sh
python -m pip install -r requirements-dev.txt
python -m unittest discover -s tools/tests -v
python tools/check_repository.py
pwsh -NoProfile -File tools/check_powershell.ps1
```

On Windows, the [backend instructions](backend/README.md) cover formatting, pure
tests, and compilation. CI runs these checks on GitHub-hosted runners; it does not
install a service, load drivers, or access NUC hardware. Passing CI is evidence for
source and protocol regressions, not hardware certification.

## Contribute and release

Use Conventional Commits and pull requests. Release Please prepares version and
changelog changes for review, then creates a tag and GitHub release after its release
PR is merged. See [contributing](CONTRIBUTING.md), [release policy](docs/RELEASING.md),
[security](SECURITY.md), and [rights and attribution](NOTICE.md).
