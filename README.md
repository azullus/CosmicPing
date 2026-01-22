# WinPing Modern

> .NET 8 C# network diagnostic tool with real-time latency charting, packet loss tracking, memory-safe architecture, and CSV export for IT professionals.

[![Release](https://img.shields.io/github/v/release/azullus/CosmicPing?logo=github)](https://github.com/azullus/CosmicPing/releases)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=.net&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12-239120?logo=csharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Windows](https://img.shields.io/badge/Windows-10%2F11-0078D6?logo=windows&logoColor=white)]()
[![License](https://img.shields.io/github/license/azullus/CosmicPing)](LICENSE)
[![Stars](https://img.shields.io/github/stars/azullus/CosmicPing?style=flat)](https://github.com/azullus/CosmicPing/stargazers)
[![Issues](https://img.shields.io/github/issues/azullus/CosmicPing)](https://github.com/azullus/CosmicPing/issues)
[![Last Commit](https://img.shields.io/github/last-commit/azullus/CosmicPing)](https://github.com/azullus/CosmicPing/commits/main)

## Features

- **Continuous Ping**: Ping any hostname or IP address with configurable intervals
- **Live Statistics**: Real-time min/max/avg latency and packet loss tracking
- **Visual Chart**: Text-based latency visualization
- **CSV Export**: Export results for analysis in Excel or other tools
- **Configurable**: Adjustable timeout, buffer size, and ping interval
- **Memory Safe**: Built-in result retention limits prevent memory leaks

## Requirements

| Component | Requirement |
|-----------|-------------|
| **OS** | Windows 10/11 (x64) |
| **Runtime** | .NET 8.0 Runtime ([Download](https://dotnet.microsoft.com/download/dotnet/8.0)) |
| **Privileges** | Standard user (no admin required) |
| **Network** | ICMP packets must be allowed by firewall |

---

## Quick Start

### Option 1: Download Pre-Built Executable

1. Download the latest release from [GitHub Releases](https://github.com/azullus/CosmicPing/releases)
2. Extract `WinPing.exe` to your desired location
3. Run `WinPing.exe` (no installation required)
4. Enter a hostname and click **Start**

### Option 2: Build from Source

```bash
# Clone repository (if standalone) or navigate to tools/WinPing
cd /path/to/tools/WinPing

# Build release version
dotnet build --configuration Release

# Run
dotnet run --configuration Release
```

---

## Building

```bash
# Build the project
dotnet build --configuration Release

# Run the application
dotnet run

# Publish as standalone executable
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

## Usage

### Basic Operation

1. **Enter Target**: Type a hostname or IP address (default: `8.8.8.8`)
   - Hostnames: `google.com`, `github.com`, `192.168.1.1`
   - IPv4: `8.8.8.8`, `1.1.1.1`
   - IPv6: Not currently supported

2. **Configure Settings**:

   | Setting | Range | Default | Description |
   |---------|-------|---------|-------------|
   | **Timeout** | 100-30000 ms | 5000 ms | Wait time for each reply |
   | **Buffer** | 1-65500 bytes | 32 bytes | Packet size (ICMP data) |
   | **Interval** | 100-60000 ms | 1000 ms | Delay between pings |

3. **Start Pinging**: Click **Start** button
   - Status updates in real-time
   - Statistics auto-update
   - Chart displays latency history

4. **Stop Session**: Click **Stop** button
   - Results remain visible
   - Can resume with same or different target
   - Maximum 1000 results retained (configurable)

5. **Export Data**: Click **Export CSV** button
   - Save to desktop or custom location
   - Filename: `ping_results_<timestamp>.csv`
   - Compatible with Excel, Google Sheets, data analysis tools

### Common Use Cases

#### Network Troubleshooting
```
Target: 192.168.1.1 (default gateway)
Timeout: 2000 ms
Buffer: 32 bytes
Interval: 500 ms
Purpose: Quick response time verification
```

#### ISP Connection Monitoring
```
Target: 8.8.8.8 (Google DNS)
Timeout: 5000 ms
Buffer: 32 bytes
Interval: 1000 ms (1 second)
Purpose: Long-term stability testing
```

#### Packet Loss Testing
```
Target: gaming-server.com
Timeout: 3000 ms
Buffer: 64 bytes
Interval: 100 ms (10 pings/sec)
Purpose: Gaming server performance
```

#### MTU Discovery (Manual)
```
Target: target-host.com
Timeout: 5000 ms
Buffer: Start at 1472, decrease by 8
Interval: 2000 ms
Purpose: Find maximum transmission unit
```

## Memory Management

This version addresses the memory leak issues from v1.0:

- **Ping objects**: Properly disposed using `using` statements
- **CancellationTokenSource**: Disposed in `finally` block and form disposal
- **Result retention**: Limited to 1000 entries (configurable via `MaxResultsRetention`)
- **Error handling**: No empty catch blocks; all errors properly logged

## Architecture

```
WinPing/
├── WinPing.sln           # Solution file
├── WinPing.csproj        # Project file (.NET 8)
├── Program.cs            # Entry point
├── MainForm.cs           # Main form logic
├── MainForm.Designer.cs  # UI layout
├── PingResult.cs         # Data model
└── README.md             # This file
```

## CSV Export Format

```csv
Sequence,Timestamp,Host,IPAddress,RoundtripMs,Status,TTL,BufferSize
1,2025-12-27 22:30:45.123,8.8.8.8,8.8.8.8,15,Success,117,32
2,2025-12-27 22:30:46.130,8.8.8.8,8.8.8.8,14,Success,117,32
```

---

## Troubleshooting

### Application Won't Start

**Issue**: Double-clicking WinPing.exe shows error or nothing happens

**Solutions**:

1. **Missing .NET Runtime**
   ```bash
   # Check if .NET 8.0 is installed
   dotnet --version

   # Should show: 8.0.x or higher
   ```
   - **Fix**: Download and install [.NET 8.0 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)
   - Choose: `.NET Desktop Runtime 8.0.x` (includes Windows Forms)

2. **Windows SmartScreen Block**
   - **Symptom**: "Windows protected your PC" message
   - **Fix**: Click "More info" → "Run anyway"
   - **Permanent Fix**: Code-sign the executable (for official releases)

3. **Antivirus False Positive**
   - **Symptom**: AV software blocks or deletes WinPing.exe
   - **Fix**: Add exception for WinPing.exe in antivirus settings
   - **Verification**: Check Windows Defender quarantine

### Ping Failures

**Issue**: All pings show "Failed" or "TimedOut"

**Solutions**:

1. **Firewall Blocking ICMP**
   ```powershell
   # Check Windows Firewall (PowerShell as Admin)
   Get-NetFirewallRule -DisplayName "*ICMP*" | Select-Object DisplayName, Enabled

   # Enable ICMP Echo Request
   New-NetFirewallRule -DisplayName "ICMP Echo Request" -Protocol ICMPv4 -IcmpType 8 -Action Allow
   ```

2. **Invalid Target**
   - **Check**: Hostname resolves correctly
   ```bash
   # Test DNS resolution
   nslookup google.com
   ping google.com -n 1
   ```
   - **Fix**: Use IP address directly (e.g., `8.8.8.8`) to rule out DNS issues

3. **Corporate Network Restrictions**
   - Some enterprise networks block ICMP entirely
   - **Fix**: Contact IT department for exemption
   - **Alternative**: Use TCP/HTTP-based monitoring tools

4. **Target Configured to Block ICMP**
   - Many servers block ping for security
   - **Example**: `github.com` does not respond to ping
   - **Fix**: Choose a different target (e.g., `8.8.8.8`, `1.1.1.1`)

### High Latency or Packet Loss

**Issue**: Results show high latency (>100ms) or packet loss (>5%)

**Diagnosis**:

1. **Test Multiple Targets**
   ```
   8.8.8.8 (Google DNS)       - Internet connectivity
   1.1.1.1 (Cloudflare DNS)   - Alternative ISP route
   192.168.1.1                - Local router (LAN health)
   ```

2. **Check Network Utilization**
   - Open Task Manager → Performance → Network
   - If bandwidth saturated (>80%), pause downloads/uploads

3. **Test with Different Buffer Sizes**
   - Small (32 bytes): Minimal overhead
   - Large (1472 bytes): Tests fragmentation/MTU issues

**Common Causes**:

- **WiFi Interference**: Switch to 5GHz band or use Ethernet
- **ISP Congestion**: Peak usage hours (evenings)
- **VPN Overhead**: Add 20-50ms latency typically
- **Router Issues**: Reboot router, check for firmware updates

### CSV Export Issues

**Issue**: Export button disabled or file not created

**Solutions**:

1. **No Data to Export**
   - Must have at least 1 ping result
   - **Fix**: Start ping session first

2. **File Permission Error**
   - Desktop folder not writable
   - **Fix**: Choose different save location (Documents, Downloads)

3. **Filename Conflicts**
   - File already open in Excel
   - **Fix**: Close Excel and retry, or choose new filename

### Application Crashes or Freezes

**Issue**: Application stops responding or crashes

**Solutions**:

1. **Long-Running Sessions**
   - After 1000 results, oldest entries are auto-pruned
   - **Best Practice**: Export data periodically for long tests

2. **DNS Resolution Timeout**
   - Hostname takes too long to resolve
   - **Fix**: Use IP address instead of hostname
   - Increase timeout to 10000ms (10 seconds)

3. **Memory Leak (v1.0 Only)**
   - **Solution**: Upgrade to v2.0.0 or later
   - v2.0 includes proper resource disposal

### Inaccurate Results

**Issue**: Latency values seem incorrect or inconsistent

**Checks**:

1. **System Load**
   - High CPU/disk usage affects timing
   - Close background applications

2. **Network Adapter**
   - **WiFi**: Inherently variable (+/- 10ms normal)
   - **Ethernet**: More consistent results

3. **Interval Too Short**
   - <100ms may cause queueing
   - **Fix**: Use 200ms+ interval for accurate measurements

4. **Compare with Command-Line Ping**
   ```bash
   ping 8.8.8.8 -n 10
   ```
   - Should match WinPing results within 5-10ms

---

## Performance & Best Practices

### Performance Characteristics

| Metric | Value | Notes |
|--------|-------|-------|
| **CPU Usage** | <1% idle, <5% active | Per ping thread |
| **Memory Usage** | ~20-30 MB | With 1000 results retained |
| **Startup Time** | <2 seconds | Cold start with .NET 8 |
| **Ping Accuracy** | ±5ms | Compared to Windows `ping` |
| **Max Ping Rate** | ~20/sec | At 50ms interval (not recommended) |

### Best Practices

**For Accurate Measurements:**

1. **Use Wired Connection**: WiFi adds variability
2. **Close Background Apps**: Especially downloads, updates, backups
3. **Reasonable Intervals**: 500ms-2000ms for stability testing
4. **Standard Buffer Size**: 32 bytes matches Windows `ping` default
5. **Multiple Tests**: Run 3-5 tests, average the results

**For Long-Term Monitoring:**

1. **Export Periodically**: Every 1000 pings (~16 minutes at 1-second intervals)
2. **Use Conservative Timeout**: 5000ms prevents false negatives
3. **Monitor System Resources**: Check Task Manager if results degrade
4. **Automate with Scripts**: Use PowerShell to run WinPing and process CSV

**For Packet Loss Detection:**

1. **Higher Ping Rate**: 200-500ms interval
2. **Sufficient Sample Size**: Run for 5-10 minutes minimum
3. **Compare Baselines**: Test during known-good conditions first
4. **Document Environment**: Note WiFi channel, router model, ISP

### Known Limitations

| Limitation | Impact | Workaround |
|------------|--------|------------|
| **IPv6 Not Supported** | Cannot ping IPv6 addresses | Use IPv4 address or dual-stack hostname |
| **Single Target** | Cannot ping multiple hosts simultaneously | Run multiple instances |
| **No Traceroute** | Cannot show hop-by-hop path | Use `tracert` command separately |
| **Windows Only** | Not cross-platform | Use platform-specific tools on Linux/Mac |
| **ICMP Only** | Some hosts block ICMP | Use TCP-based monitoring for those hosts |
| **1000 Result Limit** | Older results auto-pruned | Export periodically for longer tests |
| **No Scheduling** | Cannot run unattended | Use Task Scheduler to launch at intervals |

---

## Advanced Features

### Interpreting Statistics

**Min/Max/Avg Latency:**
- **Min**: Best-case latency (ideal network conditions)
- **Max**: Worst-case spike (investigate if >3x min)
- **Avg**: Typical latency (most useful metric)

**Packet Loss %:**
- **0%**: Perfect (ideal)
- **0-1%**: Excellent (expected on WiFi)
- **1-5%**: Acceptable (investigate if consistent)
- **>5%**: Poor (action required)

**TTL (Time To Live):**
- Decrements at each router hop
- **64**: Common starting value (Linux servers)
- **128**: Common starting value (Windows servers)
- **255**: Rare (network devices)
- Calculate hops: `Starting TTL - Current TTL`

### Customizing Result Retention

Edit `MainForm.cs` line ~20:

```csharp
private const int MaxResultsRetention = 1000; // Change to 5000 for longer sessions
```

Rebuild after modification:
```bash
dotnet build --configuration Release
```

**Trade-offs:**
- Higher values: More memory usage, longer scrolling
- Lower values: Less history, more frequent exports needed

### Integration Examples

**PowerShell Automation:**
```powershell
# Launch WinPing and auto-export after 5 minutes
Start-Process "C:\Tools\WinPing\WinPing.exe"
Start-Sleep -Seconds 300
# Note: Auto-export requires UI automation (not natively supported)
```

**CSV Data Analysis (Python):**
```python
import pandas as pd

# Load WinPing export
df = pd.read_csv('ping_results_20250101_120000.csv')

# Calculate statistics
print(f"Average Latency: {df['RoundtripMs'].mean():.2f}ms")
print(f"Packet Loss: {(df['Status'] != 'Success').sum() / len(df) * 100:.1f}%")

# Plot latency over time
df.plot(x='Sequence', y='RoundtripMs', title='Latency Over Time')
```

---

## Contributing

We welcome contributions! Here's how to help:

### Development Setup

1. **Clone Repository**
   ```bash
   git clone https://github.com/azullus/cosmicbytez-ops-toolkit.git
   cd cosmicbytez-ops-toolkit/tools/WinPing
   ```

2. **Install Prerequisites**
   - Visual Studio 2022 or VS Code with C# extension
   - .NET 8.0 SDK ([Download](https://dotnet.microsoft.com/download/dotnet/8.0))

3. **Open Solution**
   ```bash
   # Visual Studio
   start WinPing.sln

   # VS Code
   code .
   ```

4. **Build and Run**
   ```bash
   dotnet build
   dotnet run
   ```

### Code Style

- **Formatting**: Follow standard C# conventions (PascalCase, 4-space indent)
- **Comments**: XML documentation for public methods
- **Error Handling**: Always dispose resources (`using` statements)
- **Testing**: Manually verify ping accuracy against Windows `ping` command

### Pull Request Process

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/awesome-feature`)
3. Make your changes
4. Test thoroughly (different targets, buffer sizes, timeout scenarios)
5. Commit with clear message (`git commit -m 'Add awesome feature'`)
6. Push to your fork (`git push origin feature/awesome-feature`)
7. Open a Pull Request

### Feature Requests

Have an idea? Open an issue with:
- **Use Case**: What problem does it solve?
- **Proposed Solution**: How should it work?
- **Alternatives Considered**: Other approaches you've thought of

**Ideas for Contribution:**
- IPv6 support
- Multi-target parallel ping
- Graphical latency chart (not just text)
- Auto-export on threshold (e.g., every 1000 pings)
- Configurable alert thresholds
- Dark mode UI theme

---

## FAQ

**Q: Why use WinPing instead of Windows built-in `ping` command?**

A: WinPing provides:
- Real-time statistics (min/max/avg updated live)
- Visual latency history
- CSV export for analysis
- Persistent results (up to 1000 pings)
- Configurable settings via GUI

**Q: Does WinPing work on Windows Server?**

A: Yes, as long as .NET 8.0 Runtime is installed and Windows Forms is enabled.

**Q: Can I run WinPing on a schedule?**

A: Use Windows Task Scheduler to launch WinPing.exe at specific times. Note that it requires UI interaction to start/stop ping sessions.

**Q: Why doesn't it support IPv6?**

A: Current version uses `System.Net.NetworkInformation.Ping` which defaults to IPv4. IPv6 support planned for v3.0.

**Q: Is WinPing portable?**

A: Yes, if you use the self-contained publish option. Copy `WinPing.exe` to any Windows PC and run (no .NET installation needed, but larger file size ~70MB).

**Q: Can I change the chart colors?**

A: Not currently configurable via UI. Fork the repo and modify `MainForm.cs` if needed.

**Q: How accurate is the timestamp?**

A: Timestamps use `DateTime.Now` with millisecond precision (~15ms resolution on most systems).

---

## Version History

- **v2.0.0** (2025-12-27): Complete rewrite with proper resource disposal
- **v1.0.0** (2025-06): Initial release (deprecated due to memory leaks)

---

## 🔗 Related Projects

### IT Operations Toolkit
- **[cosmicbytez-ops-toolkit](https://github.com/azullus/cosmicbytez-ops-toolkit)** - PowerShell automation + C# utilities (CosmicPing is part of this toolkit)
- **[bc-docker-manager](https://github.com/azullus/bc-docker-manager)** - Electron desktop app for Business Central container management

### Infrastructure & Homelab
- **[docker-infrastructure](https://github.com/azullus/docker-infrastructure)** - Docker Compose IaC for self-hosted infrastructure
- **[cosmicbytez-homelab](https://github.com/azullus/cosmicbytez-homelab)** - Homelab documentation and security research

---

## License

MIT License - See [LICENSE](../../LICENSE) in repository root.
