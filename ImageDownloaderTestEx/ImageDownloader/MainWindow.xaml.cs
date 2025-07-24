using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
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
using ImageDownloader.Elements;

namespace ImageDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int DownloadersCount = 4;
        List<double> DownloadStatus = new List<double>();
        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < DownloadersCount; i++)
            {
                Elements.UserDownloader userDownloader = new Elements.UserDownloader(i);
                DownloadStatus.Add(0);
                userDownloader.UpdProgress += async (id, progress) =>
                {
                    DownloadStatus[id] = progress;
                    await Dispatcher.InvokeAsync(() =>
                    {
                        ProgressBar.Value = DownloadStatus.Sum() / DownloadersCount;
                    });
                };
                Parent.Children.Add(userDownloader);
            }
        }

        private void DownloadAll(object sender, RoutedEventArgs e)
        {
            foreach (UserDownloader item in Parent.Children)
            {
                item.StartButton_Click(null, null);
            }
        }
    }
}