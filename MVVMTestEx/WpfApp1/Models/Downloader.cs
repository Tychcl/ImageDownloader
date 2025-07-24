using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfApp1.Models
{
    public class Downloader : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _url;
        private BitmapImage _image;
        private double _done;
        private string _status;
        private bool _StartButtonIsEnabled;
        private bool _CancelButtonIsEnabled;

        public Downloader() { }
        public string Url
        {
            get { return _url; }
            set { _url = value; OnPropertyChanged(nameof(Url)); }
        }

        public BitmapImage Image
        {
            get { return _image; }
            set { _image = value; OnPropertyChanged(nameof(Image)); }
        }


        public double Done
        {
            get { return _done; }
            set { _done = value; OnPropertyChanged(nameof(Done)); }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(nameof(Status)); }
        }

        public bool StartButtonIsEnabled
        {
            get { return _StartButtonIsEnabled; }
            set { _StartButtonIsEnabled = value; OnPropertyChanged(nameof(StartButtonIsEnabled)); }
        }

        public bool CancelButtonIsEnabled
        {
            get { return _CancelButtonIsEnabled; }
            set { _CancelButtonIsEnabled = value; OnPropertyChanged(nameof(CancelButtonIsEnabled)); }
        }

    }
}
