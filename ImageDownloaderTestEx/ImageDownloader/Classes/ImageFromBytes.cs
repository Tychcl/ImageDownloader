using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageDownloader.Classes
{
    public static class ImageFromBytes
    {
        public static BitmapImage Byte2Image(byte[] photo)
        {
            BitmapImage image = new BitmapImage();
            using (var Stream = new MemoryStream(photo))
            {
                Stream.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = Stream;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
