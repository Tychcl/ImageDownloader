﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.ViewModel
{
    public class VMMainWindow : INotifyPropertyChanged
    {
        public List<VMUserDownloader> Downloaders { get; } = new();
        private readonly int _totalDownloaders;

        private double _totalProgress;
        public double TotalProgress
        {
            get => _totalProgress;
            set
            {
                _totalProgress = value;
                OnPropertyChanged();
            }
        }

        public VMMainWindow(int downloadersCount)
        {
            _totalDownloaders = downloadersCount;
            for (int i = 0; i < downloadersCount; i++)
            {
                var downloader = new VMUserDownloader();
                Downloaders.Add(downloader);
                VMUserDownloader.ProgressChanged += HandleProgressChanged;
            }
        }

        private void HandleProgressChanged(double progress)
        {
            double sum = Downloaders.Sum(d => d.Progress);
            TotalProgress = sum / _totalDownloaders;
        }

        private RelayCommand _downloadAll;
        public RelayCommand DownloadAll
        {
            get
            {
                return _downloadAll ?? (_downloadAll = new RelayCommand(obj =>
                {
                    foreach (var downloader in Downloaders)
                    {
                        downloader.StartDownload.Execute(null);
                    }
                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
