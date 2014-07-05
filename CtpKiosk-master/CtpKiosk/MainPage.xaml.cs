using EASendMailRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI.ViewManagement;
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
    public sealed partial class MainPage : Page
    {
        string screenType;

        public MainPage()
        {
            this.InitializeComponent();          
        }
        //It will give nevigation on square cake editor 
        private void First_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EditorResctangle));       
        }
        //It will give nevigation for cupcakes, cookies and circular cake editor 
        private void Second_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Editor));
        }
        //It will give nevigation on square cake editor
        private void Third_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CustomerInformation), "fromMainPage");
        }
        //It will give nevigation on square cake editor
        private void Fourth_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BorderNew));
        }
        // It will give nevigation to update bakers email
        private void Fifth_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UpdatingEmailId));
        }        

    }
    public enum NotifyType
    {
        StatusMessage,
        ErrorMessage
    };
}
