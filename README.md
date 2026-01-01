# WinPing Modern

> Network ping utility with live charting and CSV export.

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

## License

MIT License - See [LICENSE](../../LICENSE) in repository root.
