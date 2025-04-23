using System.ComponentModel;
using System.Net;
using System.Net.Http.Headers;

namespace IDMClone
{
    public partial class Form1 : Form
    {
        private HttpClient httpClient = new HttpClient();
        private int maxThreads = 4;
        private string fileName = "";

        public Form1()
        {
            InitializeComponent();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text;
            if (string.IsNullOrWhiteSpace(url)) return;

            try
            {
                lblStatus.Text = "Baðlantý kuruluyor...";
                HttpResponseMessage response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                string suggestedFileName = GetFileNameFromUrlOrHeaders(url, response);

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.FileName = suggestedFileName;

                if (saveDialog.ShowDialog() != DialogResult.OK)
                {
                    lblStatus.Text = "Ýndirme iptal edildi.";
                    return;
                }

                fileName = saveDialog.FileName;
                lblFileNameValue.Text = Path.GetFileName(fileName);
                lblFileSizeValue.Text = FormatBytes(response.Content.Headers.ContentLength ?? 0);

                lblStatus.Text = "Ýndiriliyor...";
                var stream = await response.Content.ReadAsStreamAsync();

                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] buffer = new byte[8192];
                    long totalRead = 0;
                    int read;
                    long totalSize = response.Content.Headers.ContentLength ?? 1;

                    while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fileStream.WriteAsync(buffer, 0, read);
                        totalRead += read;

                        int percent = (int)((totalRead * 100) / totalSize);
                        progressBar1.Value = Math.Min(percent, 100);
                        lblStatus.Text = $"Ýndiriliyor... %{percent}";
                    }
                }

                if (File.Exists(fileName))
                {
                    lblStatus.Text = $"Ýndirme tamamlandý: {fileName}";
                }
                else
                {
                    lblStatus.Text = "Dosya oluþturulamadý.";
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Hata: " + ex.Message;
            }
        }

        private async void btnAdvancedDownload_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text;
            if (string.IsNullOrWhiteSpace(url)) return;

            try
            {
                lblStatus.Text = "Baðlantý kuruluyor...";

                // HEAD isteði yerine GET ile Range kullanarak sunucu desteðini kontrol et
                var testRequest = new HttpRequestMessage(HttpMethod.Get, url);
                testRequest.Headers.Range = new RangeHeaderValue(0, 0);
                var testResponse = await httpClient.SendAsync(testRequest);

                // Range desteði kontrolü
                if (testResponse.StatusCode != HttpStatusCode.PartialContent)
                {
                    lblStatus.Text = "Sunucu parçalý indirmeyi desteklemiyor!";
                    return;
                }

                long totalSize = testResponse.Content.Headers.ContentRange?.Length ?? 0;
                if (totalSize <= 0)
                {
                    lblStatus.Text = "Geçersiz dosya boyutu!";
                    return;
                }

                // Dosya adýný belirle
                string suggestedFileName = GetFileNameFromUrlOrHeaders(url, testResponse);
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.FileName = suggestedFileName;

                if (saveDialog.ShowDialog() != DialogResult.OK)
                {
                    lblStatus.Text = "Ýndirme iptal edildi.";
                    return;
                }
                fileName = saveDialog.FileName;

                lblStatus.Text = "Parçalý indirme baþlatýlýyor...";
                string tempFile = fileName + ".tmp";

                // Geçici dosya
                using (File.Create(tempFile)) { }

                // Progress bar'larý 
                panelProgressBars.Controls.Clear();
                ProgressBar[] partBars = new ProgressBar[maxThreads];
                for (int i = 0; i < maxThreads; i++)
                {
                    partBars[i] = new ProgressBar
                    {
                        Location = new Point(0, i * 30),
                        Size = new Size(700, 25),
                        Minimum = 0,
                        Maximum = 100
                    };
                    panelProgressBars.Controls.Add(partBars[i]);
                }

               
                long partSize = totalSize / maxThreads;
                List<Task> tasks = new List<Task>();

                for (int i = 0; i < maxThreads; i++)
                {
                    int partNumber = i;
                    long start = partNumber * partSize;
                    long end = (partNumber == maxThreads - 1) ? totalSize - 1 : start + partSize - 1;

                    tasks.Add(Task.Run(async () =>
                    {
                        using (var client = new HttpClient())
                        {
                            client.Timeout = Timeout.InfiniteTimeSpan;
                            var request = new HttpRequestMessage(HttpMethod.Get, url);
                            request.Headers.Range = new RangeHeaderValue(start, end);

                            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                            response.EnsureSuccessStatusCode();

                            using (var stream = await response.Content.ReadAsStreamAsync())
                            using (var fileStream = new FileStream(tempFile, FileMode.Open, FileAccess.Write, FileShare.Write))
                            {
                                fileStream.Seek(start, SeekOrigin.Begin);
                                byte[] buffer = new byte[81920];
                                int bytesRead;
                                long totalRead = 0;

                                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                                {
                                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                                    totalRead += bytesRead;

                                    // Progress güncelleme
                                    int progress = (int)((totalRead * 100) / (end - start + 1));
                                    Invoke(new Action(() => partBars[partNumber].Value = progress));
                                }
                            }
                        }
                    }));
                }

                await Task.WhenAll(tasks);
                File.Move(tempFile, fileName, true);
                lblStatus.Text = "Ýndirme baþarýyla tamamlandý!";
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Hata: {ex.Message}";
                if (File.Exists(fileName + ".tmp"))
                    File.Delete(fileName + ".tmp");
            }
        }

        private string GetFileNameFromUrlOrHeaders(string url, HttpResponseMessage response)
        {
            var contentDisposition = response.Content.Headers.ContentDisposition;
            if (contentDisposition != null && !string.IsNullOrEmpty(contentDisposition.FileName))
                return contentDisposition.FileName.Trim('"');

            return Path.GetFileName(new Uri(url).LocalPath);
        }

        private string FormatBytes(long bytes)
        {
            if (bytes >= 1_000_000_000) return (bytes / 1_000_000_000.0).ToString("0.00") + " GB";
            if (bytes >= 1_000_000) return (bytes / 1_000_000.0).ToString("0.00") + " MB";
            if (bytes >= 1_000) return (bytes / 1_000.0).ToString("0.00") + " KB";
            return bytes + " B";
        }

        private string GetFileExtensionFromContentType(string contentType)
        {
            if (string.IsNullOrEmpty(contentType)) return ".bin"; 

            switch (contentType.ToLower())
            {
                case "application/pdf":
                    return ".pdf";
                case "image/jpeg":
                    return ".jpg";
                case "image/png":
                    return ".png";
                case "text/plain":
                    return ".txt";
                case "application/zip":
                    return ".zip";
                case "application/octet-stream":
                    return ".bin";
                default:
                    return ".bin"; 
            }
        }

        private void SaveFile(string filePath)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.FileName = fileName; 
            saveDialog.Filter = "All Files|*.*";
            saveDialog.DefaultExt = Path.GetExtension(fileName);
            saveDialog.AddExtension = true;

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                File.Copy(filePath, saveDialog.FileName, true);
            }
        }

    }
}
