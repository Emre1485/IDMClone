using System.ComponentModel;
using System.Net;

namespace IDMClone
{
    public partial class Form1 : Form
    {
        private WebClient webClient;
        private readonly HttpClient httpClient = new HttpClient();

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
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
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

        private async void btnAdvancedDownload_ClickAsync(object sender, EventArgs e)
        {
            string url = txtUrl.Text.Trim();
            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("Lütfen bir URL girin.");
                return;
            }

            try
            {

                string outputDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);  // Klasör yoksa oluþtur
                }

                string fileName = Path.GetFileName(url);
                string validFileName = string.Concat(fileName.Split(Path.GetInvalidFileNameChars()));
                string outputPath = Path.Combine(outputDirectory, validFileName);

                long totalSize = await GetFileSizeAsync(url);
                lblStatus.Text = $"Total size: {totalSize} bytes";

                using (var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    fs.SetLength(totalSize);
                }


                // 2. 4 parçaya böl
                int numParts = 4;
                long partSize = totalSize / numParts;
                List<Task> tasks = new List<Task>();

                for (int i = 0; i < numParts; i++)
                {
                    long start = i * partSize;
                    long end = (i == numParts - 1) ? totalSize - 1 : (start + partSize - 1);

                    tasks.Add(DownloadPart(url, outputPath, start, end));
                }

                // 3. Tüm parçalar indirilsin
                await Task.WhenAll(tasks);

                lblStatus.Text = "Download comlpeted!";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
            }
        }

        private async Task<long> GetFileSizeAsync(string url)
        {
            using var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));

            if (!response.IsSuccessStatusCode)
                throw new Exception("HEAD request failed.");

            // Bazý sunucular Accept-Ranges göndermez, ama yine de Range destekler
            var acceptRanges = response.Headers.AcceptRanges;
            if (acceptRanges == null || !acceptRanges.Contains("bytes"))
            {
                throw new Exception("The server does not support partial downloads.");
            }

            return response.Content.Headers.ContentLength ?? throw new Exception("Could not retrive file size.");
        }


        public async Task DownloadPart(string url, string outputPath, long start, long end)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(start, end);

            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode(); 

            using var stream = await response.Content.ReadAsStreamAsync();
            using var outputStream = new FileStream(outputPath, FileMode.Open, FileAccess.Write, FileShare.Write);

            outputStream.Seek(start, SeekOrigin.Begin);
            await stream.CopyToAsync(outputStream);
        }


    }
}
