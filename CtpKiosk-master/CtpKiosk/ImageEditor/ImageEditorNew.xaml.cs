using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Popups;
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
    public sealed partial class ImageEditorNew : BasePrintPage
    {
        double hMargin = 0;
        double vMargin = 0;
        double hSpacing = 0;
        double vSpacing = 0;
        string param = "";

        public ImageEditorNew()
        {
            this.InitializeComponent();
            //cvsEditorImages.Margin = new Thickness { Left = 0, Top = 0, Right = 0, Bottom = 0 };
            DisplayImage();
            SaveCanvas();
        }

        /// <summary>
        /// This event is used to get parameters
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            param = Convert.ToString(e.Parameter);

            // Check for border or square shape
            if (param != "")
            {
              // It will Display border strip
               cvsBorder.Visibility = Windows.UI.Xaml.Visibility.Visible;
               DisplayBorder();
               CmbImagesNumber.Items.Clear();
               CmbImagesNumber.Items.Add(new ComboBoxItem { Content = "1" });
               CmbImagesNumber.Items.Add(new ComboBoxItem { Content = "2" });
               CmbImagesNumber.Items.Add(new ComboBoxItem { Content = "3" });
               CmbImagesNumber.Items.Add(new ComboBoxItem { Content = "4" });
               numHSpacing.Minimum = 1;

            }
            else
            {  
              // It will disply square or circle 
                cvsEditorImages.Visibility = Windows.UI.Xaml.Visibility.Visible;
                cvsBorder.Visibility = Windows.UI.Xaml.Visibility.Collapsed;            
            }
        }

        /// <summary>
        /// Used to show image on screen
        /// </summary>
        private async void DisplayImage()
        {            
           // fullImage.Visibility = Visibility.Collapsed;
            StorageFolder sFolder = ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile image = await sFolder.GetFileAsync("screenshot.jpg");  // Get the image from application local folder
            var stream = await image.OpenReadAsync();
            var bitmapImage = new BitmapImage();
            bitmapImage.SetSource(stream);
            firstImage.Source = bitmapImage;
            firstImage.Width = 200;
            firstImage.Height = 150;
            secondImage.Source = bitmapImage;
            secondImage.Width = 200;
            secondImage.Height = 150;
            thirdImage.Source = bitmapImage;
            thirdImage.Width = 200;
            thirdImage.Height = 150;
            fourthImage.Source = bitmapImage;
            fourthImage.Width = 200;
            fourthImage.Height = 150;
            fifthImage.Source = bitmapImage;
            fifthImage.Width = 200;
            fifthImage.Height = 150;
            sixthImage.Source = bitmapImage;
            sixthImage.Width = 200;
            sixthImage.Height = 150;
            cvsEditorImages.Margin = new Thickness { Left = 230,Right = 133, Top = -10,Bottom = 118 };
        }


        /// <summary>
        /// This function display the border
        /// </summary>
        private async void DisplayBorder() {

            StorageFolder sFolder = ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile image = await sFolder.GetFileAsync("screenshot.jpg");  // Get the image from application local folder
            var stream = await image.OpenReadAsync();
            var bitmapImage = new BitmapImage();
            bitmapImage.SetSource(stream);
            brdImg1.Source = bitmapImage;
            brdImg2.Source = bitmapImage;
            brdImg3.Source = bitmapImage;
            brdImg4.Source = bitmapImage;
            SaveCanvas();
       }


        /// <summary>
        /// Click of back button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// Method used to adjust margin and spacing horizontally and vertically
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void numUpDown_TextChanged(object sender, TextChangedEventArgs e)
        {
           // fullImage.Visibility = Visibility.Collapsed;
            hMargin = Convert.ToDouble(numHMargin.Text);
            vMargin = Convert.ToDouble(numVMargin.Text);
            hSpacing = Convert.ToDouble(numHSpacing.Text);
            vSpacing = Convert.ToDouble(numVSpacing.Text);

            if (param == "")
            {
                spnlFirst.Margin = new Thickness { Left = hMargin, Top = vMargin, Right = 0, Bottom = 0 };
                spnlSecond.Margin = new Thickness { Left = 0, Top = vMargin, Right = 0, Bottom = 0 };
                firstImage.Margin = new Thickness { Left = hMargin, Top = 0, Right = hSpacing, Bottom = 0 };
                secondImage.Margin = new Thickness { Left = hMargin, Top = vSpacing, Right = hSpacing, Bottom = 0 };
                thirdImage.Margin = new Thickness { Left = hMargin, Top = vSpacing, Right = hSpacing, Bottom = 0 };
                fourthImage.Margin = new Thickness { Left = hSpacing, Top = 0, Right = 0, Bottom = 0 };
                fifthImage.Margin = new Thickness { Left = hSpacing, Top = vSpacing, Right = 0, Bottom = 0 };
                sixthImage.Margin = new Thickness { Left = hSpacing, Top = vSpacing, Right = 0, Bottom = 0 };
            }

            else {

                if (hSpacing < 0) { hSpacing = 20; }

                spnBorder.Margin = new Thickness { Left = hMargin-10, Top = vMargin-30, Right = 0, Bottom = 0 };
                brdImg1.Margin = new Thickness { Left = hSpacing, Top = 10, Right = 0, Bottom = 0 };
                brdImg2.Margin = new Thickness { Left = hSpacing, Top = 10, Right = 0, Bottom = 0 };
                brdImg3.Margin = new Thickness { Left = hSpacing, Top = 10, Right = 0, Bottom = 0 };
                brdImg4.Margin = new Thickness { Left = hSpacing, Top =10, Right = 0, Bottom = 0 };
            }

            SaveCanvas();
            //cvsEditorImages.Margin = new Thickness { Left = 433, Right = 10, Top = 133, Bottom = 118 };
        }


        /// <summary>
        /// Used to get number of images from drop down and display image on canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CmbImagesNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {           
            ComboBoxItem comboBoxItem = (sender as ComboBox).SelectedItem as ComboBoxItem;
            var number = comboBoxItem.Content.ToString();
            //   Windows.Storage.StorageFile image =
            // await Windows.Storage.KnownFolders.PicturesLibrary.GetFileAsync("screenshot.jpg");
            StorageFolder sFolder = ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile image = await sFolder.GetFileAsync("screenshot.jpg");
            var stream = await image.OpenReadAsync();
            var bitmapImage = new BitmapImage();
            bitmapImage.SetSource(stream);
            
           // bitmapImage.
            if (param == "")
            {

                //Check condition and then display images accordingly
                if (number == "1")   //show 1 image
                {
                    fullImage.Visibility = Visibility.Visible;
                    fullImage.Source = bitmapImage;
                    firstImage.Visibility = Visibility.Collapsed;
                    secondImage.Visibility = Visibility.Collapsed;
                    thirdImage.Visibility = Visibility.Collapsed;
                    fourthImage.Visibility = Visibility.Collapsed;
                    fifthImage.Visibility = Visibility.Collapsed;
                    sixthImage.Visibility = Visibility.Collapsed;
                    SaveCanvas();
                    // cvsEditorImages.Margin = new Thickness { Left = 433, Right = 10, Top = 133, Bottom = 118 };
                }
                else if (number == "2")   //show 2 images
                {
                    //  SaveCanvas();
                    fullImage.Visibility = Visibility.Collapsed;
                    firstImage.Visibility = Visibility.Visible;
                    secondImage.Visibility = Visibility.Visible;
                    firstImage.Source = bitmapImage;
                    firstImage.Width = 600;
                    firstImage.Height = 250;
                    firstImage.Margin = new Thickness { Left = 0, Top = -10, Right = 0, Bottom = 0 };
                    secondImage.Source = bitmapImage;
                    secondImage.Width = 600;
                    secondImage.Height = 250;
                    secondImage.Margin = new Thickness { Left = 0, Top = 19, Right = 0, Bottom = 0 };
                    thirdImage.Visibility = Visibility.Collapsed;
                    fourthImage.Visibility = Visibility.Collapsed;
                    fifthImage.Visibility = Visibility.Collapsed;
                    sixthImage.Visibility = Visibility.Collapsed;
                    SaveCanvas();
                    // cvsEditorImages.Margin = new Thickness { Left = 433, Right = 10, Top = 133, Bottom = 118 };
                }
                else if (number == "4")    //show 4 images
                {
                    fullImage.Visibility = Visibility.Collapsed;
                    firstImage.Visibility = Visibility.Visible;
                    secondImage.Visibility = Visibility.Collapsed;
                    thirdImage.Visibility = Visibility.Visible;
                    fourthImage.Visibility = Visibility.Visible;
                    fifthImage.Visibility = Visibility.Collapsed;
                    sixthImage.Visibility = Visibility.Visible;
                    firstImage.Width = 250;
                    firstImage.Height = 250;
                    fourthImage.Width = 250;
                    fourthImage.Height = 250;
                    thirdImage.Width = 250;
                    thirdImage.Height = 250;
                    sixthImage.Width = 250;
                    sixthImage.Height = 250;
                    firstImage.Margin = new Thickness { Left = 0, Top = 0, Right = 0, Bottom = 0 };
                    thirdImage.Margin = new Thickness { Left = 0, Top = -30, Right = 0, Bottom = 0 };
                    fourthImage.Margin = new Thickness { Left = 10, Top = 0, Right = 0, Bottom = 0 };
                    sixthImage.Margin = new Thickness { Left = 10, Top = -30, Right = 0, Bottom = 0 };
                    SaveCanvas();
                    // cvsEditorImages.Margin = new Thickness { Left = 433, Right = 10, Top = 133, Bottom = 118 };
                }
                else if (number == "6")  //show 6 images
                {
                    fullImage.Visibility = Visibility.Collapsed;
                    firstImage.Visibility = Visibility.Visible;
                    secondImage.Visibility = Visibility.Visible;
                    thirdImage.Visibility = Visibility.Visible;
                    fourthImage.Visibility = Visibility.Visible;
                    fifthImage.Visibility = Visibility.Visible;
                    sixthImage.Visibility = Visibility.Visible;
                    firstImage.Width = 200;
                    firstImage.Height = 150;
                    secondImage.Width = 200;
                    secondImage.Height = 150;
                    thirdImage.Width = 200;
                    thirdImage.Height = 150;
                    fourthImage.Width = 200;
                    fourthImage.Height = 150;
                    fifthImage.Width = 200;
                    fifthImage.Height = 150;
                    sixthImage.Width = 200;
                    sixthImage.Height = 150;
                    hMargin = Convert.ToDouble(numHMargin.Text);
                    vMargin = Convert.ToDouble(numVMargin.Text);
                    hSpacing = Convert.ToDouble(numHSpacing.Text);
                    vSpacing = Convert.ToDouble(numVSpacing.Text);
                    spnlFirst.Margin = new Thickness { Left = hMargin, Top = vMargin, Right = 0, Bottom = 0 };
                    spnlSecond.Margin = new Thickness { Left = 0, Top = vMargin, Right = 0, Bottom = 0 };
                    firstImage.Margin = new Thickness { Left = hMargin, Top = 0, Right = hSpacing, Bottom = 0 };
                    secondImage.Margin = new Thickness { Left = hMargin, Top = vSpacing, Right = hSpacing, Bottom = 0 };
                    thirdImage.Margin = new Thickness { Left = hMargin, Top = vSpacing, Right = hSpacing, Bottom = 0 };
                    fourthImage.Margin = new Thickness { Left = hSpacing, Top = 0, Right = 0, Bottom = 0 };
                    fifthImage.Margin = new Thickness { Left = hSpacing, Top = vSpacing, Right = 0, Bottom = 0 };
                    sixthImage.Margin = new Thickness { Left = hSpacing, Top = vSpacing, Right = 0, Bottom = 0 };
                    SaveCanvas();
                    // cvsEditorImages.Margin = new Thickness { Left = 433, Right = 10, Top = 133, Bottom = 118 };
                }
            } // End param = ""

            else {

                switch (Convert.ToInt32(number))    // Case for display number of border according to selected value
                { 
                    case 1:
                        brdImg2.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        brdImg3.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        SaveCanvas();
                        break;

                    case 2:
                        brdImg3.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        brdImg4.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        brdImg2.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        SaveCanvas();
                        break;

                    case 3:
                        brdImg1.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        brdImg2.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        brdImg3.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        brdImg4.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        SaveCanvas();
                        break;
                    case 4:
                        brdImg1.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        brdImg2.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        brdImg3.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        brdImg4.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        SaveCanvas();
                        break;

                }
            }

        }


        /// <summary>
        /// Called on click of print button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void InvokePrintButtonClick(object sender, RoutedEventArgs e)
        {
           // cvsEditorImages.Margin = new Thickness { Left = 230, Right = 133, Top = -10, Bottom = 118 };
            try
            {
                SaveCanvas();        
                firstPage = null;      
                UnregisterForPrinting();   // Unregister the previous print events
                                          //  Save the current edited image
                RegisterForPrinting();    //  Register the previous print events
                imgsPrint.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                await Windows.Graphics.Printing.PrintManager.ShowPrintUIAsync();  // Invoke the print window
            }
            catch (Exception ex)
            {
                MessageDialog msgDialog1 = new MessageDialog(ex.Message);
                msgDialog1.ShowAsync();
            }
        }

        /// <summary>
        /// Used to prepare content for print
        /// </summary>
        protected override void PreparePrintContent()
        {
            if (firstPage == null)
            {
                firstPage = new OutputImage();    // This is display on print preview on printer 
                Canvas cvsImgs = (Canvas)firstPage.FindName("cvsImages");
                cvsImgs.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

            // Add the (newley created) page to the printing root which is part of the visual tree and force it to go
            // through layout so that the linked containers correctly distribute the content inside them.
            PrintingRoot.Children.Add(firstPage);
            PrintingRoot.InvalidateMeasure();
            PrintingRoot.UpdateLayout();
        }

        /// <summary>
        /// This function save the current canvas 
        /// </summary>
        private async void SaveCanvas()
        {
            string filename = "CanvasFile.jpg";
            try
            {
                RenderTargetBitmap bitmap = await this.CanvasToBMP();
                await this.SaveToPNG(bitmap, filename);                
            }
            catch (Exception ex) { }
        }
        /// <summary>
        /// This function convert canvas image to bitmap
        /// </summary>
        /// <returns></returns>
        private async Task<RenderTargetBitmap> CanvasToBMP()
        {
            // Initialization  
            RenderTargetBitmap bitmap = null;
            try
            {
                if (param == "")
                {
                    //// Initialization.  
                    Size canvasSize = this.cvsEditorImages.RenderSize;
                    Point defaultPoint = this.cvsEditorImages.RenderTransformOrigin;
                    // Sezing to output image dimension.  
                    this.cvsEditorImages.Measure(canvasSize);
                    //this.cvsEditorImages.UpdateLayout();
                    //this.cvsEditorImages.Arrange(new Rect(defaultPoint, canvasSize));
                    var bmp = new RenderTargetBitmap();    // Convert canvas to bmp. 
                    await bmp.RenderAsync(this.cvsEditorImages);
                    bitmap = bmp as RenderTargetBitmap;
                }

                else 
                {
                    //// Initialization.  
                    Size canvasSize = this.cvsBorder.RenderSize;
                    Point defaultPoint = this.cvsBorder.RenderTransformOrigin;
                    // Sezing to output image dimension.  
                    this.cvsEditorImages.Measure(canvasSize);
                    //this.cvsEditorImages.UpdateLayout();
                    //this.cvsEditorImages.Arrange(new Rect(defaultPoint, canvasSize));
                    var bmp = new RenderTargetBitmap();    // Convert canvas to bmp. 
                    await bmp.RenderAsync(this.cvsBorder);
                    bitmap = bmp as RenderTargetBitmap;
                }
            }
            catch (Exception ex) { }
            return bitmap;
        }

        /// <summary>
        /// This function encode the bitmap to png
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        private async Task SaveToPNG(RenderTargetBitmap bitmap, string filename)
        {
            // Verification.  
            try
            {
                // Create file.  
                StorageFolder sFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await sFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                // Verification.  
                if (bitmap == null) { return; }
                // Saving to file.  
                using (var stream = await file.OpenStreamForWriteAsync())
                {
                    // Initialization.  
                    var pixelBuffer = await bitmap.GetPixelsAsync();
                    var logicalDpi = DisplayInformation.GetForCurrentView().LogicalDpi;
                    // convert stream to IRandomAccessStream  
                    var randomAccessStream = stream.AsRandomAccessStream();
                    // encoding to PNG  
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, randomAccessStream);
                    // Finish saving  
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)bitmap.PixelWidth,
                               (uint)bitmap.PixelHeight, logicalDpi, logicalDpi, pixelBuffer.ToArray());
                    // Flush encoder.  
                    await encoder.FlushAsync();
                }
            }
            catch (Exception ex) { }
        }
    }
}
