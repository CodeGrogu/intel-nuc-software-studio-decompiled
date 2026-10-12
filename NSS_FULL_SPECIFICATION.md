# Intel NUC Software Studio (NSS) - Complete Deconstructed Specification

This document provides the exhaustive, reverse-engineered specification of the **Intel NUC Software Studio** application suite (Frontend App + Background Windows Service + Kernel Driver + Firmware Interfaces).

---

## 1. System Target & Architecture Overview

* **Target Hardware Identified:** `Intel(R) Client Systems NUC11PHi7` (Phantom Canyon / NUC 11 Enthusiast)
* **BIOS Pattern:** Matches `PH` (`PlatformDetail.cs`), BIOS identifier `PHTGL579...`
* **Driver Hardware ID:** `ACPI\INTC1036` (`Intel(R) NUC Performance Driver`)
* **Driver Device Interface GUID:** `{1ACD29C0-C4DB-4542-9C73-9898B0D8FF6E}` (`GUID_DEVINTERFACE_NUCWDT`)

### Layer Stack Diagram

```
┌────────────────────────────────────────────────────────┐
│  NucSoftwareStudio.exe (MSIX Client - WPF / MVVM)      │
│  - NucLedMgr.dll                                       │
│  - SystemPerformanceMgr.dll                            │
│  - FeatureMgr.dll                                      │
│  - MobileBatterySettingsMgr.dll                        │
│  - IntelSdkClient.dll                                  │
└───────────────────────────┬────────────────────────────┘
                            │ WCF (HTTP SOAP on localhost)
                            │ Ports: 43562 - 43569
┌───────────────────────────▼────────────────────────────┐
│  NucSoftwareStudioService.exe (Background Windows Svc) │
│  - PerformanceTuningService                            │
│  - NucLedService                                       │
│  - SystemMonitorService (OpenHardwareMonitorLib)       │
└─────────────┬───────────────────────────┬──────────────┘
              │ Direct WMI                │ DeviceIoControl (IOCTL)
┌─────────────▼──────────┐   ┌────────────▼──────────────┐
│ root\WMI: CISD_WMI     │   │ PerformanceDriver.sys     │
│ - SetLedStatus         │   │ - MSR Power Limits (PL1/2)│
│ - GetLedStatus         │   │ - Fan Curves (NUC21/22/23)│
│ - PlatformCapability   │   │ - Fan Speeds & EC Profiles│
└────────────────────────┘   └───────────────────────────┘
```

---

## 2. Kernel Driver Interface (`PerformanceDriver.sys`)

The driver creates a device handle accessible via SetupAPI using GUID `{1ACD29C0-C4DB-4542-9C73-9898B0D8FF6E}`. A global mutex `Global\Access_ISABUS.HTP.Method` is acquired before bus/EC operations.

### IOCTL Definitions (`DriverControl.cs`)

| IOCTL Name | Value (Hex) | Value (Dec) | Direction / Buffer In | Buffer Out | Description |
| :--- | :--- | :--- | :--- | :--- | :--- |
| `MSRReadPowerUnit` | `0x9C40200C` | `2621464588` | None | `ulong` | Reads CPU power calculation multiplier units |
| `MSRReadPowerLimit`| `0x9C402010` | `2621464592` | None | `ulong` (64-bit) | Reads active PL1, PL2, and Tau bitfield |
| `MSRWritePowerLimit`|`0x9C406014` | `2621480980` | `ulong` (64-bit) | None | Writes new PL1, PL2, and Tau bitfield |
| `ReadFanProfileCode`| `0x9C402020`| `2621464608` | None | `FanProfileEC` (94 bytes) | Reads full active fan curve table from EC |
| `WriteFanProfileNuc21Code` | `0x9C406018` | `2621480984` | `FanProfileWmi` (90 bytes) | None | Commits fan curve table (NUC Gen 21) |
| `WriteFanProfileNuc22Code` | `0x9C40601C` | `2621480988` | `FanProfileWmi` (90 bytes) | None | Commits fan curve table (NUC Gen 22) |
| `WriteFanProfileNuc23Code` | `0x9C40604C` | `2621481036` | `FanProfileWmi23` (21 bytes)| None | Commits fan curve table (NUC Gen 23) |
| `ReadFanSpeedCode` | `0x9C402024` | `2621464612` | `byte` (EC Section) | `FanSpeedStruct` (18 bytes) | Reads current fan RPMs and duties |
| `SetFanSpeedCode`  | `0x9C406040` | `2621481024` | `FanDuty` struct | None | Overrides manual fan duty cycle |
| `ReadDriverStatus` | `0x9C402044` | `2621464644` | None | None | Verifies driver status |
| `ReadTempratureProfileCode` | `0x9C402048` | `2621464648` | None | `TempratureProfileEC` (15 bytes) | Reads EC onboard thermal sensors |

---

## 3. Power Limit (PL1 / PL2 / Tau) Logic & Bitmasks

Power tuning reads and writes a 64-bit MSR register via `PerformanceDriver.sys`.

### MSR Bitfield Layout (`PowerLevelControl.cs`)
* **PL1 Mask:** `0x0000000000003FFF` (`16383uL`)
  * Extract PL1: `(uint)(val & 0x3FFF) >> 3`
  * Write PL1: `(pl1_watts << 3)`
* **PL2 Mask:** `0x00003FFF00000000` (`70364449210368uL`)
  * Extract PL2: `(uint)((val & 0x3FFF00000000) >> 32) >> 3`
  * Write PL2: `(pl2_watts << 35)`
* **Tau (Time Window) Mask:** `0x0000000000FE0000` (`16646144uL`)
  * Extract Tau Code: `(uint)((val & 0xFE0000) >> 17)`
  * Write Tau Code: `(ulong)(tau_code << 17)`

### Tau Code to Seconds Lookup Table
| Seconds | MSR Encoded Code | Seconds | MSR Encoded Code |
| :---: | :---: | :---: | :---: |
| **1 s** | 10 | **20 s** | 46 |
| **2 s** | 11 | **24 s** | 78 |
| **3 s** | 75 | **28 s** | 110 |
| **4 s** | 12 | **32 s** | 15 |
| **5 s** | 43 | **40 s** | 47 |
| **6 s** | 76 | **48 s** | 79 |
| **7 s** | 108 | **56 s** | 111 |
| **8 s** | 13 | **64 s** | 16 |
| **10 s**| 45 | **80 s** | 48 |
| **12 s**| 77 | **96 s** | 80 |
| **14 s**| 109| **112 s**| 112 |
| **16 s**| 14 | **128 s**| 17 |

---

## 4. Fan Curve Table Layouts (`FanControl.cs`)

The fan curve system controls up to 6 fans (`FAN_MAX_NUMBER = 6`). Each fan channel contains a Primary and Secondary temperature input configuration block (14 bytes per fan in Gen 21/22):

### Byte Structure per Fan (14 Bytes)
* **Byte 0:** `PrimaryInputSourceConfiguration` (Sensor source: CPU, GPU, VRM, etc.)
* **Byte 1:** `PrimaryMaximumTemperature` (°C)
* **Byte 2:** `PrimaryMinimumTemperature` (°C)
* **Byte 3:** `PrimaryMinimumDuty` (%)
* **Byte 4:** `PrimaryDutyIncrement` (Slope / step)
* **Byte 5:** `PrimaryFanOffTemperature` (°C)
* **Byte 6:** `SecondaryInputSourceConfiguration`
* **Byte 7:** `SecondaryMaximumTemperature` (°C)
* **Byte 8:** `SecondaryMinimumTemperature` (°C)
* **Byte 9:** `SecondaryMinimumDuty` (%)
* **Byte 10:** `SecondaryDutyIncrement`
* **Byte 11:** `SecondaryFanOffTemperature` (°C)
* **Byte 12 & 13:** Fixed Mode flag & Manual Duty (Byte 12 = 1, Byte 13 = Duty in Gen 21; Byte 12 = `0x80 | duty` in Gen 22).

---

## 5. ACPI WMI LED & Lighting Architecture (`CISD_WMI`)

All RGB LEDs on the Phantom Canyon chassis (Skull, Eyes, Power Button, Light Bar) are controlled through `root\WMI` -> `CISD_WMI`.

### Methods in `CISD_WMI`
* `QueryLed(Cmd: uint32)` -> Returns capability mask, count of LEDs, and supported modes.
* `NewGetLedStatus(Cmd: uint32)` -> Returns active LED color / behavior.
* `SetIndicatorOptionLedType(Cmd: uint32)` -> Configures which event drives the LED (PowerState, HDD activity, Alexa, etc.).
* `SetValueIndicatorOptionLedType(Cmd: uint32)` -> Sets exact RGB colors, frequencies, and brightness levels.
* `AppNotify(Cmd: uint32)` -> Commits LED changes to BIOS / NVRAM non-volatile storage.

### LED Indices (`LedType` Enum)
* `PowerButton = 0`
* `Hdd = 1`
* `Skull = 2` (The large top/side skull plate on Phantom Canyon)
* `Eyes = 3` (The skull eye LEDs)
* `Front1 = 4`, `Front2 = 5`, `Front3 = 6`
* `RGBHeader = 7`

### Indicator Modes (`IndicatorType` Enum)
* `PowerState = 0` (Color changes depending on S0 on, S3 sleep, S5 off)
* `HddActivity = 1`
* `Ethernet = 2`
* `Wifi = 3`
* `Software = 4` (Static custom color controlled by app/OS)
* `PowerLimit = 5` (Changes color when CPU/GPU hits power limit)
* `Disable = 6` (LED fully off)
* `Alexa = 7`

### RGB Color & Behavior Indices (per Power State: S0 / S3 / Modern Standby / S5)
* `S0_BRIGHTNESS_INDEX = 0` (0 - 100%)
* `S0_BEHAVIOR_INDEX = 1` (`0` = Solid, `1` = Breathing, `2` = Pulsing, `3` = Strobing)
* `S0_FREQUENCY_INDEX = 2` (Hz speed)
* `S0_COLOR_RED_INDEX = 3` (0 - 255)
* `S0_COLOR_GREEN_INDEX = 4` (0 - 255)
* `S0_COLOR_BLUE_INDEX = 5` (0 - 255)

---

## 6. IPC WCF Endpoints (Local SOAP Services)

When running, the background service exposed local HTTP endpoints on localhost (unauthenticated):
* `http://localhost:43564/NssService/service/AppSettings`
* `http://localhost:43565/NssService/service/NucLed`
* `http://localhost:43566/NssService/service/SystemMonitor`
* `http://localhost:43568/NssService/service/PerformanceTuning`
