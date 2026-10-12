# Intel NUC Software Studio - Decompiled Research Repository

> **Disclaimer:** This repository contains reverse-engineered and decompiled research source code, specifications, and architecture maps for the officially discontinued (**End of Life / EOL**) Intel NUC Software Studio suite. Maintained strictly for educational, interoperability, and custom open-source driver/utility development for Intel NUC systems (specifically **Phantom Canyon / NUC 11 Enthusiast - NUC11PHi7**).

---

## 📁 Repository Structure

```
intel-nuc-software-studio-decompiled/
├── NSS_ARCHITECTURE_MAP.md     # Component layer diagram & initial IOCTL/WMI survey
├── NSS_FULL_SPECIFICATION.md   # Exhaustive MSR bitmasks, fan curve packets, & WMI API
├── NSS_FULL_AUDIT_REPORT.md    # Security audit, LPE analysis, and live system verification
├── decompiled_service/         # Background Windows Service & Hardware Abstraction
│   ├── NucSoftwareStudioService/ # Main Windows Service (.NET 4.6.1) hosting WCF endpoints
│   ├── PerformanceTuningLibrary/ # MSR Power (PL1/PL2/Tau), Fan Curves & IOCTL dispatch
│   ├── SystemMonitorLibrary/    # CPU, GPU & Sensor telemetry (OpenHardwareMonitorLib)
│   ├── Infrastructure/          # Native WMI wrappers & shared utilities
│   └── ...
└── decompiled_app/             # Front-end WPF / MSIX Client Application
    ├── NucSoftwareStudio/       # Main WPF UI shell
    ├── NucLedMgr/               # RGB lighting logic & zones (Skull, Eyes, Power LED)
    ├── SystemPerformanceMgr/    # User-facing performance modes & fan sliders
    ├── IntelSdkClient/          # WCF client proxies communicating with localhost
    └── ...
```

---

## ⚙️ Key Technical Findings

### 1. Target Hardware & ACPI Identity
* **System Product:** `Intel(R) Client Systems NUC11PHi7` (Phantom Canyon)
* **BIOS ID Prefix:** `PH` (`PHTGL579...`)
* **Hardware ID:** `ACPI\INTC1036` (`PerformanceDriver.sys`)
* **Driver Device GUID:** `{1ACD29C0-C4DB-4542-9C73-9898B0D8FF6E}`

### 2. Hardware Control Vectors
* **RGB LED Lighting:** Controlled via `root\WMI` class `CISD_WMI` (`SetIndicatorOptionLedType`, `SetValueIndicatorOptionLedType`, and persistent NVRAM write `AppNotify`).
* **Fan Curves & Manual Overrides:** Controlled via IOCTLs (`0x9C406018` / `0x9C406040`) dispatched to `PerformanceDriver.sys` with 14-byte per-fan configuration blocks.
* **CPU Power Limits (PL1 / PL2 / Tau):** Handled via 64-bit MSR register writes (`IOCTL 0x9C406014`) using custom Tau code translation tables.

---

## 🔒 Security & IPC Audit Summary
The background service exposes unauthenticated HTTP SOAP endpoints over local ports (`43562` – `43569`) with Windows Message Security enabled. Detailed security implications, local privilege escalation vectors, and replacement architectural designs are documented in `NSS_FULL_AUDIT_REPORT.md`.

---

## 🛠️ Research Goals & Roadmap
- [x] Complete extraction and decompilation of MSIX and MSI packages.
- [x] Architecture deconstruction and IOCTL / WMI documentation.
- [x] Full security and protocol audit.
- [ ] Build a lightweight, standalone, zero-bloat CLI / Tray utility for fan curves, power limits, and RGB lighting.
- [ ] Develop plugins for open-source tools (e.g., [Rem0o/FanControl](https://github.com/Rem0o/FanControl.Releases) and OpenRGB).
