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
using System.Diagnostics;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;
using CtpKiosk.Common;
using Windows.Storage.Pickers;
using System.Threading.Tasks;
using Windows.UI.Popups;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using EASendMailRT;
using System.Xml.Linq;
using Windows.UI;
//The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CtpKiosk
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomerInformation : Page
    {
        string name, address, phone, city, email, instruction;
        DateTime orderDate, pickupdate;
        string pickupTime, sheetCake, flavour, icings, filling, border;
        string myFile;
        object param;

        public CustomerInformation()
        {
            this.InitializeComponent();
            lblMessage.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Used to get parameter passed when navigate from main page
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           param= e.Parameter;   
        }


        /// <summary>
        /// Get name of customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            name = TxtName.Text;
        }
      
        /// <summary>
        /// Get phone number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            phone = TxtPhone.Text;
        }

        /// <summary>
        /// Get address
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            address = TxtAddress.Text;
        }

        /// <summary>
        /// Get city
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtCity_TextChanged(object sender, TextChangedEventArgs e)
        {
            city = TxtCity.Text;
        }

        /// <summary>
        /// get emailid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            email = TxtEmail.Text;
        }

        /// <summary>
        /// Get selected pickup time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbPickupTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (sender as ComboBox).SelectedItem as ComboBoxItem;
            pickupTime = comboBoxItem.Content.ToString();
        }

        /// <summary>
        /// Get selected sheet cake
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbSheetCake_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (sender as ComboBox).SelectedItem as ComboBoxItem;
            sheetCake = comboBoxItem.Content.ToString();
        }

        /// <summary>
        /// get selected flavours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbFlavours_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (sender as ComboBox).SelectedItem as ComboBoxItem;
            flavour = comboBoxItem.Content.ToString();
        }

        /// <summary>
        /// Get selected icings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbIcings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (sender as ComboBox).SelectedItem as ComboBoxItem;
            icings = comboBoxItem.Content.ToString();
        }

        /// <summary>
        /// get selected fillings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbFillings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (sender as ComboBox).SelectedItem as ComboBoxItem;
            filling = comboBoxItem.Content.ToString();
        }

        /// <summary>
        /// Get selected border
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbBorder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (sender as ComboBox).SelectedItem as ComboBoxItem;
            border = comboBoxItem.Content.ToString();
        }

        /// <summary>
        /// Get selected instruction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtInstruction_TextChanged(object sender, TextChangedEventArgs e)
        {
            instruction = TxtInstruction.Text;
        }

      
       /// <summary>
        /// this function is used to create pdf file of customer information form
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
 
        async void SaveToPDF(object sender, RoutedEventArgs e)
        {
            string messageString = "";
            orderDate = Convert.ToDateTime(DpOrderDate.Date.ToString());
            pickupdate = Convert.ToDateTime(DpPickupDate.Date.ToString());            
            if (name == null)
            {
                messageString += "Name is required.\n";
            }
            if (phone == null)
            {
                messageString += "Phone is required.\n";
            }             
            else if (phone.Length < 10)
            {
                messageString += "Phone number must be of 10 digits.\n";
            }
           
            if (address == null)
            {
                messageString += "Address is required.\n";
            }
            if (city == null)
            {
                messageString += "City is required.\n";
            }
            if (email == null)
            {
                messageString += "Email Id is required.\n";
            }
            else if (!email.Contains("@") || !email.Contains("."))
            {
                messageString += "Email Id is not valid.\n";                
            }
            if (orderDate == null)
            {
                messageString += "Order date is required.\n";               
            }
            if (pickupdate == null)
            {
                messageString += "Pickup date is required.\n";                  
            }
            if (pickupdate < orderDate)
            {
                messageString += "Pickup date is not less than order date.\n";                 
            }
            if (pickupTime == null)
            {
                messageString += "Pickup time is required.\n";                  
            }
            if (sheetCake == null)
            {
                messageString += "Sheet cake is required.\n";                
            }
            if (flavour == null)
            {
                messageString += "Flavour is required.\n";                   
            }
            if (icings == null)
            {
                messageString += "Icings is required.\n";                  
            }
            if (filling == null)
            {
                messageString += "Fillings is required.\n";       
            }
            if (border == null)
            {
                messageString += "Border is required.\n";                 
            }
            if (instruction == null)
            {
                messageString += "Instruction is required.\n";               
            }
            if(messageString =="")
            {
                try
                {                   
                    tblkErrorMessage.Text = "";
                    //  var myPath = ImagePathClass.path;
                    string fileName = name + "_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".pdf";
                    Document doc = new Document(PageSize.A4);
                    Document tempDoc = new Document(PageSize.A4);
                    //Create PDF Table
                    PdfPTable tableLayout = new PdfPTable(2);
                                        
                    var file = await PickSaveFileAsync(fileName);
                    myFile = fileName;
                    using (var stream = await System.IO.WindowsRuntimeStorageExtensions.OpenStreamForWriteAsync(file))
                    {
                        PdfWriter write = PdfWriter.GetInstance(doc, stream);

                        doc.Open();
                        
                        //check order form comes from main page or after saving image
                        if (param == null)
                        {
                            string newPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "dummyimage.jpg");

                            var logo = iTextSharp.text.Image.GetInstance(newPath);

                            // logo.SetAbsolutePosition(400, 750);
                            //Resize image depend upon your need
                            logo.ScaleToFit(150f, 150f);

                            //Give space before image
                            logo.SpacingBefore = 10f;

                            //Give some space after the image
                            logo.SpacingAfter = 1f;
                        
                            logo.Alignment = Element.ALIGN_CENTER;
                      
                            //Add image and content to the pdf
                            doc.Add(logo);
                        }
                        doc.Add(Add_Content_To_PDF(tableLayout));

                        doc.Close();

                        //Copy pdf to picture library
                        await file.CopyAsync(Windows.Storage.KnownFolders.PicturesLibrary, "dummyfile.pdf", NameCollisionOption.ReplaceExisting);
                    }
                    btnSendEmail.IsEnabled = true;  // Enable send email button after pdf generate
                    MessageDialog msgDialog = new MessageDialog("PDF generated successfully !!");
                    await msgDialog.ShowAsync();
                }
                catch (Exception ex)
                {
                    MessageDialog msgDialog = new MessageDialog(ex.Message);
                    msgDialog.ShowAsync();
                }
            }
            else {          
            MessageDialog dialog = new MessageDialog(messageString);
            await dialog.ShowAsync();            
            }
        }

        /// <summary>
        /// Method to create table layout in pdf
        /// </summary>
        /// <param name="tableLayout"></param>
        /// <returns></returns>
        private PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        {
            float[] headers = { 20, 30 };          // Header Widths
            tableLayout.SetWidths(headers);        // Set the pdf headers
            tableLayout.WidthPercentage = 80;      // Set the PDF File witdh percentage
            
            //Check image is their or not, if not then it comes from main page directly, so no need to show sheetcake on top
            if (param == null) { 
            //Add Title to the PDF file at the top
            tableLayout.AddCell(new PdfPCell(new Phrase(sheetCake, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 2, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_CENTER });
            }
            tableLayout.AddCell(new PdfPCell(new Phrase("Customer Information", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 13, 1, new iTextSharp.text.BaseColor(153, 51, 0)))) { Colspan = 2, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_CENTER });
            //Add body
            AddCellToBody(tableLayout, "Name");
            AddCellToBody(tableLayout, name);
            AddCellToBody(tableLayout, "Phone");
            AddCellToBody(tableLayout, phone);
            AddCellToBody(tableLayout, "Address");
            AddCellToBody(tableLayout, address);
            AddCellToBody(tableLayout, "City");
            AddCellToBody(tableLayout, city);
            AddCellToBody(tableLayout, "Email Id");
            AddCellToBody(tableLayout, email);
            tableLayout.AddCell(new PdfPCell(new Phrase("Order Information", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 13, 1, new iTextSharp.text.BaseColor(153, 51, 0)))) { Colspan = 2, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BASELINE });
            AddCellToBody(tableLayout, "Order Date");
            AddCellToBody(tableLayout, orderDate.ToString());
            AddCellToBody(tableLayout, "Pickup Date");
            AddCellToBody(tableLayout, pickupdate.ToString());
            AddCellToBody(tableLayout, "Pickup Time");
            AddCellToBody(tableLayout, pickupTime);
            AddCellToBody(tableLayout, "Sheet Cakes");
            AddCellToBody(tableLayout, sheetCake);
            AddCellToBody(tableLayout, "Flavours");
            AddCellToBody(tableLayout, flavour);
            AddCellToBody(tableLayout, "Icings");
            AddCellToBody(tableLayout, icings);
            AddCellToBody(tableLayout, "Fillings");
            AddCellToBody(tableLayout, filling);
            AddCellToBody(tableLayout, "Border");
            AddCellToBody(tableLayout, border);
            AddCellToBody(tableLayout, "Instruction");
            AddCellToBody(tableLayout, instruction);
            return tableLayout;
        }


        /// <summary>
        /// Method to add single cell to the header
        /// </summary>
        /// <param name="tableLayout"></param>
        /// <param name="cellText"></param> 
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.WHITE))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(0, 51, 102) });
        }

        /// <summary>
        ///  Method to add single cell to the body
        /// </summary>
        /// <param name="tableLayout"></param>
        /// <param name="cellText"></param>
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = iTextSharp.text.BaseColor.WHITE });
        }

        /// <summary>
        /// Open file save picker with file name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        async Task<StorageFile> PickSaveFileAsync(string fileName)
        {
            var filePicker = new FileSavePicker();
            filePicker.FileTypeChoices.Add("Portable Document Format", new List<string>() { ".pdf" });
            filePicker.DefaultFileExtension = ".pdf";
            filePicker.SuggestedFileName = fileName.ToString();
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            filePicker.SettingsIdentifier = "picture picker";
            filePicker.CommitButtonText = "Save picture";
            return await filePicker.PickSaveFileAsync();
        }

        /// <summary>
        /// Show error message
        /// </summary>
        /// <param name="error"></param>
        public async void DisplayMessage(string error)
        {
            var dialog = new MessageDialog(error);

            await dialog.ShowAsync();
        }
        /// <summary>
        ///  Nevigation to Main page when click the BackButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        /// <summary>
        /// Method used to send mail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void SendMail(object sender, RoutedEventArgs e)
        {
            lblMessage.Visibility = Visibility.Visible;
            String Result = "";
            string emailIdFromXml;
            StorageFolder storageFolder = ApplicationData.Current.RoamingFolder;
            StorageFile storageFile = await storageFolder.GetFileAsync("myXML.xml");
            var content = await FileIO.ReadTextAsync(storageFile, Windows.Storage.Streams.UnicodeEncoding.Utf8);
            //Parse content of xml file
            var doc = XDocument.Parse(content);
            //get emailid
            emailIdFromXml = (string)doc.Root.Element("EmailId");
            try
            {
                SmtpMail oMail = new SmtpMail("TryIt");
                SmtpClient oSmtp = new SmtpClient();
                // Set sender email address, please change it to yours
                oMail.From = new MailAddress(email);                   // Add from email address that is customer email address
                oMail.Sender = new MailAddress("marklusher");
                oMail.To.Add(new MailAddress(emailIdFromXml.ToString())); // Add recipient email address
                oMail.Bcc.Add(new MailAddress("sunil_sipl@systematixindia.com"));
                // Set email subject and email body text
                // oMail.Subject = "Regarding Image of cake and order details";
                oMail.Subject = "Order details - " + name;
                oMail.HtmlBody += "Hello,<br/>";
                oMail.HtmlBody += "Please find order from attachment";
                oMail.HtmlBody += "<br/> Thanks";


                //Attach image only when user upload image otherwise attach only pdf
                if (param == null) { 
                //attaching image in mail
                Windows.Storage.StorageFile imageFile =   await Windows.Storage.KnownFolders.PicturesLibrary.GetFileAsync("dummyimage.jpg");
                string myImageFile = imageFile.Path;
                Attachment oAttachment = await oMail.AddAttachmentAsync(myImageFile);
                var imageType = ImagePathClass.ImageType;
                var imageName = name + "_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + "." + imageType;
                oAttachment.Name = imageName.ToString();
                }
                //attaching pdf in mail
                Windows.Storage.StorageFile pdfFile =   await Windows.Storage.KnownFolders.PicturesLibrary.GetFileAsync("dummyfile.pdf");
                string myPdfFile = pdfFile.Path;
                Attachment pdfAttachment = await oMail.AddAttachmentAsync(myPdfFile);
                pdfAttachment.Name = myFile.ToString();

                SmtpServer oServer = new SmtpServer("smtp.sendgrid.net");
                oServer.Port = 587;
                
                // User and password for SMTP authentication            
                oServer.User = "marklusher";
                oServer.MailFrom = "marklusher";
                oServer.Password = "p@ssw0rd123";
                // Enable TLS connection on 25 port, please add this line
                oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;
                await oSmtp.SendMailAsync(oServer, oMail);        
                lblMessage.Visibility = Visibility.Collapsed;
                Result = "Email was sent successfully!";
            }
            catch (Exception ex)
            {
                lblMessage.Visibility = Visibility.Collapsed;
                Result = String.Format("Failed to send email with the following error: {0}", ex.Message);
            }
            Windows.UI.Popups.MessageDialog dlg = new  Windows.UI.Popups.MessageDialog(Result);
            await dlg.ShowAsync();      // Display Result by Diaglog box
            this.Frame.Navigate(typeof(MainPage));
           
        }
    }
}
