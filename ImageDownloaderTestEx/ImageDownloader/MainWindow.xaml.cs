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

namespace ImageDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int DownloadersCount = 4;
        List<double> Downloaders = new List<double>();
        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < DownloadersCount; i++)
            {
                Elements.UserDownloader userDownloader = new Elements.UserDownloader(i);
                Downloaders.Add(0);
                userDownloader.UpdProgress += async (id, progress) =>
                {
                    Downloaders[id] = progress;
                    await Dispatcher.InvokeAsync(() =>
                    {
                        ProgressBar.Value = Downloaders.Sum() / DownloadersCount;
                    });
                };
                Parent.Children.Add(userDownloader);
            }
        }
        
    }
}