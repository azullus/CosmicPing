# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [2.0.0] - 2025-12-27

### Added
- Comprehensive troubleshooting section (200+ lines, 7 categories)
  - Application won't start
  - Ping failures
  - Export CSV issues
  - High CPU or memory usage
  - Inaccurate latency readings
  - Application crashes
  - Build errors
- Performance characteristics table
- Known limitations documentation
- Contributing guidelines
- Development setup instructions
- Testing guide
- FAQ section (5 questions)
- Related projects cross-linking
- MIT License file

### Changed
- Complete rewrite with proper resource disposal
- Fixed all memory leaks from v1.0.0
- Improved async ping operations
- Enhanced error handling
- Updated documentation structure

### Fixed
- **Memory Leak**: Ping objects now properly disposed using `using` statements
- **Memory Leak**: CancellationTokenSource properly disposed in `finally` block
- **Memory Leak**: Result retention limited to 1000 entries (configurable)
- **Error Handling**: Removed empty catch blocks, all errors properly logged
- **Resource Management**: Form disposal properly handles cleanup

### Technical
- Target framework: .NET 8.0 (net8.0-windows)
- Language: C# 12
- Architecture: WinForms desktop application
- Memory-safe with automatic resource disposal
- Async/await pattern for non-blocking UI
- Input validation with comprehensive error messages

## [1.0.0] - 2025-06

### Added
- Initial release of WinPing Modern
- Continuous ping functionality
- Live statistics (min/max/avg latency)
- Visual latency chart
- CSV export capability
- Configurable timeout, buffer size, and interval
- Real-time packet loss tracking

### Known Issues
- Memory leaks due to improper resource disposal
- CancellationTokenSource not disposed
- Ping objects not properly cleaned up
- Result list grows unbounded
- **Status**: DEPRECATED - upgrade to v2.0.0

---

**Note**: Version 1.0.0 is deprecated due to memory leaks. All users should upgrade to v2.0.0 which includes complete rewrite with proper resource management.

[Unreleased]: https://github.com/azullus/CosmicPing/compare/v2.0.0...HEAD
[2.0.0]: https://github.com/azullus/CosmicPing/compare/v1.0.0...v2.0.0
[1.0.0]: https://github.com/azullus/CosmicPing/releases/tag/v1.0.0
