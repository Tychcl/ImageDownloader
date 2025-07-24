using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using WpfApp1.ViewModel;
using WpfApp1.Views;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int DownloadersCount = 3;
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new VMMainWindow(DownloadersCount);
            DataContext = viewModel;

            foreach (var downloaderVM in viewModel.Downloaders)
            {
                var userDownloader = new UserDownloader();
                userDownloader.DataContext = downloaderVM;
                Parent.Children.Add(userDownloader);
            }
        }
    }
}
