// WinPing Modern - Network Ping Utility
// Copyright (c) 2025 CosmicBytez
// Licensed under MIT License

namespace WinPing;

/// <summary>
/// Provides validation methods for ping parameters.
/// Extracted to enable unit testing of validation logic.
/// </summary>
public static class InputValidator
{
    /// <summary>
    /// Minimum allowed timeout in milliseconds.
    /// </summary>
    public const int MinTimeout = 100;

    /// <summary>
    /// Maximum allowed timeout in milliseconds.
    /// </summary>
    public const int MaxTimeout = 30000;

    /// <summary>
    /// Minimum allowed buffer size in bytes.
    /// </summary>
    public const int MinBufferSize = 1;

    /// <summary>
    /// Maximum allowed buffer size in bytes.
    /// </summary>
    public const int MaxBufferSize = 65500;

    /// <summary>
    /// Minimum allowed interval in milliseconds.
    /// </summary>
    public const int MinInterval = 100;

    /// <summary>
    /// Maximum allowed interval in milliseconds.
    /// </summary>
    public const int MaxInterval = 60000;

    /// <summary>
    /// Validates a host name or IP address.
    /// </summary>
    /// <param name="host">The host to validate.</param>
    /// <returns>True if the host is valid, false otherwise.</returns>
    public static bool ValidateHost(string host)
    {
        if (string.IsNullOrWhiteSpace(host))
            return false;

        // Check if it's a valid hostname or IP
        return Uri.CheckHostName(host) != UriHostNameType.Unknown;
    }

    /// <summary>
    /// Validates a timeout value.
    /// </summary>
    /// <param name="timeout">The timeout to validate in milliseconds.</param>
    /// <returns>True if the timeout is within valid range, false otherwise.</returns>
    public static bool ValidateTimeout(int timeout)
    {
        return timeout >= MinTimeout && timeout <= MaxTimeout;
    }

    /// <summary>
    /// Validates a buffer size value.
    /// </summary>
    /// <param name="bufferSize">The buffer size to validate in bytes.</param>
    /// <returns>True if the buffer size is within valid range, false otherwise.</returns>
    public static bool ValidateBufferSize(int bufferSize)
    {
        return bufferSize >= MinBufferSize && bufferSize <= MaxBufferSize;
    }

    /// <summary>
    /// Validates an interval value.
    /// </summary>
    /// <param name="interval">The interval to validate in milliseconds.</param>
    /// <returns>True if the interval is within valid range, false otherwise.</returns>
    public static bool ValidateInterval(int interval)
    {
        return interval >= MinInterval && interval <= MaxInterval;
    }

    /// <summary>
    /// Tries to parse and validate a timeout value from a string.
    /// </summary>
    /// <param name="input">The string to parse.</param>
    /// <param name="timeout">The parsed timeout value if successful.</param>
    /// <returns>True if parsing and validation succeeded, false otherwise.</returns>
    public static bool TryParseTimeout(string input, out int timeout)
    {
        if (int.TryParse(input, out timeout))
        {
            return ValidateTimeout(timeout);
        }
        return false;
    }

    /// <summary>
    /// Tries to parse and validate a buffer size value from a string.
    /// </summary>
    /// <param name="input">The string to parse.</param>
    /// <param name="bufferSize">The parsed buffer size value if successful.</param>
    /// <returns>True if parsing and validation succeeded, false otherwise.</returns>
    public static bool TryParseBufferSize(string input, out int bufferSize)
    {
        if (int.TryParse(input, out bufferSize))
        {
            return ValidateBufferSize(bufferSize);
        }
        return false;
    }

    /// <summary>
    /// Tries to parse and validate an interval value from a string.
    /// </summary>
    /// <param name="input">The string to parse.</param>
    /// <param name="interval">The parsed interval value if successful.</param>
    /// <returns>True if parsing and validation succeeded, false otherwise.</returns>
    public static bool TryParseInterval(string input, out int interval)
    {
        if (int.TryParse(input, out interval))
        {
            return ValidateInterval(interval);
        }
        return false;
    }
}
