using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
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
    public sealed partial class UpdatingEmailId : Page
    {
        string emailId;
        
        public UpdatingEmailId()
        {
            this.InitializeComponent();
            CheckXmlExist();
        }

        public async void CheckXmlExist() {
            try
            {
                Windows.Storage.StorageFolder storageFolder = ApplicationData.Current.RoamingFolder;
                //  StorageFile storageFile = await storageFolder.GetFileAsync("myXML.xml");
                var isExist = await storageFolder.TryGetItemAsync("myXML.xml");

                if (isExist != null)
                {
                    //  this.Frame.Navigate(typeof(UpdatingEmailId)); 
                    BackButton.Visibility = Visibility.Visible;
                }
                else { btnUpdate.Content = "Save Email"; }
            }

            catch (Exception ex) { }
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
        /// Get email id to update in xml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TxtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            emailId = TxtEmail.Text;            
        }
        /// <summary>
        /// Update emilid in xml file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void Update_EmailId_Click(object sender, RoutedEventArgs e)
        {
            if (emailId == null)
            {
                tblkErrorMessage.Text = "Email Id is required";
            }
            else if (!emailId.Contains("@") || !emailId.Contains("."))
            {
                tblkErrorMessage.Text = "Email Id is not valid";
            }
            else
            {
                try
                {
                    tblkErrorMessage.Text = "";
                    TxtEmail.Text = "";
                    //Create xml document
                    XmlDocument objXDoc = new XmlDocument();
                    XmlElement objXElement;
                    //Add comment
                    XmlComment comment = objXDoc.CreateComment("This is email id");
                    objXDoc.AppendChild(comment);
                    //Create element "xml"
                    objXElement = objXDoc.CreateElement("xml");
                    objXDoc.AppendChild(objXElement);
                    //Create second element "EmailId"
                    XmlElement objXElement2 = objXDoc.CreateElement("EmailId");
                    //Add text in second element and append it with first element
                    objXElement2.InnerText = emailId;
                    objXElement.AppendChild(objXElement2);

                    //Save xml to roaming folder. Ex--The location of file is:- C:\Users\SHEETAL\AppData\Local\Packages\3cc52d37-51d9-47b9-abc2-a0cfba44b4c0_2pyw4mctbxr1a\RoamingState
                    Windows.Storage.StorageFolder storageFolder = ApplicationData.Current.RoamingFolder;
                    StorageFile storageFile = await storageFolder.CreateFileAsync("myXML.xml", CreationCollisionOption.ReplaceExisting);
                    await objXDoc.SaveToFileAsync(storageFile);
                    MessageDialog msgDialog = new MessageDialog("Email saved");
                    await msgDialog.ShowAsync();
                    this.Frame.Navigate(typeof(MainPage));
                }
                catch (Exception ex)
                {
                    MessageDialog msgDialog = new MessageDialog(ex.Message);
                    msgDialog.ShowAsync();
                }
            }
        }
    }
}
