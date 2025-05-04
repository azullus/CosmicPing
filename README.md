![cosmicping_readme_icon_128](https://github.com/user-attachments/assets/7d7610fd-7d2f-4f88-84ba-bb15b9dfc218)


**CosmicPingModern** is a modern, lightweight, multi‑host ping monitoring tool for Windows. It builds on the original WinPing utility by **WinPing** and leverages the WinForms DataVisualization chart to plot live ping response times for multiple hosts simultaneously.

---

## Features

* **Multi‑host support**: Monitor pings to as many targets as you like, each displayed in its own colored line.
* **Interactive chart**: Zoom, pan, and scroll with mouse selection and wheel support.
* **Time‑range buttons**: Quick‑access buttons for 1H, 6H, 12H, and 24H views of your data.
* **Customizable**: Adjust packet size, timeout, and count before starting.
* **Live log & export**: View real‑time text logs and export your session to CSV with:

  * **Summary** of attempts, drops, max/avg latencies
  * **Events** list with timestamp, latency, & drop flag
  * **Full log** of every ping attempt
* **Portable**: Single‑file, self‑contained executable for easy distribution.

---

## Usage

1. **Launch** the `CosmicPingModern.exe` executable.
2. **Enter** one hostname or IP per line in the **Hosts** panel.
3. **Set** packet size, timeout (ms), and count (0 = infinite).
4. **Click** **Start** to begin pinging and plotting.
5. **Use** time‑range buttons to focus on recent history.
6. **Export** your data at any time via the **Export** button.

---

## Installation

1. **Download** the latest release from the [[Releases](https://github.com/azullus/CosmicPing/releases)](https://github.com/azullus/CosmicPing/releases) page.
2. **Run** `CosmicPingModern.exe` — no installation required.

---

## Credits

* **WinPing** by **WinPing**: the original ping visualizer that inspired this GUI.
---

*Powered by .NET 7.0 and WinForms.DataVisualization*
