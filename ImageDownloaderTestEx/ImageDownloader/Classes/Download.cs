using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ImageDownloader.Classes
{
    public class Download
    {
        private string Url;
        private int ChunkSize;
        private long TotalBytes;
        private bool CanDownload = false;
        private int DownloadedBytes = 0;

        public event Action<double> DownloadProgress; 
        public double Done { get; private set; } = 0;
        public bool ItsImage { get; private set; }
        public List<byte> ImageData { get; private set; }
        public CancellationToken Token { private get; set; }

        public Download(string url, CancellationToken token, int chunkSize)
        {
            Url = url;
            Token = token;
            ImageData = new();
            ChunkSize = chunkSize;
        }

        private async Task StartDownload()
        {
            try
            {
                var getdata = (HttpWebRequest)WebRequest.Create(Url);
                getdata.Method = "HEAD";
                using (var response = await getdata.GetResponseAsync())
                {
                    ItsImage = response.ContentType.Contains("image");
                    TotalBytes = response.ContentLength;
                }
            }
            catch
            {
                throw new Exception("Что то не так с ссылкой\nИли нет интернета");
            }

            if (!ItsImage)
                throw new Exception("Это не картинка");

            var request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.AddRange(DownloadedBytes);

            using (var response = await request.GetResponseAsync())
            {
                using (var stream = response.GetResponseStream())
                {
                    while (CanDownload)
                    {
                        Token.ThrowIfCancellationRequested();
                        byte[] bytes = new byte[ChunkSize];
                        int readed = await stream.ReadAsync(bytes, 0, bytes.Length);
                        if (readed == 0)
                        {
                            break;
                        }
                        DownloadedBytes += readed;
                        Done = Math.Round((double)DownloadedBytes / TotalBytes, 2);
                        DownloadProgress?.Invoke(Done);
                        Array.Resize(ref bytes, readed);
                        ImageData.AddRange(bytes);
                    }
                }
            }
        }

        public Task Start()
        {
            CanDownload = true;
            return StartDownload();
        }

        public void Cancel()
        {
            CanDownload = false;
        }
    }
}
