// WinPing Modern - Network Ping Utility
// Copyright (c) 2025 CosmicBytez
// Licensed under MIT License

using System.Net.NetworkInformation;

namespace WinPing;

/// <summary>
/// Represents a single ping result with timestamp and metrics.
/// </summary>
public sealed class PingResult
{
    /// <summary>
    /// Gets the sequence number of this ping.
    /// </summary>
    public int Sequence { get; init; }

    /// <summary>
    /// Gets the timestamp when the ping was sent.
    /// </summary>
    public DateTime Timestamp { get; init; }

    /// <summary>
    /// Gets the target host address.
    /// </summary>
    public string Host { get; init; } = string.Empty;

    /// <summary>
    /// Gets the resolved IP address.
    /// </summary>
    public string? IpAddress { get; init; }

    /// <summary>
    /// Gets the round-trip time in milliseconds, or -1 if failed.
    /// </summary>
    public long RoundtripTime { get; init; }

    /// <summary>
    /// Gets the ping status.
    /// </summary>
    public IPStatus Status { get; init; }

    /// <summary>
    /// Gets the TTL (Time To Live) value.
    /// </summary>
    public int Ttl { get; init; }

    /// <summary>
    /// Gets the buffer size used for the ping.
    /// </summary>
    public int BufferSize { get; init; }

    /// <summary>
    /// Gets a value indicating whether the ping was successful.
    /// </summary>
    public bool IsSuccess => Status == IPStatus.Success;

    /// <summary>
    /// Returns a formatted string representation for display.
    /// </summary>
    public override string ToString()
    {
        if (IsSuccess)
        {
            return $"[{Timestamp:HH:mm:ss.fff}] Reply from {IpAddress}: bytes={BufferSize} time={RoundtripTime}ms TTL={Ttl}";
        }
        return $"[{Timestamp:HH:mm:ss.fff}] {Host}: {Status}";
    }

    /// <summary>
    /// Returns a CSV-formatted line for export.
    /// </summary>
    public string ToCsvLine()
    {
        return $"{Sequence},{Timestamp:yyyy-MM-dd HH:mm:ss.fff},{Host},{IpAddress ?? "N/A"},{RoundtripTime},{Status},{Ttl},{BufferSize}";
    }

    /// <summary>
    /// Returns the CSV header line.
    /// </summary>
    public static string CsvHeader => "Sequence,Timestamp,Host,IPAddress,RoundtripMs,Status,TTL,BufferSize";
}
