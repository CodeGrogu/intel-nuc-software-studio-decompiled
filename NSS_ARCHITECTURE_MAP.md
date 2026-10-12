# Architecture & Hardware Interface Map: Intel NUC Software Studio (NSS)

## 1. System Overview & Host Hardware
- **Host Device Identified:** `Intel(R) Client Systems NUC11PHi7` (Phantom Canyon / NUC 11 Enthusiast)
- **ACPI Hardware Device:** `ACPI\INTC1036` (`Intel(R) NUC Performance Driver`)
- **Firmware Interfaces:**
  - `root\WMI` -> `CISD_WMI` (ACPI WMI namespace for LEDs, HDMI CEC, Display Emulation, USB power)
  - `\\.\ACPI#INTC1036#...` / `GUID_DEVINTERFACE_NUCWDT` (`{1ACD29C0-C4DB-4542-9C73-9898B0D8FF6E}`) -> Kernel IOCTLs for Fan Curves, EC Registers, and MSR Power Limits.

---

## 2. Component Stack

```
┌────────────────────────────────────────────────────────┐
│  NucSoftwareStudio.exe (MSIX Client - WPF / XAML)      │
│  - SystemPerformanceMgr.dll                            │
│  - NucLedMgr.dll                                       │
│  - MobileBatterySettingsMgr.dll                        │
│  - IntelSdkClient.dll / UniwillSdkClient.dll           │
└───────────────────────────┬────────────────────────────┘
                            │ WCF (HTTP SOAP on localhost)
┌───────────────────────────▼────────────────────────────┐
│  NucSoftwareStudioService.exe (Background Windows Svc) │
│  - Ports: 43562 - 43569                                │
│  - PerformanceTuningService                            │
│  - NucLedService                                       │
│  - SystemMonitorService                                │
└─────────────┬───────────────────────────┬──────────────┘
              │ Direct WMI                │ DeviceIoControl (IOCTL)
┌─────────────▼──────────┐   ┌────────────▼──────────────┐
│ root\WMI: CISD_WMI     │   │ PerformanceDriver.sys     │
│ - SetLedStatus         │   │ - MSR Power Limits        │
│ - GetLedStatus         │   │ - Fan Curves (NUC21/22/23)│
│ - PlatformCapability   │   │ - Fan Speeds & EC Profile │
└────────────────────────┘   └───────────────────────────┘
```

---

## 3. Kernel IOCTL Interface (`PerformanceDriver.sys`)

The driver device interface is registered under GUID:
`{1ACD29C0-C4DB-4542-9C73-9898B0D8FF6E}`

### Known IOCTL Codes (`DriverControl.cs`)
| IOCTL Name | Value (Dec) | Value (Hex) | Purpose |
| :--- | :--- | :--- | :--- |
| `MSRReadPowerUnit` | `2621464588` | `0x9C40200C` | Read CPU MSR energy / power units |
| `MSRReadPowerLimit` | `2621464592` | `0x9C402010` | Read PL1 / PL2 / Tau MSR register |
| `MSRWritePowerLimit` | `2621480980` | `0x9C406014` | Write PL1 / PL2 / Tau limits |
| `ReadFanProfileCode` | `2621464608` | `0x9C402020` | Read fan curve profile struct from EC |
| `WriteFanProfileNuc21Code` | `2621480984` | `0x9C406018` | Set fan curves (NUC generation 21) |
| `WriteFanProfileNuc22Code` | `2621480988` | `0x9C40601C` | Set fan curves (NUC generation 22) |
| `WriteFanProfileNuc23Code` | `2621481036` | `0x9C40604C` | Set fan curves (NUC generation 23) |
| `ReadFanSpeedCode` | `2621464612` | `0x9C402024` | Read fan RPM / duty cycle |
| `SetFanSpeedCode` | `2621481024` | `0x9C406040` | Manual fan duty override |
| `ReadDriverStatus` | `2621464644` | `0x9C402044` | Driver health check |
| `ReadTempratureProfileCode` | `2621464648` | `0x9C402048` | Read EC temperature sensor map |

---

## 4. WMI Interface (`root\WMI` -> `CISD_WMI`)

Verified active on this machine!

### Primary Methods
- **`PlatformCapabilityReport`**: Queries supported subsystems (RGB, Fan, Power, CEC).
- **`GetLedStatus` / `SetLedStatus`**: Read and write lighting states (Skull LED, Power LED, Ring LED).
- **`PlatformConfigurationAccessV1` / `PlatformConfigurationAccessV2`**: Extended BIOS/EC platform configurations.
- **`SetDisplayDevicePower` / `GetEdidOnHdmi` / `CecMessagePassingThrough`**: On-board HDMI CEC control.

---

## 5. Local WCF Services (Port Map)
The background service exposed unauthenticated local HTTP endpoints:
- `http://localhost:43562/NssService/service/AlexaSettings`
- `http://localhost:43563/NssService/service/AlexaState`
- `http://localhost:43564/NssService/service/AppSettings`
- `http://localhost:43565/NssService/service/NucLed`
- `http://localhost:43566/NssService/service/SystemMonitor`
- `http://localhost:43567/NssService/service/Arduino`
- `http://localhost:43568/NssService/service/PerformanceTuning`
- `http://localhost:43569/NssService/service/Ene`
