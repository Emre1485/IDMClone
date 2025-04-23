namespace IDMClone
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtUrl = new TextBox();
            btnDownload = new Button();
            lblStatus = new Label();
            progressBar1 = new ProgressBar();
            btnAdvancedDownload = new Button();
            panelProgressBars = new Panel();
            SuspendLayout();
            // 
            // txtUrl
            // 
            txtUrl.Location = new Point(31, 38);
            txtUrl.Name = "txtUrl";
            txtUrl.Size = new Size(317, 27);
            txtUrl.TabIndex = 0;
            // 
            // btnDownload
            // 
            btnDownload.Location = new Point(354, 36);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(157, 29);
            btnDownload.TabIndex = 1;
            btnDownload.Text = "Download";
            btnDownload.UseVisualStyleBackColor = true;
            btnDownload.Click += btnDownload_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(31, 123);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(49, 20);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "Status";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(31, 71);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(317, 29);
            progressBar1.TabIndex = 3;
            // 
            // btnAdvancedDownload
            // 
            btnAdvancedDownload.Location = new Point(354, 71);
            btnAdvancedDownload.Name = "btnAdvancedDownload";
            btnAdvancedDownload.Size = new Size(157, 29);
            btnAdvancedDownload.TabIndex = 4;
            btnAdvancedDownload.Text = "AdvancedDownload";
            btnAdvancedDownload.UseVisualStyleBackColor = true;
            btnAdvancedDownload.Click += btnAdvancedDownload_ClickAsync;
            // 
            // panelProgressBars
            // 
            panelProgressBars.Location = new Point(31, 163);
            panelProgressBars.Name = "panelProgressBars";
            panelProgressBars.Size = new Size(480, 382);
            panelProgressBars.TabIndex = 5;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(552, 559);
            Controls.Add(panelProgressBars);
            Controls.Add(btnAdvancedDownload);
            Controls.Add(progressBar1);
            Controls.Add(lblStatus);
            Controls.Add(btnDownload);
            Controls.Add(txtUrl);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtUrl;
        private Button btnDownload;
        private Label lblStatus;
        private ProgressBar progressBar1;
        private Button btnAdvancedDownload;
        private Panel panelProgressBars;
    }
}
