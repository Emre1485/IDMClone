namespace IDMClone;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        txtUrl = new TextBox();
        btnDownload = new Button();
        btnAdvancedDownload = new Button();
        lblFileNameTitle = new Label();
        lblFileNameValue = new Label();
        lblFileSizeTitle = new Label();
        lblFileSizeValue = new Label();
        lblDownloadStatusTitle = new Label();
        lblStatus = new Label();
        progressBar1 = new ProgressBar();
        panelProgressBars = new Panel();
        SuspendLayout();
        // 
        // txtUrl
        // 
        txtUrl.Location = new Point(20, 20);
        txtUrl.Name = "txtUrl";
        txtUrl.PlaceholderText = "Dosya URL'si girin...";
        txtUrl.Size = new Size(400, 27);
        txtUrl.TabIndex = 0;
        // 
        // btnDownload
        // 
        btnDownload.Location = new Point(430, 20);
        btnDownload.Name = "btnDownload";
        btnDownload.Size = new Size(120, 27);
        btnDownload.TabIndex = 1;
        btnDownload.Text = "Download";
        btnDownload.Click += btnDownload_Click;
        // 
        // btnAdvancedDownload
        // 
        btnAdvancedDownload.Location = new Point(560, 20);
        btnAdvancedDownload.Name = "btnAdvancedDownload";
        btnAdvancedDownload.Size = new Size(160, 27);
        btnAdvancedDownload.TabIndex = 2;
        btnAdvancedDownload.Text = "Advanced Download";
        btnAdvancedDownload.Click += btnAdvancedDownload_Click;
        // 
        // lblFileNameTitle
        // 
        lblFileNameTitle.Location = new Point(20, 60);
        lblFileNameTitle.Name = "lblFileNameTitle";
        lblFileNameTitle.Size = new Size(100, 23);
        lblFileNameTitle.TabIndex = 3;
        lblFileNameTitle.Text = "Dosya Adı:";
        // 
        // lblFileNameValue
        // 
        lblFileNameValue.Location = new Point(120, 60);
        lblFileNameValue.Name = "lblFileNameValue";
        lblFileNameValue.Size = new Size(600, 20);
        lblFileNameValue.TabIndex = 4;
        // 
        // lblFileSizeTitle
        // 
        lblFileSizeTitle.Location = new Point(20, 90);
        lblFileSizeTitle.Name = "lblFileSizeTitle";
        lblFileSizeTitle.Size = new Size(100, 23);
        lblFileSizeTitle.TabIndex = 5;
        lblFileSizeTitle.Text = "Dosya Boyutu:";
        // 
        // lblFileSizeValue
        // 
        lblFileSizeValue.Location = new Point(120, 90);
        lblFileSizeValue.Name = "lblFileSizeValue";
        lblFileSizeValue.Size = new Size(600, 20);
        lblFileSizeValue.TabIndex = 6;
        // 
        // lblDownloadStatusTitle
        // 
        lblDownloadStatusTitle.Location = new Point(20, 120);
        lblDownloadStatusTitle.Name = "lblDownloadStatusTitle";
        lblDownloadStatusTitle.Size = new Size(100, 23);
        lblDownloadStatusTitle.TabIndex = 7;
        lblDownloadStatusTitle.Text = "Durum:";
        // 
        // lblStatus
        // 
        lblStatus.Location = new Point(120, 120);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(600, 20);
        lblStatus.TabIndex = 8;
        // 
        // progressBar1
        // 
        progressBar1.Location = new Point(20, 160);
        progressBar1.Name = "progressBar1";
        progressBar1.Size = new Size(700, 25);
        progressBar1.TabIndex = 9;
        // 
        // panelProgressBars
        // 
        panelProgressBars.AutoScroll = true;
        panelProgressBars.Location = new Point(20, 200);
        panelProgressBars.Name = "panelProgressBars";
        panelProgressBars.Size = new Size(700, 152);
        panelProgressBars.TabIndex = 10;
        // 
        // Form1
        // 
        AutoScaleMode = AutoScaleMode.None;
        ClientSize = new Size(750, 372);
        Controls.Add(txtUrl);
        Controls.Add(btnDownload);
        Controls.Add(btnAdvancedDownload);
        Controls.Add(lblFileNameTitle);
        Controls.Add(lblFileNameValue);
        Controls.Add(lblFileSizeTitle);
        Controls.Add(lblFileSizeValue);
        Controls.Add(lblDownloadStatusTitle);
        Controls.Add(lblStatus);
        Controls.Add(progressBar1);
        Controls.Add(panelProgressBars);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "IDM Clone - Dosya İndirme";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TextBox txtUrl;
    private Button btnDownload;
    private Button btnAdvancedDownload;
    private Label lblFileNameTitle;
    private Label lblFileNameValue;
    private Label lblFileSizeTitle;
    private Label lblFileSizeValue;
    private Label lblDownloadStatusTitle;
    private Label lblStatus;
    private ProgressBar progressBar1;
    private Panel panelProgressBars;
}
