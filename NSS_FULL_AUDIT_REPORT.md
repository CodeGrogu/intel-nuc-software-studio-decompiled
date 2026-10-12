# Comprehensive Architecture, Protocol & Security Audit: Intel NUC Software Studio

**Target Platform:** Intel(R) Client Systems NUC11PHi7 (Phantom Canyon / NUC 11 Enthusiast)  
**Host BIOS Identification:** `PHTGL579.0080.2025.1118.1734` (`PH` Family)  
**Hardware ID Node:** `ACPI\INTC1036`  
**Driver Binary:** `PerformanceDriver.sys` (WDM/KMDF kernel driver)  
**Installed Service:** `NucSoftwareStudioService` (Running under PID, `.NET Framework 4.6.1`)  

---

## 1. Executive Summary & Audit Findings

The audit evaluated all components across the suite:
1. **The Kernel Driver (`PerformanceDriver.sys`)**: Exposes raw Model-Specific Register (MSR) reads/writes and Embedded Controller (EC) fan/thermal access.
2. **The Background Service (`NucSoftwareStudioService.exe`)**: Runs as `NT AUTHORITY\SYSTEM`. Hosts WCF endpoints on local HTTP ports (`43562` - `43569`) with Windows Message security credentials.
3. **The ACPI WMI Provider (`CISD_WMI` in `root\WMI`)**: Hardware-level ACPI methods for RGB lighting control, HDMI CEC bridging, and BIOS profile persistence.
4. **The Client Front-End (`NucSoftwareStudio.exe` / `*.dll`)**: WPF MSIX container that binds to the local WCF endpoints and delegates all privileged operations.

---

## 2. Kernel Driver Layer Audit (`PerformanceDriver.sys`)

### Device Interface & Dispatch
- **Interface GUID:** `{1ACD29C0-C4DB-4542-9C73-9898B0D8FF6E}` (`GUID_DEVINTERFACE_NUCWDT`)
- **Synchronization:** Mutex `Global\Access_ISABUS.HTP.Method` acquired before dispatching EC/bus queries.

### Vulnerability / Risk Analysis
* **Arbitrary Power MSR Manipulation (`IOCTL 0x9C406014`):**
  The driver provides an unvalidated write path to CPU power limit registers (MSR `0x610` / `MSR_PKG_POWER_LIMIT`). While intended for PL1/PL2, writing malformed power bitfields can cause instant thermal throttling or system freeze.
* **Direct Fan Duty Override (`IOCTL 0x9C406040`):**
  Accepts raw duty cycles between 1% and 99%. A malicious process with access to the driver device node can stall fans indefinitely under load, triggering hardware thermal shutdown.
* **Privilege Level Required:** Administrative access (`GENERIC_READ | GENERIC_WRITE`) is required to open the driver symlink from userland.

---

## 3. IPC & Network Service Audit (`NucSoftwareStudioService.exe`)

### Exposed Ports & Protocol Bindings
The service opens 5 HTTP ports bound to `0.0.0.0` (all network interfaces, including LAN) using `System.ServiceModel.WSHttpBinding`:

| Port | Service Name | Contract Interface | Endpoint URI |
| :---: | :---: | :---: | :---: |
| **43562** | `AlexaSettingsService` | `IAlexaSettingsServiceContract` | `http://localhost:43562/NssService/service/AlexaSettings` |
| **43563** | `AlexaStateService` | `IAlexaStateServiceContract` | `http://localhost:43563/NssService/service/AlexaState` |
| **43564** | `AppSettingsService` | `IAppSettingsServiceContract` | `http://localhost:43564/NssService/service/AppSettings` |
| **43565** | `NucLedService` | `INucLedServiceContract` | `http://localhost:43565/NssService/service/NucLed` |
| **43566** | `SystemMonitorService` | `ISystemMonitorServiceContract` | `http://localhost:43566/NssService/service/SystemMonitor` |
| **43567** | `ArduinoService` | `IArduinoServiceContract` | `http://localhost:43567/NssService/service/Arduino` |
| **43568** | `PerformanceTuningService` | `IPerformanceTuningServiceContract` | `http://localhost:43568/NssService/service/PerformanceTuning` |
| **43569** | `EneService` | `IEneServiceContract` | `http://localhost:43569/NssService/service/Ene` |

### Security Evaluation of IPC
* **Binding Configuration:** Uses default `wsHttpBinding`, which enforces **SOAP 1.2 + WS-Security (MessageCredentialType = Windows)**.
* **Authentication Strength:** Unauthenticated HTTP POST requests from external tools or web browsers fail with `BadContextToken` / security context mismatch. Calls must negotiate Windows SSPI/Kerberos/NTLM tokens.
* **Local Privilege Escalation (LPE) Vector:**
  Any unprivileged local user session running on the machine that can establish a Windows authenticated WCF channel can call methods on `IPerformanceTuningServiceContract` and `INucLedServiceContract` to manipulate hardware power limits, fans, and firmware without UAC elevation, because the background service runs as `SYSTEM`.

---

## 4. Firmware & ACPI WMI Layer Audit (`CISD_WMI`)

### Namespace & Methods
* **Namespace:** `root\WMI`
* **Class:** `CISD_WMI`
* **Access Control:** Restricted by default WMI security descriptors. Standard users receive `Access to a CIM resource was not available`. Administrative / Elevated privileges are required to invoke methods directly.

### Persistence Mechanism (`AppNotify` / `NotificationLedApp`)
* The application writes LED color configurations to volatile memory registers and then triggers `AppNotify(Cmd: 0)`.
* This causes the BIOS/EC to commit current LED states to NVRAM, preserving lighting themes across system reboots and cold boots.

---

## 5. Reverse Engineering & Replacement Feasibility Assessment

| Component | Official Implementation | Custom Replacement Strategy | Feasibility |
| :--- | :--- | :--- | :---: |
| **Chassis RGB Lighting** | `NucLedMgr.dll` -> `CISD_WMI` | Direct elevated PowerShell / C# WMI invocation to `CISD_WMI` (`SetIndicatorOptionLedType`, `SetValueIndicatorOptionLedType`). No Intel services required. | **100% (High)** |
| **Power Limit (PL1 / PL2 / Tau)** | `PerformanceTuningLibrary` -> `PerformanceDriver.sys` | Can be controlled via: (A) WCF client to existing service, or (B) direct IOCTLs to `PerformanceDriver.sys`, or (C) native Intel MSR calls. | **100% (High)** |
| **Fan Curve & Speed Tuning** | `FanControl.cs` -> `PerformanceDriver.sys` | Issue direct IOCTL `0x9C406018` / `0x9C406040` with reconstructed 14-byte fan configuration packets. | **100% (High)** |
| **System Telemetry** | `SystemMonitorLibrary` | Use modern lightweight C# libraries (e.g., LibreHardwareMonitor) without the legacy 2022 service footprint. | **100% (High)** |
