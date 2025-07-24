using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ImageDownloader.Classes;

namespace WpfApp1.ViewModel
{
    public class VMUserDownloader : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private CancellationTokenSource Token;
        public Models.Downloader Downloader { get; set; }
        public VMUserDownloader()
        {
            Downloader = new Models.Downloader();
            Downloader.StartButtonIsEnabled = true;
            Downloader.CancelButtonIsEnabled = false;
        }

        public static event Action<double> ProgressChanged;
        private double _progress;
        public double Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged();
                ProgressChanged?.Invoke(value);
            }
        }

        private async Task<BitmapImage> Start(string Url, CancellationToken token)
        {
            Download download = new Download(Url, token, 5012);
            download.DownloadProgress += (progress) =>
            {
                Downloader.Done = progress;
                Progress = progress;
            };
            await download.Start();
            return ImageFromBytes.Byte2Image(download.ImageData.ToArray());
        }

        private RelayCommand _start;
        public RelayCommand StartDownload
        { 
            get
            {
                return _start ?? (_start = new RelayCommand(async obj =>
                {
                    Downloader.Done = 0;
                    Downloader.Status = "Загрузка...";
                    Downloader.StartButtonIsEnabled = false;
                    Downloader.CancelButtonIsEnabled = true;

                    Token = new CancellationTokenSource();

                    try
                    {
                        Downloader.Image = await Start(Downloader.Url, Token.Token);
                        Downloader.Status = $"Загрузка завершена";
                    }
                    catch (OperationCanceledException)
                    {
                        Downloader.Done = 0;
                        Progress = 0;
                        Downloader.Status = "Загрузка отменена";
                    }
                    catch (Exception ex)
                    {
                        Downloader.Status = ex.Message;
                    }
                    finally
                    {
                        Downloader.StartButtonIsEnabled = true;
                        Downloader.CancelButtonIsEnabled = false;
                    }
                }));
            } 
        }

        private RelayCommand _cancel;
        public RelayCommand CancelDownload
        {
            get
            {
                return _cancel ?? (_cancel = new RelayCommand(obj =>
                {
                    Downloader.StartButtonIsEnabled = true;
                    Downloader.CancelButtonIsEnabled = false;
                    Token?.Cancel();
                }));
            }
        }
    }
}
