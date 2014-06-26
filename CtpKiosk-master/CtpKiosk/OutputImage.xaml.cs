using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CtpKiosk
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OutputImage : Page
    {
        public OutputImage()
        {
            this.InitializeComponent();
            DisplayImage();
        }

        private async void DisplayImage()
        {
            StorageFolder sFolder = ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile image = await sFolder.GetFileAsync("CanvasFile.jpg");   // Get the image from local folder
            var stream = await image.OpenReadAsync();
            var bitmapImage = new BitmapImage();
            bitmapImage.SetSource(stream);
            fullImage.Source = bitmapImage;        // Set the source of image from bitmap image which we fetch from applicaiton local folder
        }
    }
}
