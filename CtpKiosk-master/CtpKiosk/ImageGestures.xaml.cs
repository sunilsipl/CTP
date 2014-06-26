using Limilabs.Client.SMTP;
using Limilabs.Mail;
using Limilabs.Mail.Headers;
using Limilabs.Mail.MIME;
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
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CtpKiosk
{

    
    /// </summary>
    public sealed partial class ImageGestures : Page
    {

        private CompositeTransform transform;

        public ImageGestures()
        {
            this.InitializeComponent();
        }

        private void TestRectangle_PointerPressed(object sender,
    PointerRoutedEventArgs e)
        {
            Rectangle rect = sender as Rectangle;

            // Change the size of the Rectangle
            if (null != rect)
            {
                rect.Width = 250;
                rect.Height = 150;
            }
        }

        private void TestRectangle_PointerReleased(object sender,
            PointerRoutedEventArgs e)
        {
            Rectangle rect = sender as Rectangle;

            // Reset the dimensions on the Rectangle
            if (null != rect)
            {
                rect.Width = 200;
                rect.Height = 100;
            }
        }

        private void TestRectangle_PointerExited(object sender,
            PointerRoutedEventArgs e)
        {
            Rectangle rect = sender as Rectangle;

            // Finger moved out of Rectangle before the pointer exited event
            // Reset the dimensions on the Rectangle
            if (null != rect)
            {
                rect.Width = 200;
                rect.Height = 100;
            }
        }

        private void Img_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            
        }

       
        protected override void OnManipulationDelta(ManipulationDeltaRoutedEventArgs args)
        {
            // All the Image elements have ManipulationMode = All enabled
            // The other elements on the page have manipulations disabled
            // therefore the OriginalSource can only be an image, no need to test for null

            var currentImage = args.OriginalSource as Image;
         //   var transform = currentImage.RenderTransform as CompositeTransform;



            transform = new CompositeTransform();

            transform.TranslateX += args.Delta.Translation.X;
            transform.TranslateY += args.Delta.Translation.Y;

           // transform.ScaleX *= args.Delta.Scale;
         //   transform.ScaleY *= args.Delta.Scale;

            transform.ScaleX += 1;
            transform.ScaleY += 1;

            Test.RenderTransform = this.transform;

            transform.Rotation += args.Delta.Rotation;
            base.OnManipulationDelta(args);


          /*  StorageFile imageFile;
StorageFile attachmentFile;

MailBuilder builder = new MailBuilder();
builder.Html = @"Html with an image: <img src="" cid:lena@example.com""="">";
MimeData image = await builder.AddVisual(imageFile);
image.ContentId = "lena@example.com";
await builder.AddAttachment(attachmentFile);
builder.To.Add(new MailBox("to@example.com"));
          //  builder.AddAttachment()
builder.From.Add(new MailBox("from@example.com"));
builder.Subject = "Subject";

IMail email = builder.Create();


using(Smtp smtp = new Smtp())
{
    await smtp.Connect("smtp.server.com");  // or ConnectSSL for SSL
  
    await smtp.UseBestLoginAsync("user", "password");

    await smtp.SendMessageAsync(email);                       

    await smtp.CloseAsync();    
}       */        


        }

    }
}
