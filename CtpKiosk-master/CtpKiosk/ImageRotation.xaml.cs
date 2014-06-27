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
    public sealed partial class ImageRotation : Page
    {
        public ImageRotation()
        {
            this.InitializeComponent();
        }

      /*  private void btnRotate_Click(object sender, RoutedEventArgs e)
        {
            var imgTargetSec = (CompositeTransform)imgRotation.RenderTransform;
            var angle = imgTargetSec.Rotation;
            imgTargetSec.Rotation += 90;
        }*/


        private void Image_ManipulationDelta_1(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var ct = (CompositeTransform)myImage.RenderTransform;
            ct.ScaleX *= e.Delta.Scale;
            ct.ScaleY *= e.Delta.Scale;
            ct.TranslateX += e.Delta.Translation.X;
            ct.TranslateY += e.Delta.Translation.Y;
            ct.Rotation += 180.0 / Math.PI * e.Delta.Rotation;
           // UpdateControls(ct);
        }

    }
}
