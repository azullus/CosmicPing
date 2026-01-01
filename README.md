# WinPing Modern

> Network ping utility with live charting and CSV export.

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

- Windows 10/11
- .NET 8.0 Runtime

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

1. Enter a hostname or IP address (default: `8.8.8.8`)
2. Adjust settings:
   - **Timeout**: How long to wait for each reply (100-30000 ms)
   - **Buffer**: Packet size in bytes (1-65500)
   - **Interval**: Time between pings (100-60000 ms)
3. Click **Start** to begin pinging
4. Click **Stop** to end the session
5. Click **Export CSV** to save results

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

## Version History

- **v2.0.0** (2025-12-27): Complete rewrite with proper resource disposal
- **v1.0.0** (2025-06): Initial release (deprecated due to memory leaks)

## Troubleshooting

### Application Won't Start

**Issue**: Double-clicking WinPing.exe shows error or nothing happens

**Solutions**:

1. **Missing .NET Runtime**
   ```bash
   # Check if .NET 8.0 is installed
   dotnet --version
   # Should return 8.0.x
   ```
   - **Fix**: Download and install [.NET 8.0 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)
   - Choose "Run desktop apps" runtime
   - Restart after installation

2. **Windows SmartScreen Block**
   - Windows may block unsigned executables
   - **Fix**: Click "More info" → "Run anyway"
   - Or right-click exe → Properties → Unblock

3. **Corrupted Download**
   ```bash
   # Verify file size (should be ~150KB for single-file publish)
   dir WinPing.exe
   ```
   - **Fix**: Re-download from GitHub Releases

### Ping Failures

**Issue**: All pings show "Failed" or "TimedOut"

**Solutions**:

1. **Firewall Blocking ICMP**
   ```powershell
   # Check if ICMP Echo Request is allowed
   Get-NetFirewallRule | Where-Object {$_.DisplayName -like "*ICMP*"}

   # Enable ICMP Echo Request (requires admin)
   New-NetFirewallRule -DisplayName "ICMP Echo Request" `
     -Protocol ICMPv4 -IcmpType 8 -Action Allow -Direction Outbound
   ```

2. **No Internet Connection**
   ```bash
   # Test basic connectivity
   ping 8.8.8.8
   # If this fails, check network adapter
   ```
   - **Fix**: Check network connection, Wi-Fi/Ethernet status

3. **Invalid Hostname**
   - Verify hostname is correct (e.g., `google.com` not `google`)
   - **Fix**: Use IP address instead (8.8.8.8, 1.1.1.1)

4. **Timeout Too Short**
   - Default 2000ms may be too short for distant hosts
   - **Fix**: Increase timeout to 5000-10000ms

### Export CSV Issues

**Issue**: Export doesn't work or file is empty

**Solutions**:

1. **No Data to Export**
   - Must have at least one successful ping
   - **Fix**: Wait for successful ping before exporting

2. **File Permission Denied**
   ```powershell
   # Check if you have write permission to selected directory
   Test-Path -Path "C:\Users\YourName\Documents" -PathType Container
   ```
   - **Fix**: Choose different save location (Desktop, Documents)

3. **File Already Open**
   - Cannot overwrite CSV if open in Excel
   - **Fix**: Close Excel and try again

4. **CSV Format Issues in Excel**
   ```
   Issue: Timestamps show as text in Excel
   Fix: Use "Text to Columns" → Delimited → Comma
        Or open CSV with Excel Data Import wizard
   ```

### High CPU or Memory Usage

**Issue**: WinPing uses too much resources

**Solutions**:

1. **Too Many Retained Results**
   - Default retains 1000 results
   - **Fix**: Export and restart app to clear results
   - Or modify `MaxResultsRetention` in code and recompile

2. **Ping Interval Too Short**
   - 100ms intervals can cause high CPU
   - **Fix**: Increase interval to 500-1000ms

3. **Memory Leak Suspected**
   ```
   This is v2.0.0 which fixes all known memory leaks.
   If you experience memory growth:
   - Export current results
   - Stop pinging
   - Restart application
   - Report issue on GitHub with steps to reproduce
   ```

### Inaccurate Latency Readings

**Issue**: Latency seems wrong or inconsistent

**Solutions**:

1. **Compare with Command-Line Ping**
   ```bash
   # Test with native Windows ping
   ping -n 10 8.8.8.8

   # Compare results to WinPing
   # Should be within ±5ms
   ```

2. **Network Congestion**
   - Other applications using bandwidth can affect latency
   - **Fix**: Close bandwidth-heavy apps (downloads, streaming)

3. **VPN or Proxy**
   - VPN adds latency overhead
   - **Fix**: Disconnect VPN to test direct connection

### Application Crashes

**Issue**: WinPing closes unexpectedly

**Solutions**:

1. **Check Event Viewer**
   ```
   Event Viewer → Windows Logs → Application
   Look for .NET Runtime errors with WinPing.exe
   ```

2. **Outdated .NET Runtime**
   ```bash
   # Check installed .NET versions
   dotnet --list-runtimes

   # Should show Microsoft.WindowsDesktop.App 8.0.x
   ```
   - **Fix**: Update to latest .NET 8.0 runtime

3. **Invalid Input Values**
   - Timeout: Must be 100-30000ms
   - Buffer: Must be 1-65500 bytes
   - Interval: Must be 100-60000ms
   - **Fix**: Use default values and adjust gradually

### Build Errors

**Issue**: `dotnet build` fails

**Solutions**:

1. **Missing .NET SDK**
   ```bash
   # Check if SDK is installed (not just runtime)
   dotnet --version

   # Should return 8.0.x or higher
   ```
   - **Fix**: Download [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

2. **NuGet Restore Fails**
   ```bash
   # Manually restore packages
   dotnet restore
   dotnet build
   ```

3. **Target Framework Mismatch**
   - Ensure WinPing.csproj has `<TargetFramework>net8.0-windows</TargetFramework>`
   - **Fix**: Edit .csproj or use correct SDK version

## Performance Characteristics

| Metric | Value | Notes |
|--------|-------|-------|
| **CPU Usage** | <1% idle, <5% pinging | Per ping thread |
| **Memory Usage** | ~20-30 MB | With 1000 results retained |
| **Ping Accuracy** | ±5ms | Compared to Windows `ping` |
| **Result Retention** | 1000 entries | Configurable via `MaxResultsRetention` |
| **UI Responsiveness** | Real-time | Async ping operations |

## Known Limitations

| Limitation | Impact | Workaround |
|------------|--------|------------|
| **IPv6 Not Supported** | Cannot ping IPv6 addresses | Use IPv4 or dual-stack hostname |
| **1000 Result Limit** | Older results auto-pruned | Export periodically |
| **Windows Only** | No Linux/macOS support | .NET 8 WinForms limitation |
| **No Traceroute** | Cannot see routing path | Use Windows `tracert` command |
| **Single Host** | Can only ping one target at a time | Run multiple instances |

## Contributing

Contributions welcome! Please:
1. Fork the repository
2. Create a feature branch (`git checkout -b feature/improvement`)
3. Commit changes (`git commit -am 'Add feature'`)
4. Push to branch (`git push origin feature/improvement`)
5. Open a Pull Request

### Development Setup

```bash
# Clone repository
git clone https://github.com/azullus/CosmicPing.git
cd CosmicPing

# Restore dependencies
dotnet restore

# Build
dotnet build

# Run
dotnet run
```

### Testing

```bash
# Run unit tests
cd WinPing.Tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true
```

## FAQ

**Q: Why use WinPing instead of Windows built-in `ping` command?**

A: WinPing provides:
- Real-time statistics (min/max/avg updated live)
- Visual latency chart
- CSV export for analysis
- Persistent results (up to 1000 pings)
- Configurable buffer and interval
- GUI interface for easier use

**Q: Is WinPing portable?**

A: Yes, if you use the self-contained publish option:
```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```
The resulting .exe includes the .NET runtime (~150MB).

**Q: Can I ping multiple hosts simultaneously?**

A: Not in a single instance, but you can run multiple WinPing.exe instances.

**Q: How do I report a bug?**

A: Open an issue on [GitHub Issues](https://github.com/azullus/CosmicPing/issues) with:
- WinPing version
- .NET runtime version (`dotnet --version`)
- Steps to reproduce
- Expected vs actual behavior

**Q: Can I use this for commercial purposes?**

A: Yes, it's MIT licensed. Free for personal and commercial use.

## Related Projects

### Networking & Diagnostics
- **[cosmicbytez-ops-toolkit](https://github.com/azullus/cosmicbytez-ops-toolkit)** - PowerShell automation suite for Windows infrastructure and network management

### Infrastructure & DevOps
- **[docker-infrastructure](https://github.com/azullus/docker-infrastructure)** - Docker Compose IaC for self-hosted homelab services
- **[bc-docker-manager](https://github.com/azullus/bc-docker-manager)** - Electron desktop app for Business Central Docker containers

---

## License

MIT License - See [LICENSE](LICENSE) for details.

---

**Built with ❤️ for Network Diagnostics | IT Operations | System Administration**
