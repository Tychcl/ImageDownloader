using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ImageDownloader.Classes;

namespace ImageDownloader.Elements
{
    /// <summary>
    /// Логика взаимодействия для UserDownloader.xaml
    /// </summary>
    public partial class UserDownloader : UserControl
    {
        private CancellationTokenSource Token;
        private int Id;
        public event Action<int, double> UpdProgress;

        public UserDownloader(int id)
        {
            InitializeComponent();
            Id = id;
        }

        public async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.IsEnabled = false;
            CancelButton.IsEnabled = true;
            //ProgressBar.Value = 0;
            ResultText.Text = "Загрузка...";

            Token = new CancellationTokenSource();

            try
            {
                img.Source = await Start(SetUrl.Text, Token.Token);
                ResultText.Text = $"Загрузка завершена";
            }
            catch (OperationCanceledException)
            {
                ResultText.Text = "Загрузка отменена";
            }
            catch (Exception ex)
            {
                ResultText.Text = ex.Message;
            }
            finally
            {
                StartButton.IsEnabled = true;
                CancelButton.IsEnabled = false;
            }
        }

        private async Task<BitmapImage> Start(string Url, CancellationToken token)
        {
            Download download = new Download(Url, token, 5012);
            download.DownloadProgress += async (progress) =>
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    //ProgressBar.Value = progress;
                    UpdProgress?.Invoke(Id, progress);
                });
            };
            await download.Start();
            return ImageFromBytes.Byte2Image(download.ImageData.ToArray());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.IsEnabled = true;
            CancelButton.IsEnabled = false;
            Token?.Cancel();
        }
    }
}
