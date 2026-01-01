# WinPing.Tests

Unit tests for the WinPing Modern network ping utility.

## Overview

This test project uses **xUnit** to test the core logic of WinPing without testing the Windows Forms UI directly.

## Test Coverage

### PingResultTests.cs
Tests for the `PingResult` class:
- **ToString Formatting**: Verifies correct formatting of successful and failed ping results
- **CSV Generation**: Tests `ToCsvLine()` method and CSV header
- **Property Initialization**: Validates property setters and getters
- **Edge Cases**: Handles null IP addresses, zero roundtrip times, IPv6 addresses

### ValidationTests.cs
Tests for the `InputValidator` class:
- **Host Validation**: Tests valid/invalid hostnames, IP addresses (IPv4/IPv6), edge cases
- **Timeout Validation**: Range validation (100-30000ms), boundary tests, parse methods
- **Buffer Size Validation**: Range validation (1-65500 bytes), boundary tests, parse methods
- **Interval Validation**: Range validation (100-60000ms), boundary tests, parse methods

## Running Tests

### Using .NET CLI
```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --verbosity normal

# Run specific test file
dotnet test --filter "FullyQualifiedName~PingResultTests"

# Run with code coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Using Visual Studio
1. Open `WinPing.sln` in Visual Studio
2. Build the solution (Ctrl+Shift+B)
3. Open Test Explorer (Test > Test Explorer)
4. Click "Run All Tests"

### Using Rider
1. Open `WinPing.sln` in JetBrains Rider
2. Right-click on the `WinPing.Tests` project
3. Select "Run Unit Tests"

## Test Structure

```
WinPing.Tests/
├── WinPing.Tests.csproj    # Project file with xUnit dependencies
├── PingResultTests.cs      # Tests for PingResult class
├── ValidationTests.cs      # Tests for InputValidator class
└── README.md              # This file
```

## Dependencies

- **xUnit** 2.6.2 - Testing framework
- **Microsoft.NET.Test.Sdk** 17.8.0 - Test platform
- **coverlet.collector** 6.0.0 - Code coverage collector

## Test Approach

The tests focus on:
1. **Business Logic**: Testing pure functions and data classes
2. **Validation Rules**: Ensuring input constraints are enforced
3. **Edge Cases**: Boundary values, null inputs, extreme values
4. **Data Formatting**: CSV export and display formatting

**Note**: Windows Forms UI components are not directly tested. UI testing would require additional frameworks like FlaUI or WinAppDriver.

## Test Statistics

- **Total Test Methods**: 60+
- **Test Coverage Areas**:
  - PingResult class (100%)
  - InputValidator class (100%)
  - MainForm validation logic (indirectly via InputValidator)

## Future Enhancements

Potential additions for expanded test coverage:
- Integration tests for actual network pinging
- Performance tests for high-volume results
- UI automation tests (if needed)
- Mock-based tests for edge cases
