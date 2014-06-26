using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CtpKiosk
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InputImage : BasePrintPage
    {
        public InputImage()
        {
            this.InitializeComponent();
        }

        private async void InvokePrintButtonClick(object sender, RoutedEventArgs e)
        {
            
            RegisterForPrinting();
            await Windows.Graphics.Printing.PrintManager.ShowPrintUIAsync();
        }
        protected override void PreparePrintContent()
        {
            if (firstPage == null)
            {
                firstPage = new OutputImage();
               
               // StackPanel header = (StackPanel)firstPage.FindName("header");
               // header.Visibility = Windows.UI.Xaml.Visibility.Visible;
                Canvas cvsImgs = (Canvas)firstPage.FindName("cvsImages");
                cvsImgs.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

            // Add the (newley created) page to the printing root which is part of the visual tree and force it to go
            // through layout so that the linked containers correctly distribute the content inside them.
            PrintingRoot.Children.Add(firstPage);
            PrintingRoot.InvalidateMeasure();
            PrintingRoot.UpdateLayout();
        }

    }
}
