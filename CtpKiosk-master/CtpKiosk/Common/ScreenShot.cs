using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using CtpKiosk.Common;

namespace CtpKiosk.Common
{
  public class ScreenShot
    {
      /// <summary>
      /// This functions saves the captured cancas(edited image)
      /// </summary>
      /// <param name="uielement">Form Element like canvas in our case</param>
      /// <param name="fileName">Name of background image</param>
      /// <param name="isEllipse">True in case of circular cropping</param>
      /// <returns></returns>
      public async Task<RenderTargetBitmap> SaveScreenshotAsync(FrameworkElement uielement, string fileName, bool isEllipse)
        {
            if (isEllipse == true)
            {
                return await SaveToFileAsync(uielement, null);
            }
            else
            {
                var file = await PickSaveImageAsync(fileName);
                if (file == null) {
                    return null;
                } return await SaveToFileAsync(uielement, file);
                
            }
        }

      /// <summary>
      /// This Event open the box to save image
      /// </summary>
      /// <param name="fileName">This is name of file which user set in background</param>
      /// <returns></returns>
        async Task<StorageFile> PickSaveImageAsync(string fileName)
        {
            var filePicker = new FileSavePicker();
            filePicker.FileTypeChoices.Add("Bitmap", new List<string>() { ".bmp" });
            filePicker.FileTypeChoices.Add("JPEG format", new List<string>() { ".jpg" });
            filePicker.FileTypeChoices.Add("Compuserve format", new List<string>() { ".gif" });
            filePicker.FileTypeChoices.Add("Portable Network Graphics", new List<string>() { ".png" });
            filePicker.FileTypeChoices.Add("Tagged Image File Format", new List<string>() { ".tif" });
            filePicker.DefaultFileExtension = ".jpg";
            filePicker.SuggestedFileName = "screenshot";
            if (fileName != null ) {
                filePicker.SuggestedFileName = fileName.ToString();              
            }
           
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            filePicker.SettingsIdentifier = "picture picker";
            filePicker.CommitButtonText = "Save picture";

            return await filePicker.PickSaveFileAsync();
        }
      /// <summary>
      /// Used to save file
      /// </summary>
      /// <param name="uielement"></param>
      /// <param name="file"></param>
      /// <returns></returns>
        async Task<RenderTargetBitmap> SaveToFileAsync(FrameworkElement uielement, StorageFile file)
        {
            Guid encoderId;
            if (file != null)
            {
                ImagePathClass.ImageType = file.FileType;
                CachedFileManager.DeferUpdates(file);
                encoderId = GetBitmapEncoder(file.FileType);
                try
                {
                    StorageFolder sFolder = ApplicationData.Current.LocalFolder;
                    StorageFile sFile = await sFolder.CreateFileAsync("dummyimage.jpg", CreationCollisionOption.ReplaceExisting);
                    StorageFile pFile = await Windows.Storage.KnownFolders.PicturesLibrary.CreateFileAsync("dummyimage.jpg", CreationCollisionOption.ReplaceExisting);
                    using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        using (var stream1 = await sFile.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            using (var stream2 = await pFile.OpenAsync(FileAccessMode.ReadWrite))
                            {
                                await CaptureToStreamAsync(uielement, stream2, encoderId);
                            }
                            await CaptureToStreamAsync(uielement, stream1, encoderId);
                        }
                        return await CaptureToStreamAsync(uielement, stream, encoderId);
                    }

                }
                catch (Exception ex)
                {
                    DisplayMessage(ex.Message);
                }
                var status = await CachedFileManager.CompleteUpdatesAsync(file);
            }
            else
            {
                encoderId = GetBitmapEncoder("jpg");
                try
                {
                    StorageFolder sFolder = ApplicationData.Current.LocalFolder;
                    StorageFile sFile = await sFolder.CreateFileAsync("screenshot.jpg", CreationCollisionOption.ReplaceExisting);
                    using (var stream = await sFile.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        return await CaptureToStreamAsync(uielement, stream, encoderId);
                    }
                }
                catch (Exception ex)
                {
                    DisplayMessage(ex.Message);
                }
                // var status = await CachedFileManager.CompleteUpdatesAsync(sFile);
            }

            return null;
        }

      /// <summary>
      /// Used to get encoder id
      /// </summary>
      /// <param name="fileType"></param>
      /// <returns></returns>
        Guid GetBitmapEncoder(string fileType)
        {
            Guid encoderId = BitmapEncoder.JpegEncoderId;
            switch (fileType)
            {
                case ".bmp":
                    encoderId = BitmapEncoder.BmpEncoderId;
                    break;
                case ".gif":
                    encoderId = BitmapEncoder.GifEncoderId;
                    break;
                case ".png":
                    encoderId = BitmapEncoder.PngEncoderId;
                    break;
                case ".tif":
                    encoderId = BitmapEncoder.TiffEncoderId;
                    break;
            }

            return encoderId;
        }

      /// <summary>
      /// Used to get bitmap
      /// </summary>
      /// <param name="uielement"></param>
      /// <param name="stream"></param>
      /// <param name="encoderId"></param>
      /// <returns></returns>
        async Task<RenderTargetBitmap> CaptureToStreamAsync(FrameworkElement uielement, IRandomAccessStream stream, Guid encoderId)
        {
            try
            {
                var renderTargetBitmap = new RenderTargetBitmap();
                await renderTargetBitmap.RenderAsync(uielement);

                var pixels = await renderTargetBitmap.GetPixelsAsync();

                var logicalDpi = DisplayInformation.GetForCurrentView().LogicalDpi;
                var encoder = await BitmapEncoder.CreateAsync(encoderId, stream);
                encoder.SetPixelData(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Ignore,
                    (uint)renderTargetBitmap.PixelWidth,
                    (uint)renderTargetBitmap.PixelHeight,
                    logicalDpi,
                    logicalDpi,
                    pixels.ToArray());

                await encoder.FlushAsync();

                return renderTargetBitmap;
            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message);
            }

            return null;
        }

      /// <summary>
      /// Used to show error message if any
      /// </summary>
      /// <param name="error"></param>
       public async void DisplayMessage(string error)
        {
            var dialog = new MessageDialog(error);

            await dialog.ShowAsync();
        }
    }
}
