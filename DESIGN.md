# NUC Control interface

The app is a native Operate surface for a desktop owner making occasional hardware adjustments. It follows the user's Windows light/dark/high-contrast preferences and uses standard WinUI controls, Segoe typography, theme resources, and normal window chrome.

One scrollable column groups device/connection status, CPU/GPU readings, Power, Cooling, and Lighting. Dividers separate tasks. There is no sidebar, chart grid, or card dashboard. The title is 24 DIPs, section headings 16, and ordinary text uses native sizing. Content has 24-DIP side padding and 12-DIP group spacing.

Power and cooling use equal-width toggle buttons. Lighting uses a named physical zone, brightness slider, color dialog, and explicit Apply/Discard. Unsaved changes remain visible. Manual role changes are explained beside the editor. Disabled controls retain a reason; missing readings show Unavailable rather than zero.

The application icon is an original three-light chassis mark with recorded provenance. The settings glyph comes from the Windows font icon system. Motion and interaction feedback use native control behavior.

The first desktop inspection confirmed the compact composition and accessibility labels in the current system theme. It also identified periodic button recreation, which was corrected to preserve focus. Broader theme, high-contrast, and 150/200% scaling checks require additional environment validation; no passing result is claimed for them.
