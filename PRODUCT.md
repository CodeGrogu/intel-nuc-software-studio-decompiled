# NUC Control

A native Windows utility for this Phantom Canyon NUC11PHi7. The owner needs to see temperatures and fan speeds, select firmware power and cooling presets, and edit one light without navigating a broad tuning suite.

The first release uses WinUI 3, a portable frontend, and the existing independently installed Rust service. Controls use verified firmware paths and explicit readback. Unknown data stays unknown. Startup and reconnect never apply saved hardware settings.

The approved interface is compact and inspired by G-Helper's simplicity: separate power, cooling, and lighting sections, no sidebar, charts, or dashboard cards. Effects, custom fan curves, Ethernet, BIOS saving, and automated profiles are excluded.
