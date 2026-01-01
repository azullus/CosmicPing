// WinPing Modern - Network Ping Utility
// Copyright (c) 2025 CosmicBytez
// Licensed under MIT License

using System.Net.NetworkInformation;

namespace WinPing.Tests;

/// <summary>
/// Unit tests for the PingResult class.
/// Tests ToString formatting, CSV generation, and property initialization.
/// </summary>
public class PingResultTests
{
    #region ToString Tests

    [Fact]
    public void ToString_SuccessfulPing_ReturnsFormattedReply()
    {
        // Arrange
        var timestamp = new DateTime(2025, 1, 15, 14, 30, 45, 123);
        var result = new PingResult
        {
            Sequence = 1,
            Timestamp = timestamp,
            Host = "google.com",
            IpAddress = "142.250.185.46",
            RoundtripTime = 25,
            Status = IPStatus.Success,
            Ttl = 57,
            BufferSize = 32
        };

        // Act
        var output = result.ToString();

        // Assert
        Assert.Equal("[14:30:45.123] Reply from 142.250.185.46: bytes=32 time=25ms TTL=57", output);
    }

    [Fact]
    public void ToString_FailedPing_ReturnsStatusMessage()
    {
        // Arrange
        var timestamp = new DateTime(2025, 1, 15, 14, 30, 45, 456);
        var result = new PingResult
        {
            Sequence = 2,
            Timestamp = timestamp,
            Host = "unreachable.local",
            IpAddress = null,
            RoundtripTime = -1,
            Status = IPStatus.TimedOut,
            Ttl = 0,
            BufferSize = 32
        };

        // Act
        var output = result.ToString();

        // Assert
        Assert.Equal("[14:30:45.456] unreachable.local: TimedOut", output);
    }

    [Fact]
    public void ToString_DestinationHostUnreachable_ReturnsStatusMessage()
    {
        // Arrange
        var timestamp = new DateTime(2025, 1, 15, 9, 15, 30, 789);
        var result = new PingResult
        {
            Sequence = 3,
            Timestamp = timestamp,
            Host = "192.168.1.99",
            IpAddress = null,
            RoundtripTime = -1,
            Status = IPStatus.DestinationHostUnreachable,
            Ttl = 0,
            BufferSize = 64
        };

        // Act
        var output = result.ToString();

        // Assert
        Assert.Equal("[09:15:30.789] 192.168.1.99: DestinationHostUnreachable", output);
    }

    [Fact]
    public void ToString_PreservesMillisecondPrecision()
    {
        // Arrange - Test millisecond formatting with leading zeros
        var timestamp = new DateTime(2025, 1, 15, 1, 2, 3, 4);
        var result = new PingResult
        {
            Sequence = 1,
            Timestamp = timestamp,
            Host = "test.local",
            IpAddress = "10.0.0.1",
            RoundtripTime = 1,
            Status = IPStatus.Success,
            Ttl = 64,
            BufferSize = 32
        };

        // Act
        var output = result.ToString();

        // Assert
        Assert.Contains("[01:02:03.004]", output);
    }

    #endregion

    #region CSV Tests

    [Fact]
    public void ToCsvLine_SuccessfulPing_ReturnsCorrectFormat()
    {
        // Arrange
        var timestamp = new DateTime(2025, 1, 15, 14, 30, 45, 123);
        var result = new PingResult
        {
            Sequence = 1,
            Host = "google.com",
            Timestamp = timestamp,
            IpAddress = "142.250.185.46",
            RoundtripTime = 25,
            Status = IPStatus.Success,
            Ttl = 57,
            BufferSize = 32
        };

        // Act
        var csvLine = result.ToCsvLine();

        // Assert
        Assert.Equal("1,2025-01-15 14:30:45.123,google.com,142.250.185.46,25,Success,57,32", csvLine);
    }

    [Fact]
    public void ToCsvLine_FailedPing_ShowsNegativeRoundtripTime()
    {
        // Arrange
        var timestamp = new DateTime(2025, 1, 15, 14, 30, 45, 456);
        var result = new PingResult
        {
            Sequence = 2,
            Timestamp = timestamp,
            Host = "unreachable.local",
            IpAddress = null,
            RoundtripTime = -1,
            Status = IPStatus.TimedOut,
            Ttl = 0,
            BufferSize = 32
        };

        // Act
        var csvLine = result.ToCsvLine();

        // Assert
        Assert.Equal("2,2025-01-15 14:30:45.456,unreachable.local,N/A,-1,TimedOut,0,32", csvLine);
    }

    [Fact]
    public void ToCsvLine_NullIpAddress_ShowsNA()
    {
        // Arrange
        var timestamp = new DateTime(2025, 1, 15, 10, 0, 0, 0);
        var result = new PingResult
        {
            Sequence = 5,
            Timestamp = timestamp,
            Host = "test.local",
            IpAddress = null,
            RoundtripTime = -1,
            Status = IPStatus.DestinationNetworkUnreachable,
            Ttl = 0,
            BufferSize = 48
        };

        // Act
        var csvLine = result.ToCsvLine();

        // Assert
        Assert.Contains(",N/A,", csvLine);
    }

    [Fact]
    public void ToCsvLine_LargeSequenceNumber_HandlesCorrectly()
    {
        // Arrange
        var timestamp = new DateTime(2025, 1, 15, 23, 59, 59, 999);
        var result = new PingResult
        {
            Sequence = 99999,
            Timestamp = timestamp,
            Host = "longrunning.test",
            IpAddress = "192.168.1.1",
            RoundtripTime = 1234,
            Status = IPStatus.Success,
            Ttl = 128,
            BufferSize = 65500
        };

        // Act
        var csvLine = result.ToCsvLine();

        // Assert
        Assert.StartsWith("99999,", csvLine);
        Assert.Contains(",65500", csvLine);
    }

    [Fact]
    public void CsvHeader_ReturnsCorrectFormat()
    {
        // Act
        var header = PingResult.CsvHeader;

        // Assert
        Assert.Equal("Sequence,Timestamp,Host,IPAddress,RoundtripMs,Status,TTL,BufferSize", header);
    }

    #endregion

    #region Property Tests

    [Fact]
    public void IsSuccess_StatusSuccess_ReturnsTrue()
    {
        // Arrange
        var result = new PingResult
        {
            Status = IPStatus.Success,
            RoundtripTime = 10
        };

        // Act & Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void IsSuccess_StatusTimedOut_ReturnsFalse()
    {
        // Arrange
        var result = new PingResult
        {
            Status = IPStatus.TimedOut,
            RoundtripTime = -1
        };

        // Act & Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void IsSuccess_StatusDestinationHostUnreachable_ReturnsFalse()
    {
        // Arrange
        var result = new PingResult
        {
            Status = IPStatus.DestinationHostUnreachable,
            RoundtripTime = -1
        };

        // Act & Assert
        Assert.False(result.IsSuccess);
    }

    [Theory]
    [InlineData(IPStatus.Success, true)]
    [InlineData(IPStatus.TimedOut, false)]
    [InlineData(IPStatus.DestinationHostUnreachable, false)]
    [InlineData(IPStatus.DestinationNetworkUnreachable, false)]
    [InlineData(IPStatus.DestinationUnreachable, false)]
    [InlineData(IPStatus.TtlExpired, false)]
    [InlineData(IPStatus.BadDestination, false)]
    public void IsSuccess_VariousStatuses_ReturnsExpectedResult(IPStatus status, bool expectedSuccess)
    {
        // Arrange
        var result = new PingResult { Status = status };

        // Act & Assert
        Assert.Equal(expectedSuccess, result.IsSuccess);
    }

    [Fact]
    public void Properties_InitWithValues_StoreCorrectly()
    {
        // Arrange
        var timestamp = DateTime.Now;
        var result = new PingResult
        {
            Sequence = 42,
            Timestamp = timestamp,
            Host = "example.com",
            IpAddress = "93.184.216.34",
            RoundtripTime = 50,
            Status = IPStatus.Success,
            Ttl = 64,
            BufferSize = 128
        };

        // Assert
        Assert.Equal(42, result.Sequence);
        Assert.Equal(timestamp, result.Timestamp);
        Assert.Equal("example.com", result.Host);
        Assert.Equal("93.184.216.34", result.IpAddress);
        Assert.Equal(50, result.RoundtripTime);
        Assert.Equal(IPStatus.Success, result.Status);
        Assert.Equal(64, result.Ttl);
        Assert.Equal(128, result.BufferSize);
    }

    [Fact]
    public void Host_DefaultValue_IsEmptyString()
    {
        // Arrange & Act
        var result = new PingResult();

        // Assert
        Assert.Equal(string.Empty, result.Host);
    }

    [Fact]
    public void IpAddress_DefaultValue_IsNull()
    {
        // Arrange & Act
        var result = new PingResult();

        // Assert
        Assert.Null(result.IpAddress);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void ToString_ZeroRoundtripTime_DisplaysCorrectly()
    {
        // Arrange - Testing very fast local response
        var timestamp = new DateTime(2025, 1, 15, 12, 0, 0, 0);
        var result = new PingResult
        {
            Timestamp = timestamp,
            IpAddress = "127.0.0.1",
            RoundtripTime = 0,
            Status = IPStatus.Success,
            Ttl = 128,
            BufferSize = 32
        };

        // Act
        var output = result.ToString();

        // Assert
        Assert.Contains("time=0ms", output);
    }

    [Fact]
    public void ToCsvLine_SpecialCharactersInHost_HandlesCorrectly()
    {
        // Arrange - Test with hostname containing valid characters
        var timestamp = new DateTime(2025, 1, 15, 12, 0, 0, 0);
        var result = new PingResult
        {
            Sequence = 1,
            Timestamp = timestamp,
            Host = "server-01.sub-domain.example.com",
            IpAddress = "10.0.0.1",
            RoundtripTime = 5,
            Status = IPStatus.Success,
            Ttl = 64,
            BufferSize = 32
        };

        // Act
        var csvLine = result.ToCsvLine();

        // Assert
        Assert.Contains("server-01.sub-domain.example.com", csvLine);
    }

    [Fact]
    public void ToCsvLine_IPv6Address_HandlesCorrectly()
    {
        // Arrange
        var timestamp = new DateTime(2025, 1, 15, 12, 0, 0, 0);
        var result = new PingResult
        {
            Sequence = 1,
            Timestamp = timestamp,
            Host = "ipv6.google.com",
            IpAddress = "2607:f8b0:4004:c07::66",
            RoundtripTime = 30,
            Status = IPStatus.Success,
            Ttl = 57,
            BufferSize = 32
        };

        // Act
        var csvLine = result.ToCsvLine();

        // Assert
        Assert.Contains("2607:f8b0:4004:c07::66", csvLine);
    }

    #endregion
}
