using System.ComponentModel;
using System.Net;

namespace IDMClone
{
    public partial class Form1 : Form
    {
        private WebClient webClient;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text.Trim();
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Lütfen dosya url adresini giriniz!");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = System.IO.Path.GetFileName(url);
            if (saveFileDialog.ShowDialog()== DialogResult.OK)
            {
                webClient = new WebClient();
                webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;

                try
                {
                    Uri uri = new Uri(url);
                    webClient.DownloadFileAsync(uri, saveFileDialog.FileName);
                    lblStatus.Text = "Download started...";
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Error: " + ex.Message;
                }
            }
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            lblStatus.Text = $"Downloading...%{e.ProgressPercentage}";
        }

        private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
                lblStatus.Text = "Download cancelled.";
            else if (e.Error != null)
                lblStatus.Text = "An error ocurred: " + e.Error.Message;
            else
                lblStatus.Text = "Download completed!";
        }
    }
}
