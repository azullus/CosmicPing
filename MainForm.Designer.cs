// WinPing Modern - Network Ping Utility
// Copyright (c) 2025 CosmicBytez
// Licensed under MIT License

#nullable enable

namespace WinPing;

partial class MainForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer? components = null;

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();

        // Create controls
        txtHost = new TextBox();
        txtTimeout = new TextBox();
        txtBufferSize = new TextBox();
        txtInterval = new TextBox();
        btnStart = new Button();
        btnStop = new Button();
        btnClear = new Button();
        btnExport = new Button();
        lstResults = new ListBox();
        txtChart = new TextBox();
        lblStats = new Label();
        lblHost = new Label();
        lblTimeout = new Label();
        lblBufferSize = new Label();
        lblInterval = new Label();
        splitContainer = new SplitContainer();
        panelTop = new Panel();
        panelBottom = new Panel();

        ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
        splitContainer.Panel1.SuspendLayout();
        splitContainer.Panel2.SuspendLayout();
        splitContainer.SuspendLayout();
        panelTop.SuspendLayout();
        panelBottom.SuspendLayout();
        SuspendLayout();

        // lblHost
        lblHost.AutoSize = true;
        lblHost.Location = new Point(12, 15);
        lblHost.Name = "lblHost";
        lblHost.Size = new Size(35, 15);
        lblHost.Text = "Host:";

        // txtHost
        txtHost.Location = new Point(53, 12);
        txtHost.Name = "txtHost";
        txtHost.Size = new Size(200, 23);
        txtHost.Text = "8.8.8.8";
        txtHost.KeyPress += txtHost_KeyPress;

        // lblTimeout
        lblTimeout.AutoSize = true;
        lblTimeout.Location = new Point(270, 15);
        lblTimeout.Name = "lblTimeout";
        lblTimeout.Size = new Size(73, 15);
        lblTimeout.Text = "Timeout (ms):";

        // txtTimeout
        txtTimeout.Location = new Point(349, 12);
        txtTimeout.Name = "txtTimeout";
        txtTimeout.Size = new Size(60, 23);
        txtTimeout.Text = "1000";

        // lblBufferSize
        lblBufferSize.AutoSize = true;
        lblBufferSize.Location = new Point(425, 15);
        lblBufferSize.Name = "lblBufferSize";
        lblBufferSize.Size = new Size(72, 15);
        lblBufferSize.Text = "Buffer (bytes):";

        // txtBufferSize
        txtBufferSize.Location = new Point(503, 12);
        txtBufferSize.Name = "txtBufferSize";
        txtBufferSize.Size = new Size(50, 23);
        txtBufferSize.Text = "32";

        // lblInterval
        lblInterval.AutoSize = true;
        lblInterval.Location = new Point(570, 15);
        lblInterval.Name = "lblInterval";
        lblInterval.Size = new Size(70, 15);
        lblInterval.Text = "Interval (ms):";

        // txtInterval
        txtInterval.Location = new Point(646, 12);
        txtInterval.Name = "txtInterval";
        txtInterval.Size = new Size(60, 23);
        txtInterval.Text = "1000";

        // btnStart
        btnStart.Location = new Point(730, 10);
        btnStart.Name = "btnStart";
        btnStart.Size = new Size(75, 27);
        btnStart.Text = "Start";
        btnStart.UseVisualStyleBackColor = true;
        btnStart.Click += btnStart_Click;

        // btnStop
        btnStop.Location = new Point(811, 10);
        btnStop.Name = "btnStop";
        btnStop.Size = new Size(75, 27);
        btnStop.Text = "Stop";
        btnStop.UseVisualStyleBackColor = true;
        btnStop.Click += btnStop_Click;

        // btnClear
        btnClear.Location = new Point(892, 10);
        btnClear.Name = "btnClear";
        btnClear.Size = new Size(75, 27);
        btnClear.Text = "Clear";
        btnClear.UseVisualStyleBackColor = true;
        btnClear.Click += btnClear_Click;

        // btnExport
        btnExport.Location = new Point(973, 10);
        btnExport.Name = "btnExport";
        btnExport.Size = new Size(75, 27);
        btnExport.Text = "Export CSV";
        btnExport.UseVisualStyleBackColor = true;
        btnExport.Click += btnExport_Click;

        // panelTop
        panelTop.Controls.Add(lblHost);
        panelTop.Controls.Add(txtHost);
        panelTop.Controls.Add(lblTimeout);
        panelTop.Controls.Add(txtTimeout);
        panelTop.Controls.Add(lblBufferSize);
        panelTop.Controls.Add(txtBufferSize);
        panelTop.Controls.Add(lblInterval);
        panelTop.Controls.Add(txtInterval);
        panelTop.Controls.Add(btnStart);
        panelTop.Controls.Add(btnStop);
        panelTop.Controls.Add(btnClear);
        panelTop.Controls.Add(btnExport);
        panelTop.Dock = DockStyle.Top;
        panelTop.Location = new Point(0, 0);
        panelTop.Name = "panelTop";
        panelTop.Size = new Size(1064, 48);

        // lstResults
        lstResults.Dock = DockStyle.Fill;
        lstResults.Font = new Font("Consolas", 9F);
        lstResults.FormattingEnabled = true;
        lstResults.HorizontalScrollbar = true;
        lstResults.IntegralHeight = false;
        lstResults.ItemHeight = 14;
        lstResults.Location = new Point(0, 0);
        lstResults.Name = "lstResults";
        lstResults.Size = new Size(1064, 300);

        // txtChart
        txtChart.Dock = DockStyle.Fill;
        txtChart.Font = new Font("Consolas", 8F);
        txtChart.Location = new Point(0, 0);
        txtChart.Multiline = true;
        txtChart.Name = "txtChart";
        txtChart.ReadOnly = true;
        txtChart.ScrollBars = ScrollBars.Both;
        txtChart.Size = new Size(1064, 150);
        txtChart.WordWrap = false;
        txtChart.BackColor = System.Drawing.Color.Black;
        txtChart.ForeColor = System.Drawing.Color.LimeGreen;

        // splitContainer
        splitContainer.Dock = DockStyle.Fill;
        splitContainer.Location = new Point(0, 48);
        splitContainer.Name = "splitContainer";
        splitContainer.Orientation = Orientation.Horizontal;
        splitContainer.Panel1.Controls.Add(lstResults);
        splitContainer.Panel2.Controls.Add(txtChart);
        splitContainer.Size = new Size(1064, 453);
        splitContainer.SplitterDistance = 300;
        splitContainer.SplitterWidth = 3;

        // lblStats
        lblStats.Dock = DockStyle.Fill;
        lblStats.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblStats.Location = new Point(0, 0);
        lblStats.Name = "lblStats";
        lblStats.Padding = new Padding(5, 0, 0, 0);
        lblStats.Size = new Size(1064, 30);
        lblStats.Text = "Packets: 0 sent, 0 received, 0% loss | Min: -ms, Max: -ms, Avg: -ms";
        lblStats.TextAlign = ContentAlignment.MiddleLeft;
        lblStats.BackColor = System.Drawing.SystemColors.ControlLight;

        // panelBottom
        panelBottom.Controls.Add(lblStats);
        panelBottom.Dock = DockStyle.Bottom;
        panelBottom.Location = new Point(0, 501);
        panelBottom.Name = "panelBottom";
        panelBottom.Size = new Size(1064, 30);

        // MainForm
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1064, 531);
        Controls.Add(splitContainer);
        Controls.Add(panelTop);
        Controls.Add(panelBottom);
        MinimumSize = new Size(800, 400);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "WinPing Modern v2.1 - CosmicBytez";

        ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
        splitContainer.Panel1.ResumeLayout(false);
        splitContainer.Panel2.ResumeLayout(false);
        splitContainer.Panel2.PerformLayout();
        splitContainer.ResumeLayout(false);
        panelTop.ResumeLayout(false);
        panelTop.PerformLayout();
        panelBottom.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private TextBox txtHost = null!;
    private TextBox txtTimeout = null!;
    private TextBox txtBufferSize = null!;
    private TextBox txtInterval = null!;
    private Button btnStart = null!;
    private Button btnStop = null!;
    private Button btnClear = null!;
    private Button btnExport = null!;
    private ListBox lstResults = null!;
    private TextBox txtChart = null!;
    private Label lblStats = null!;
    private Label lblHost = null!;
    private Label lblTimeout = null!;
    private Label lblBufferSize = null!;
    private Label lblInterval = null!;
    private SplitContainer splitContainer = null!;
    private Panel panelTop = null!;
    private Panel panelBottom = null!;
}
