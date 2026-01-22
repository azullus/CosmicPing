// WinPing Modern - Network Ping Utility
// Copyright (c) 2025 CosmicBytez
// Licensed under MIT License

using System.Net.NetworkInformation;
using System.Text;

namespace WinPing;

/// <summary>
/// Main form for the WinPing Modern application.
/// Provides network ping functionality with live results and CSV export.
/// </summary>
public partial class MainForm : Form, IDisposable
{
    // Maximum results to retain (prevents memory leak)
    private const int MaxResultsRetention = 1000;

    private readonly List<PingResult> _results = new();
    private readonly object _resultsLock = new();
    private CancellationTokenSource? _cts;
    private bool _isRunning;
    private int _sequenceNumber;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the MainForm class.
    /// </summary>
    public MainForm()
    {
        InitializeComponent();
        UpdateButtonStates();
    }


    /// <summary>
    /// Starts the ping operation.
    /// </summary>
    private async void StartPing()
    {
        var host = txtHost.Text.Trim();

        if (!InputValidator.ValidateHost(host))
        {
            MessageBox.Show("Please enter a valid hostname or IP address.",
                "Invalid Host", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!InputValidator.TryParseTimeout(txtTimeout.Text, out int timeout))
        {
            MessageBox.Show($"Timeout must be between {InputValidator.MinTimeout} and {InputValidator.MaxTimeout} milliseconds.",
                "Invalid Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!InputValidator.TryParseBufferSize(txtBufferSize.Text, out int bufferSize))
        {
            MessageBox.Show($"Buffer size must be between {InputValidator.MinBufferSize} and {InputValidator.MaxBufferSize} bytes.",
                "Invalid Buffer Size", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!InputValidator.TryParseInterval(txtInterval.Text, out int interval))
        {
            MessageBox.Show($"Interval must be between {InputValidator.MinInterval} and {InputValidator.MaxInterval} milliseconds.",
                "Invalid Interval", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Reset state
        _sequenceNumber = 0;
        lock (_resultsLock)
        {
            _results.Clear();
        }
        lstResults.Items.Clear();
        UpdateStatistics();

        _isRunning = true;
        UpdateButtonStates();

        // Create new CancellationTokenSource (properly disposed later)
        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        try
        {
            await PingLoopAsync(host, timeout, bufferSize, interval, token);
        }
        catch (OperationCanceledException)
        {
            // Expected when stopped by user
            AppendLog("Ping stopped by user.");
        }
        catch (Exception ex)
        {
            AppendLog($"Error: {ex.Message}");
            MessageBox.Show($"Ping failed: {ex.Message}",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _isRunning = false;
            UpdateButtonStates();

            // Properly dispose CancellationTokenSource
            _cts?.Dispose();
            _cts = null;
        }
    }

    /// <summary>
    /// Main ping loop that executes asynchronously.
    /// </summary>
    private async Task PingLoopAsync(string host, int timeout, int bufferSize, int interval, CancellationToken token)
    {
        var buffer = new byte[bufferSize];
        Array.Fill(buffer, (byte)'X');

        var options = new PingOptions
        {
            DontFragment = true,
            Ttl = 128
        };

        AppendLog($"Pinging {host} with {bufferSize} bytes of data:");

        while (!token.IsCancellationRequested)
        {
            _sequenceNumber++;

            // Use 'using' statement to ensure Ping is properly disposed
            using (var ping = new Ping())
            {
                try
                {
                    var reply = await ping.SendPingAsync(host, timeout, buffer, options);

                    var result = new PingResult
                    {
                        Sequence = _sequenceNumber,
                        Timestamp = DateTime.Now,
                        Host = host,
                        IpAddress = reply.Address?.ToString(),
                        RoundtripTime = reply.Status == IPStatus.Success ? reply.RoundtripTime : -1,
                        Status = reply.Status,
                        Ttl = reply.Options?.Ttl ?? 0,
                        BufferSize = bufferSize
                    };

                    AddResult(result);
                }
                catch (PingException ex)
                {
                    // Handle ping-specific errors properly (not empty catch)
                    var result = new PingResult
                    {
                        Sequence = _sequenceNumber,
                        Timestamp = DateTime.Now,
                        Host = host,
                        RoundtripTime = -1,
                        Status = IPStatus.Unknown,
                        BufferSize = bufferSize
                    };

                    AddResult(result);
                    AppendLog($"Ping error: {ex.Message}");
                }
            } // Ping is disposed here

            // Wait for interval before next ping
            await Task.Delay(interval, token);
        }
    }

    /// <summary>
    /// Adds a ping result and updates the UI.
    /// </summary>
    private void AddResult(PingResult result)
    {
        lock (_resultsLock)
        {
            _results.Add(result);

            // Enforce retention limit to prevent memory leak
            while (_results.Count > MaxResultsRetention)
            {
                _results.RemoveAt(0);
            }
        }

        // Update UI on UI thread
        if (InvokeRequired)
        {
            Invoke(() => UpdateUI(result));
        }
        else
        {
            UpdateUI(result);
        }
    }

    /// <summary>
    /// Updates the UI with a new result.
    /// </summary>
    private void UpdateUI(PingResult result)
    {
        // Add to listbox
        lstResults.Items.Add(result.ToString());

        // Auto-scroll to bottom
        lstResults.TopIndex = lstResults.Items.Count - 1;

        // Enforce UI list limit
        while (lstResults.Items.Count > MaxResultsRetention)
        {
            lstResults.Items.RemoveAt(0);
        }

        UpdateStatistics();
        UpdateChart(result);
    }

    /// <summary>
    /// Updates the statistics labels.
    /// </summary>
    private void UpdateStatistics()
    {
        List<PingResult> snapshot;
        lock (_resultsLock)
        {
            snapshot = _results.ToList();
        }

        if (snapshot.Count == 0)
        {
            lblStats.Text = "Packets: 0 sent, 0 received, 0% loss | Min: -ms, Max: -ms, Avg: -ms";
            return;
        }

        var sent = snapshot.Count;
        var received = snapshot.Count(r => r.IsSuccess);
        var loss = (double)(sent - received) / sent * 100;

        var successfulResults = snapshot.Where(r => r.IsSuccess).ToList();
        var min = successfulResults.Any() ? successfulResults.Min(r => r.RoundtripTime) : 0;
        var max = successfulResults.Any() ? successfulResults.Max(r => r.RoundtripTime) : 0;
        var avg = successfulResults.Any() ? successfulResults.Average(r => r.RoundtripTime) : 0;

        lblStats.Text = $"Packets: {sent} sent, {received} received, {loss:F1}% loss | " +
                       $"Min: {min}ms, Max: {max}ms, Avg: {avg:F1}ms";
    }

    /// <summary>
    /// Updates the latency chart with a new data point.
    /// </summary>
    private void UpdateChart(PingResult result)
    {
        // Simple text-based chart representation
        // In a full implementation, this could use a charting library
        var latency = result.IsSuccess ? result.RoundtripTime : 0;
        var barLength = Math.Min((int)(latency / 5), 50); // Scale: 5ms per character, max 50 chars
        var bar = new string('|', barLength);
        var color = result.IsSuccess ? (latency < 50 ? "green" : latency < 150 ? "yellow" : "red") : "timeout";

        // Update chart area (simplified - real implementation would draw graphics)
        txtChart.AppendText($"{result.Sequence:D4}: {bar} {latency}ms\r\n");

        // Limit chart history
        var lines = txtChart.Lines;
        if (lines.Length > MaxResultsRetention)
        {
            txtChart.Lines = lines.Skip(lines.Length - MaxResultsRetention).ToArray();
        }
    }

    /// <summary>
    /// Stops the ping operation.
    /// </summary>
    private void StopPing()
    {
        _cts?.Cancel();
    }

    /// <summary>
    /// Clears all results.
    /// </summary>
    private void ClearResults()
    {
        lock (_resultsLock)
        {
            _results.Clear();
        }
        lstResults.Items.Clear();
        txtChart.Clear();
        _sequenceNumber = 0;
        UpdateStatistics();
    }

    /// <summary>
    /// Exports results to a CSV file.
    /// </summary>
    private void ExportToCsv()
    {
        List<PingResult> snapshot;
        lock (_resultsLock)
        {
            snapshot = _results.ToList();
        }

        if (snapshot.Count == 0)
        {
            MessageBox.Show("No results to export.",
                "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dialog = new SaveFileDialog
        {
            Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
            FileName = $"ping_results_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
            DefaultExt = "csv"
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine(PingResult.CsvHeader);
                foreach (var result in snapshot)
                {
                    sb.AppendLine(result.ToCsvLine());
                }
                File.WriteAllText(dialog.FileName, sb.ToString());

                AppendLog($"Exported {snapshot.Count} results to {dialog.FileName}");
                MessageBox.Show($"Exported {snapshot.Count} results successfully.",
                    "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to export: {ex.Message}",
                    "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    /// <summary>
    /// Appends a message to the log.
    /// </summary>
    private void AppendLog(string message)
    {
        if (InvokeRequired)
        {
            Invoke(() => AppendLog(message));
            return;
        }

        lstResults.Items.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
        lstResults.TopIndex = lstResults.Items.Count - 1;
    }

    /// <summary>
    /// Updates the enabled state of buttons.
    /// </summary>
    private void UpdateButtonStates()
    {
        btnStart.Enabled = !_isRunning;
        btnStop.Enabled = _isRunning;
        btnExport.Enabled = !_isRunning;
        txtHost.Enabled = !_isRunning;
        txtTimeout.Enabled = !_isRunning;
        txtBufferSize.Enabled = !_isRunning;
        txtInterval.Enabled = !_isRunning;
    }

    #region Event Handlers

    private void btnStart_Click(object? sender, EventArgs e) => StartPing();
    private void btnStop_Click(object? sender, EventArgs e) => StopPing();
    private void btnClear_Click(object? sender, EventArgs e) => ClearResults();
    private void btnExport_Click(object? sender, EventArgs e) => ExportToCsv();

    private void txtHost_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter && !_isRunning)
        {
            e.Handled = true;
            StartPing();
        }
    }

    #endregion

    #region IDisposable Implementation

    /// <summary>
    /// Releases all resources used by the form.
    /// </summary>
    /// <param name="disposing">True if disposing managed resources.</param>
    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed resources
                _cts?.Cancel();
                _cts?.Dispose();
                _cts = null;

                lock (_resultsLock)
                {
                    _results.Clear();
                }

                components?.Dispose();
            }

            _disposed = true;
        }
        base.Dispose(disposing);
    }

    #endregion
}
