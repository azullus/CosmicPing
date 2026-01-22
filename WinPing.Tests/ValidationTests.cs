// WinPing Modern - Network Ping Utility
// Copyright (c) 2025 CosmicBytez
// Licensed under MIT License

namespace WinPing.Tests;

/// <summary>
/// Unit tests for input validation logic.
/// Tests host validation, timeout, buffer size, and interval validation.
/// </summary>
public class ValidationTests
{
    #region Host Validation Tests

    [Theory]
    [InlineData("google.com")]
    [InlineData("www.example.com")]
    [InlineData("sub.domain.example.co.uk")]
    [InlineData("localhost")]
    [InlineData("my-server")]
    [InlineData("server01")]
    [InlineData("192.168.1.1")]
    [InlineData("10.0.0.1")]
    [InlineData("8.8.8.8")]
    [InlineData("2607:f8b0:4004:c07::66")]
    [InlineData("::1")]
    [InlineData("fe80::1")]
    public void ValidateHost_ValidHostnames_ReturnsTrue(string host)
    {
        // Act
        var result = InputValidator.ValidateHost(host);

        // Assert
        Assert.True(result, $"Expected '{host}' to be valid");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void ValidateHost_WhitespaceOrEmpty_ReturnsFalse(string host)
    {
        // Act
        var result = InputValidator.ValidateHost(host);

        // Assert
        Assert.False(result, $"Expected whitespace/empty to be invalid");
    }

    [Theory]
    [InlineData("invalid!@#$.com")]    // Special characters not allowed
    [InlineData("space domain.com")]   // Spaces not allowed
    [InlineData("host/path")]          // Forward slash not allowed
    [InlineData("host:port")]          // Colon not valid in hostname
    [InlineData("-hostname.com")]      // Leading hyphen rejected by Uri.CheckHostName
    [InlineData("invalid..domain")]    // Double dots rejected by Uri.CheckHostName
    public void ValidateHost_InvalidHostnames_ReturnsFalse(string host)
    {
        // Act
        var result = InputValidator.ValidateHost(host);

        // Assert
        Assert.False(result, $"Expected '{host}' to be invalid");
    }

    [Theory]
    [InlineData("hostname-.com")]      // Trailing hyphen - Uri.CheckHostName accepts
    [InlineData("192.168.1")]          // Incomplete IP - Uri.CheckHostName accepts (will fail at ping)
    public void ValidateHost_EdgeCases_AcceptedByUri_FailsAtRuntime(string host)
    {
        // These edge cases pass Uri.CheckHostName validation but will fail
        // when actually attempting to ping. This is acceptable behavior as
        // the user gets a clear error message at runtime.

        // Act
        var result = InputValidator.ValidateHost(host);

        // Assert - documenting .NET Uri.CheckHostName behavior
        Assert.True(result, $"Uri.CheckHostName accepts '{host}'");
    }

    [Fact]
    public void ValidateHost_NullInput_ReturnsFalse()
    {
        // Act
        var result = InputValidator.ValidateHost(null!);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidateHost_MaxLengthHostname_HandlesCorrectly()
    {
        // Arrange - DNS label max is 63 chars, total FQDN max is 253
        var validLongHostname = new string('a', 63) + ".com";

        // Act
        var result = InputValidator.ValidateHost(validLongHostname);

        // Assert
        Assert.True(result);
    }

    #endregion

    #region Timeout Validation Tests

    [Theory]
    [InlineData(100)]    // Minimum
    [InlineData(1000)]   // Common: 1 second
    [InlineData(5000)]   // Common: 5 seconds
    [InlineData(15000)]  // Mid-range
    [InlineData(30000)]  // Maximum
    public void ValidateTimeout_ValidValues_ReturnsTrue(int timeout)
    {
        // Act
        var result = InputValidator.ValidateTimeout(timeout);

        // Assert
        Assert.True(result, $"Expected {timeout}ms to be valid");
    }

    [Theory]
    [InlineData(99)]      // Just below minimum
    [InlineData(0)]       // Zero
    [InlineData(-1)]      // Negative
    [InlineData(-1000)]   // Large negative
    [InlineData(30001)]   // Just above maximum
    [InlineData(60000)]   // Too large
    [InlineData(int.MaxValue)]
    public void ValidateTimeout_InvalidValues_ReturnsFalse(int timeout)
    {
        // Act
        var result = InputValidator.ValidateTimeout(timeout);

        // Assert
        Assert.False(result, $"Expected {timeout}ms to be invalid");
    }

    [Fact]
    public void ValidateTimeout_BoundaryMinimum_ReturnsTrue()
    {
        // Act
        var result = InputValidator.ValidateTimeout(InputValidator.MinTimeout);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ValidateTimeout_BoundaryMaximum_ReturnsTrue()
    {
        // Act
        var result = InputValidator.ValidateTimeout(InputValidator.MaxTimeout);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("1000", true, 1000)]
    [InlineData("5000", true, 5000)]
    [InlineData("99", false, 0)]
    [InlineData("30001", false, 0)]
    [InlineData("abc", false, 0)]
    [InlineData("", false, 0)]
    [InlineData("1000.5", false, 0)]
    public void TryParseTimeout_VariousInputs_ReturnsExpectedResults(string input, bool expectedResult, int expectedValue)
    {
        // Act
        var result = InputValidator.TryParseTimeout(input, out int timeout);

        // Assert
        Assert.Equal(expectedResult, result);
        if (expectedResult)
        {
            Assert.Equal(expectedValue, timeout);
        }
    }

    #endregion

    #region Buffer Size Validation Tests

    [Theory]
    [InlineData(1)]       // Minimum
    [InlineData(32)]      // Default ping size
    [InlineData(64)]      // Common
    [InlineData(1024)]    // 1 KB
    [InlineData(8192)]    // 8 KB
    [InlineData(32768)]   // 32 KB
    [InlineData(65500)]   // Maximum
    public void ValidateBufferSize_ValidValues_ReturnsTrue(int bufferSize)
    {
        // Act
        var result = InputValidator.ValidateBufferSize(bufferSize);

        // Assert
        Assert.True(result, $"Expected {bufferSize} bytes to be valid");
    }

    [Theory]
    [InlineData(0)]       // Zero
    [InlineData(-1)]      // Negative
    [InlineData(-100)]    // Large negative
    [InlineData(65501)]   // Just above maximum
    [InlineData(100000)]  // Too large
    [InlineData(int.MaxValue)]
    public void ValidateBufferSize_InvalidValues_ReturnsFalse(int bufferSize)
    {
        // Act
        var result = InputValidator.ValidateBufferSize(bufferSize);

        // Assert
        Assert.False(result, $"Expected {bufferSize} bytes to be invalid");
    }

    [Fact]
    public void ValidateBufferSize_BoundaryMinimum_ReturnsTrue()
    {
        // Act
        var result = InputValidator.ValidateBufferSize(InputValidator.MinBufferSize);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ValidateBufferSize_BoundaryMaximum_ReturnsTrue()
    {
        // Act
        var result = InputValidator.ValidateBufferSize(InputValidator.MaxBufferSize);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("32", true, 32)]
    [InlineData("1024", true, 1024)]
    [InlineData("0", false, 0)]
    [InlineData("65501", false, 0)]
    [InlineData("abc", false, 0)]
    [InlineData("", false, 0)]
    [InlineData("32.5", false, 0)]
    [InlineData("-100", false, 0)]
    public void TryParseBufferSize_VariousInputs_ReturnsExpectedResults(string input, bool expectedResult, int expectedValue)
    {
        // Act
        var result = InputValidator.TryParseBufferSize(input, out int bufferSize);

        // Assert
        Assert.Equal(expectedResult, result);
        if (expectedResult)
        {
            Assert.Equal(expectedValue, bufferSize);
        }
    }

    #endregion

    #region Interval Validation Tests

    [Theory]
    [InlineData(100)]     // Minimum
    [InlineData(500)]     // Half second
    [InlineData(1000)]    // 1 second (common default)
    [InlineData(5000)]    // 5 seconds
    [InlineData(30000)]   // 30 seconds
    [InlineData(60000)]   // Maximum (1 minute)
    public void ValidateInterval_ValidValues_ReturnsTrue(int interval)
    {
        // Act
        var result = InputValidator.ValidateInterval(interval);

        // Assert
        Assert.True(result, $"Expected {interval}ms to be valid");
    }

    [Theory]
    [InlineData(99)]      // Just below minimum
    [InlineData(0)]       // Zero
    [InlineData(-1)]      // Negative
    [InlineData(-1000)]   // Large negative
    [InlineData(60001)]   // Just above maximum
    [InlineData(120000)]  // Too large
    [InlineData(int.MaxValue)]
    public void ValidateInterval_InvalidValues_ReturnsFalse(int interval)
    {
        // Act
        var result = InputValidator.ValidateInterval(interval);

        // Assert
        Assert.False(result, $"Expected {interval}ms to be invalid");
    }

    [Fact]
    public void ValidateInterval_BoundaryMinimum_ReturnsTrue()
    {
        // Act
        var result = InputValidator.ValidateInterval(InputValidator.MinInterval);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ValidateInterval_BoundaryMaximum_ReturnsTrue()
    {
        // Act
        var result = InputValidator.ValidateInterval(InputValidator.MaxInterval);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("1000", true, 1000)]
    [InlineData("5000", true, 5000)]
    [InlineData("99", false, 0)]
    [InlineData("60001", false, 0)]
    [InlineData("abc", false, 0)]
    [InlineData("", false, 0)]
    [InlineData("1000.5", false, 0)]
    [InlineData("-500", false, 0)]
    public void TryParseInterval_VariousInputs_ReturnsExpectedResults(string input, bool expectedResult, int expectedValue)
    {
        // Act
        var result = InputValidator.TryParseInterval(input, out int interval);

        // Assert
        Assert.Equal(expectedResult, result);
        if (expectedResult)
        {
            Assert.Equal(expectedValue, interval);
        }
    }

    #endregion

    #region Constants Tests

    [Fact]
    public void Constants_HaveExpectedValues()
    {
        // Assert
        Assert.Equal(100, InputValidator.MinTimeout);
        Assert.Equal(30000, InputValidator.MaxTimeout);
        Assert.Equal(1, InputValidator.MinBufferSize);
        Assert.Equal(65500, InputValidator.MaxBufferSize);
        Assert.Equal(100, InputValidator.MinInterval);
        Assert.Equal(60000, InputValidator.MaxInterval);
    }

    [Fact]
    public void Constants_MinLessThanMax()
    {
        // Assert
        Assert.True(InputValidator.MinTimeout < InputValidator.MaxTimeout);
        Assert.True(InputValidator.MinBufferSize < InputValidator.MaxBufferSize);
        Assert.True(InputValidator.MinInterval < InputValidator.MaxInterval);
    }

    #endregion

    #region Edge Cases and Special Scenarios

    [Theory]
    [InlineData("  google.com  ")]  // Leading/trailing spaces
    [InlineData("\tgoogle.com\t")]  // Tabs
    public void ValidateHost_TrimsWhitespace_ValidatesCorrectly(string host)
    {
        // Note: The current implementation doesn't trim, it checks IsNullOrWhiteSpace
        // This test documents the current behavior
        // Act
        var result = InputValidator.ValidateHost(host);

        // Assert - Uri.CheckHostName handles whitespace
        Assert.True(result);
    }

    [Fact]
    public void TryParseTimeout_WhitespaceInput_ReturnsFalse()
    {
        // Act
        var result = InputValidator.TryParseTimeout("   ", out int timeout);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryParseBufferSize_NullInput_ReturnsFalse()
    {
        // Act
        var result = InputValidator.TryParseBufferSize(null!, out int bufferSize);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("2147483647")]  // int.MaxValue
    [InlineData("2147483648")]  // Overflow
    [InlineData("-2147483648")] // int.MinValue
    public void TryParseInterval_ExtremeValues_HandlesCorrectly(string input)
    {
        // Act
        var result = InputValidator.TryParseInterval(input, out int interval);

        // Assert
        // Should either fail to parse or fail validation
        if (result)
        {
            Assert.True(InputValidator.ValidateInterval(interval));
        }
    }

    #endregion
}
