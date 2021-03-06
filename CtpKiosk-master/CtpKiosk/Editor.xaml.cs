﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
//using Windows.UI.Xaml.Media.Imaging;
using CtpKiosk;
using CtpKiosk.Common;
using Windows.UI.Xaml.Automation;
using Windows.UI.Popups;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CtpKiosk
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Editor : Page
    {
        double leftStart, topStart, markingWidth, markingHeight;
        double selectedLeft, selectedTop, selectedWidth, selectedHeight;
        double clippedImgHeight;
        double clippedImgWidth;
        string filename;
        StorageFile sourceFile;
        string storeCropClick;
        int radius = 150;
        int sliderSpacing;
        double x;
        double y;
        string curveTextColor;
        double newX, newY;
        double curveTextSize = 30;
        string curveTextFont;
        double newSliderRadius = 1;
        string checkCurveSlider;
        bool isTopImageExist = false;
        string screenTest;


        /// <summary>
        /// Constructor
        /// </summary>
        public Editor()
        {
            this.InitializeComponent();
            DetectScreenType();
            btnSave.Click += saveClick;  // Bind the click event of save button with function
        }


        /// <summary>
        /// Function used to detect window size
        /// </summary>
        void DetectScreenType()
        {
            double dpi = DisplayProperties.LogicalDpi;
            var bounds = Window.Current.Bounds;
            double h;
            switch (ApplicationView.Value)         // To detect and set screen resolution
            {
                case ApplicationViewState.Filled:
                    h = bounds.Height;
                    break;

                case ApplicationViewState.FullScreenLandscape:
                    h = bounds.Height;
                    break;

                case ApplicationViewState.Snapped:
                    h = bounds.Height;
                    break;

                case ApplicationViewState.FullScreenPortrait:
                    h = bounds.Width;
                    break;

                default:
                    return;
            }
            double inches = h / dpi;
            screenTest = "Normal";
            if (inches < 10)
            {
                screenTest = "Normal";
                root.Width = 1100;
                root.Height = 550;
                grdImageContainer.MaxWidth = 1100;
                grdImageContainer.MaxHeight = 550;
                target.MaxWidth = 1100;
                target.MaxHeight = 550;
                sliderXAxis.Minimum = 0;
                sliderXAxis.Maximum = 800;
                sliderYAxis.Minimum = 0;
                sliderYAxis.Maximum = 500;
                StandardPopup.VerticalOffset = 40;
                StandardPopup.HorizontalOffset = 600;
                ImageScale.VerticalOffset = 60;
                ImageScale.HorizontalOffset = 750;
                curvePopup.VerticalOffset = 60;
                curvePopup.HorizontalOffset = 750;
                TextEffects.VerticalOffset = 60;
                TextEffects.HorizontalOffset = 750;
                Canvas.SetLeft(stkpnlButtons, -36);
                Canvas.SetTop(stkpnlButtons, 160);
            }
            else if (inches < 14)
            {
                screenTest = "Large";
                root.Width = 1600;
                root.Height = 750;
                grdImageContainer.MaxWidth = 1600;
                grdImageContainer.MaxHeight = 750;
                target.MaxWidth = 1600;
                target.Height = 750;
                sliderXAxis.Minimum = -265;
                sliderXAxis.Maximum = 800;
                sliderYAxis.Minimum = -160;
                sliderYAxis.Maximum = 645;
                StandardPopup.VerticalOffset = 100;
                StandardPopup.HorizontalOffset = 900;
                ImageScale.VerticalOffset = 150;
                ImageScale.HorizontalOffset = 1100;
                curvePopup.VerticalOffset = 150;
                curvePopup.HorizontalOffset = 1100;
                TextEffects.VerticalOffset = 150;
                TextEffects.HorizontalOffset = 1100;
                Canvas.SetLeft(stkpnlButtons, 300);
                Canvas.SetTop(stkpnlButtons, 300);
            }
            else
            {

            }
            //ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            //localSettings.Values["screenType"] = screenType;
        }

        
        /// <summary>
        /// Event execute when we drag the inner image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void target_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (storeCropClick != null)
            {
                // Move the rectangle.
                double height = root.ActualHeight;
                double width = root.ActualWidth;

                try
                {
                    // Drag functionality of image
                    var imgTargetSec = (CompositeTransform)elipsimage.RenderTransform;
                    var scaleY = -(imgTargetSec.ScaleY * 28);    //Set the Y axis boudary to move up               
                    imgTargetSec.TranslateX = Boundary(imgTargetSec.TranslateX + e.Delta.Translation.X, width/2, 0);
                    imgTargetSec.TranslateY = Boundary(imgTargetSec.TranslateY + e.Delta.Translation.Y, height / 2, scaleY);
                }
                catch (Exception ex)
                {
                    MessageDialog msgDailog = new MessageDialog(ex.Message);
                    msgDailog.ShowAsync();
                }

            }
        }



        /// <summary>
        /// Function to set boundary for transform
        /// </summary>
        /// <param name="value">Transform cordinate value </param>
        /// <param name="max">Max Transform</param>
        /// <param name="min">Min Transform</param>
        /// <returns></returns>
        double Boundary(double value, double max, double min)
        {
            if (value > max) return max;
            else if (value < min) return min;
            else return value;
        }

        /// <summary>
        /// Select the background image on editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void selectFileClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var getButtonValue = selectCake.GetValue(AutomationProperties.NameProperty);
                FileOpenPicker picker = new FileOpenPicker();
                picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                picker.FileTypeFilter.Add(".png");
                picker.FileTypeFilter.Add(".jpg");
                sourceFile = await picker.PickSingleFileAsync();
                filename = sourceFile.Name;
              
                if (sourceFile != null)
                {
                    var image = new BitmapImage();
                    using (var stream = await sourceFile.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        await image.SetSourceAsync(stream);
                    }
                    if (getButtonValue.ToString().ToLower() == "select file")
                    {                        
                        target.Source = image;                                  
                       // target.Visibility = Visibility.Visible;
                        selectCake.SetValue(AutomationProperties.NameProperty, "select 2 file");
                    }
                    else
                    {
                        targetSecond.Source = image;
                        targetSecond.Height = 200;
                        targetSecond.Width = 200;
                        isTopImageExist = true;
                    }
                }
                var name = target.Name;
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
            }
        }

        /// <summary>
        /// This function is call when we click on save button to save edited image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void saveClick(object sender, RoutedEventArgs e)
        {
            if (TextTransform.Text == "") { TextTransform.Visibility = Visibility.Collapsed; }
            if (isTopImageExist == false) { targetSecond.Visibility = Windows.UI.Xaml.Visibility.Collapsed; }
            var objScreenShot = new ScreenShot();
            try
            {
                bool ellipse = false;
                var bitmap = await objScreenShot.SaveScreenshotAsync(root, filename, ellipse);
                if (bitmap != null)
                {
                    this.Frame.Navigate(typeof(CustomerInformation));
                }
                else
                {
                    this.Frame.Navigate(typeof(Editor));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
           
        }
        /// <summary>
        /// This event crop the image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void cropClick(object sender, RoutedEventArgs e)
        {
            storeCropClick = "clicked";
            target.Clip = new RectangleGeometry() { Rect = new Rect(selectedLeft, selectedTop, selectedWidth, selectedHeight) };
            try
            {
                
                elipsimage.Height = selectedHeight;
                elipsimage.Width = selectedWidth;
            }
            catch (Exception ex) { }
            selectCake.SetValue(AutomationProperties.NameProperty, "select 2 file");
            marking.Visibility = Visibility.Collapsed;
            updateMarking();
            fitTargetBack();
            //Canvas.SetTop(grdImageContainer, selectedTop * -1);
            //Canvas.SetLeft(grdImageContainer, selectedLeft * -1);
            btnCrop.Visibility = Visibility.Collapsed;
            var objScreenShot = new ScreenShot();
            try
            {
                // Logic to crop and display image in circular shape
                bool ellipse = true;
                var bitmap = await objScreenShot.SaveScreenshotAsync(root, filename, ellipse);
                targetCircular.ImageSource = bitmap;               
                target.Visibility = Visibility.Collapsed;
                clippedImgHeight = selectedHeight;
                clippedImgWidth = selectedWidth;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// This function clear the changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetClick(object sender, RoutedEventArgs e)
        {
           this.Frame.Navigate(typeof(Editor));
        }
        /// <summary>
        /// Call when we do marking on screen(In case of cropping)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartMarking(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint pointerLocation = e.GetCurrentPoint(sender as UIElement);
            leftStart = pointerLocation.Position.X;
            topStart = pointerLocation.Position.Y;
            updateMarking();
        }
        /// <summary>
        ///This event is call when marking is update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateMarking(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.IsInContact)
            {
                PointerPoint pointerLocation = e.GetCurrentPoint(sender as UIElement);

                markingWidth = pointerLocation.Position.X - leftStart;
                markingHeight = pointerLocation.Position.Y - topStart;
                if (pointerLocation.Position.X <= target.ActualWidth)
                {
                    updateMarking();
                }
            }
        }

        /// <summary>
        /// Event fire when we stop marking on cavas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopMarking(object sender, PointerRoutedEventArgs e)
        {
            selectedTop = Canvas.GetTop(marking);
            selectedLeft = Canvas.GetLeft(marking);
            selectedHeight = marking.Height;
            selectedWidth = marking.Width;
            markingWidth = markingHeight = topStart = leftStart = 0;
        }

        /// <summary>
        /// Event fire for updating the marking
        /// </summary>
        private void updateMarking()
        {
            if (markingHeight >= 0)
            {
                Canvas.SetTop(marking, topStart);
                marking.Height = markingHeight;
            }
            else
            {
                Canvas.SetTop(marking, topStart + markingHeight);
                marking.Height = markingHeight * -1;
            }

            if (markingWidth >= 0)
            {
                Canvas.SetLeft(marking, leftStart);
                marking.Width = markingWidth;
            }
            else
            {
                Canvas.SetLeft(marking, leftStart + markingWidth);
                marking.Width = markingWidth * -1;
            }
        }
        /// <summary>
        /// This event set the image to Top left corner of canvas after cropping 
        /// </summary>
        private void fitTargetBack()
        {
            Canvas.SetTop(target, selectedTop * -1);
            Canvas.SetLeft(target, selectedLeft * -1);
        }
        /// <summary>
        /// Save the cropped changes
        /// </summary>
        /// <param name="destinationFile"></param>
        private async void cropAndSaveImage(StorageFile destinationFile)
        {
            if (destinationFile != null)
            {
                using (var readStream = await sourceFile.OpenReadAsync())
                {
                    var decoder = await BitmapDecoder.CreateAsync(readStream);

                    using (InMemoryRandomAccessStream writeStream = new InMemoryRandomAccessStream())
                    {
                        BitmapEncoder encoder = await BitmapEncoder.CreateForTranscodingAsync(writeStream, decoder);
                        encoder.BitmapTransform.Bounds = new BitmapBounds()
                        {
                            X = (uint)Math.Round(selectedLeft, 0),
                            Y = (uint)Math.Round(selectedTop, 0),
                            Width = (uint)Math.Round(selectedWidth, 0),
                            Height = (uint)Math.Round(selectedHeight, 0)
                        };
                        await encoder.FlushAsync();
                        using (var stream = await destinationFile.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            await RandomAccessStream.CopyAndCloseAsync(writeStream.GetInputStreamAt(0), stream.GetOutputStreamAt(0));
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Nevigation to Main page when click the BackButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        /// <summary>
        /// Close pop of Text Cutomization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; }
        }

        /// <summary>
        /// Show pop for offset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowPopupOffsetClicked(object sender, RoutedEventArgs e)
        {
            // open the Popup if it isn't open already
            if (!StandardPopup.IsOpen) { StandardPopup.IsOpen = true; }
        }
        /// <summary>
        /// This event show the pop to customize text effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowPopupTextEffects(Object sender, RoutedEventArgs e)
        {
            if (!TextEffects.IsOpen) { TextEffects.IsOpen = true; }
        }
        /// <summary>
        /// This event close the pop to customize text effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClosePopupTextEffectsClicked(object sender, RoutedEventArgs e)
        {
            if (TextEffects.IsOpen) { TextEffects.IsOpen = false; }
        }
        /// <summary>
        /// This event set the text in image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Transform_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextTransform.Visibility == Visibility.Visible)
            {
                TextTransform.Text = Transform.Text;
            }
            else
            {
                TextTransform.Text = Transform.Text;
                curve.Children.Clear();
                TextCurving();
            }
        }

        /// <summary>
        /// This event is not currently in use
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeFont_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
        /// <summary>
        /// Event Fire when we makes change on font
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeFont_SelectionChanges(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (sender as ComboBox).SelectedItem as ComboBoxItem;
            string font = comboBoxItem.Content.ToString();
            curveTextFont = font;
            if (TextTransform.Visibility == Visibility.Visible)
            {
                TextTransform.FontFamily = new Windows.UI.Xaml.Media.FontFamily(font);
            }
            else
            {
                curve.Children.Clear();
                TextCurving();
            }
        }
        /// <summary>
        /// This functions changes the color of font
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (sender as ComboBox).SelectedItem as ComboBoxItem;
            string color = comboBoxItem.Content.ToString();
            curveTextColor = color;
            SolidColorBrush scb = new SolidColorBrush();
            switch (color)
            {
                case "Aqua": scb.Color = Windows.UI.Color.FromArgb(255, 0, 255, 255);
                    break;
                case "Black": scb.Color = Windows.UI.Color.FromArgb(255, 0, 0, 0);
                    break;
                case "Blue": scb.Color = Windows.UI.Color.FromArgb(255, 0, 0, 255);
                    break;
                case "Fuchsia": scb.Color = Windows.UI.Color.FromArgb(255, 255, 0, 255);
                    break;
                case "Green": scb.Color = Windows.UI.Color.FromArgb(255, 0, 128, 0);
                    break;
                case "Gray": scb.Color = Windows.UI.Color.FromArgb(255, 128, 128, 128);
                    break;
                case "Lime": scb.Color = Windows.UI.Color.FromArgb(255, 0, 255, 0);
                    break;
                case "Maroon": scb.Color = Windows.UI.Color.FromArgb(255, 128, 0, 0);
                    break;
                case "Navy": scb.Color = Windows.UI.Color.FromArgb(255, 0, 0, 128);
                    break;
                case "Olive": scb.Color = Windows.UI.Color.FromArgb(255, 128, 128, 0);
                    break;
                case "Purple": scb.Color = Windows.UI.Color.FromArgb(255, 128, 0, 128);
                    break;
                case "Red": scb.Color = Windows.UI.Color.FromArgb(255, 255, 0, 0);
                    break;
                case "Silver": scb.Color = Windows.UI.Color.FromArgb(255, 192, 192, 192);
                    break;
                case "Teal": scb.Color = Windows.UI.Color.FromArgb(255, 0, 128, 128);
                    break;
                case "White": scb.Color = Windows.UI.Color.FromArgb(255, 255, 255, 255);
                    break;
                case "Yellow": scb.Color = Windows.UI.Color.FromArgb(255, 255, 255, 0);
                    break;
            }
            if (TextTransform.Visibility == Visibility.Visible)
            {
                TextTransform.Foreground = scb;
            }
            else
            {
                curve.Children.Clear();
                TextCurving();
            }
        }

        /// <summary>
        /// This Event changes the font size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextSize_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (TextTransform.Visibility == Visibility.Visible)
            {
                TextTransform.FontSize = 10 + e.NewValue;
            }
            else
            {
                curve.Children.Clear();
                TextCurving();
            }
            curveTextSize = 10 + e.NewValue;
        }

        /// <summary>
        /// This event use for text Curving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            checkCurveSlider="";
            TextTransform.Visibility = Visibility.Collapsed;
            var oldValue = e.OldValue;
            var newValue = e.NewValue;
            var val = TextTransform.Text;
            var length = val.Length;
            var diff = newValue - oldValue;
            if (diff < 0)
            {
                curve.Children.Clear();
                sliderSpacing = Convert.ToInt32(oldValue);
                TextCurving();
            }
            else
            {
                curve.Children.Clear();
                sliderSpacing = Convert.ToInt32(newValue);
                TextCurving();
            }
        }

        /// <summary>
        /// For text curving
        /// </summary>
        public void TextCurving()
        {
            //For text coloring
            SolidColorBrush scb = new SolidColorBrush();
            scb.Color = Windows.UI.Color.FromArgb(255, 255, 255, 255);
            switch (curveTextColor)
            {
                case "Aqua": scb.Color = Windows.UI.Color.FromArgb(255, 0, 255, 255);
                    break;
                case "Black": scb.Color = Windows.UI.Color.FromArgb(255, 0, 0, 0);
                    break;
                case "Blue": scb.Color = Windows.UI.Color.FromArgb(255, 0, 0, 255);
                    break;
                case "Fuchsia": scb.Color = Windows.UI.Color.FromArgb(255, 255, 0, 255);
                    break;
                case "Green": scb.Color = Windows.UI.Color.FromArgb(255, 0, 128, 0);
                    break;
                case "Gray": scb.Color = Windows.UI.Color.FromArgb(255, 128, 128, 128);
                    break;
                case "Lime": scb.Color = Windows.UI.Color.FromArgb(255, 0, 255, 0);
                    break;
                case "Maroon": scb.Color = Windows.UI.Color.FromArgb(255, 128, 0, 0);
                    break;
                case "Navy": scb.Color = Windows.UI.Color.FromArgb(255, 0, 0, 128);
                    break;
                case "Olive": scb.Color = Windows.UI.Color.FromArgb(255, 128, 128, 0);
                    break;
                case "Purple": scb.Color = Windows.UI.Color.FromArgb(255, 128, 0, 128);
                    break;
                case "Red": scb.Color = Windows.UI.Color.FromArgb(255, 255, 0, 0);
                    break;
                case "Silver": scb.Color = Windows.UI.Color.FromArgb(255, 192, 192, 192);
                    break;
                case "Teal": scb.Color = Windows.UI.Color.FromArgb(255, 0, 128, 128);
                    break;
                case "White": scb.Color = Windows.UI.Color.FromArgb(255, 255, 255, 255);
                    break;
                case "Yellow": scb.Color = Windows.UI.Color.FromArgb(255, 255, 255, 0);
                    break;
            }

            //Get complete text which is going to curve
            string textToCurve = TextTransform.Text;

            //Loop till text length
            for (var i = 0; i <= textToCurve.Length - 1; i++)
            {
                if (checkCurveSlider == "")
                {
                    if (i < 2)
                    {
                        sliderSpacing = sliderSpacing + 5;
                    }
                    if (i < 3 && i > 1)
                    {
                        sliderSpacing = sliderSpacing + 1;
                    }
                }
                //x = (radius * 2.5) * Math.Cos(degrees2radians(i * sliderSpacing)) + 20;
                //y = (radius) * Math.Sin(degrees2radians(i * sliderSpacing)) + 90; 
                x = (radius * newSliderRadius) * Math.Cos(degrees2radians(i * sliderSpacing)) + 20;
                y = (radius) * Math.Sin(degrees2radians(i * sliderSpacing)) + 90;

                //Create new textblock
                TextBlock textBlock = new TextBlock();

                textBlock.Text = textToCurve[i].ToString();
                textBlock.FontSize = curveTextSize;
                textBlock.Margin = new Thickness { Left = 400, Right = 0, Top = 235, Bottom = 0 };
                textBlock.Foreground = scb;
                if (curveTextFont != null)
                {
                    textBlock.FontFamily = new Windows.UI.Xaml.Media.FontFamily(curveTextFont);
                }
                //Set canvas with x and y coordinates
                Canvas.SetLeft(textBlock, -x);
                Canvas.SetTop(textBlock, -y);
                Canvas.SetZIndex(textBlock, 50);
                curve.Children.Add(textBlock);

            }

        }

        /// <summary>
        /// Function which returns angle
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        private double degrees2radians(double deg)
        {
            return (Math.PI * deg) / 180;
        }

        /// <summary>
        /// Used to scale curved text from x axis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScaleX_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var oldX = e.OldValue;
            newX = e.NewValue;
            curve.Margin = new Thickness { Left = newX, Right = 0, Top = newY, Bottom = 0 };
        }

        /// <summary>
        /// Used to scale curved text from y axis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScaleY_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var oldY = e.OldValue;
            newY = e.NewValue;
            curve.Margin = new Thickness { Left = newX, Right = 0, Top = newY, Bottom = 0 };
        }

        /// <summary>
        /// Used to scale(width) on curved text 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScaleSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            checkCurveSlider = "true";
            if (TextTransform.Visibility != Visibility.Visible)
            {
                if (e.NewValue > 0)
                {
                    newSliderRadius = e.NewValue / 2;
                    curve.Children.Clear();
                    TextCurving();
                }
            }
        }


        /// <summary>
         //This is currnently not in use
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextTransform_DragEnter(object sender, DragEventArgs e)
        {
            var x = e.Data;
        }

        /// <summary>
        /// Open the pop for scaling the top image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowPopupScaleEffects(object sender, RoutedEventArgs e)
        {
           if (!ImageScale.IsOpen) { ImageScale.IsOpen = true; }
        }


        /// <summary>
        /// Open the popup for text curving 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowPopupCurveEffects(object sender, RoutedEventArgs e)
        {
            if (!curvePopup.IsOpen) { curvePopup.IsOpen = true; }
        }



        /// <summary>
        /// Close the pop for scaling the top image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClosePopupScaleeffectsClicked(object sender, RoutedEventArgs e)
        {
            ImageScale.IsOpen = false;
        }


        /// <summary>
        /// Close the pop for curving the text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClosePopupCurveeffectsClicked(object sender, RoutedEventArgs e)
        {
            curvePopup.IsOpen = false;
        }

        /// <summary>
        /// This Event scale the image acccording to X axis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Scale_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var imgTargetSec = (CompositeTransform)targetSecond.RenderTransform;
            var newValue = e.NewValue;
            var oldvalue = e.OldValue;
            var scaleFactor = .4;
            var diff = newValue - oldvalue;
            if (diff < 0)
            {
                diff = -(diff);         // Make the positive number
            }
            if (diff > 0 && diff < 31)
            {
                scaleFactor = .1;
            }
            // conditions for scale factor
            if (diff > 30 && diff < 61)
            {
                scaleFactor = .3;
            }
            if (diff > 60 && diff < 91)
            {
                scaleFactor = .4;
            }
            if (diff > 90 && diff < 121)
            {
                scaleFactor = .5;
            }
            if (diff > 120 && diff < 151)
            {
                scaleFactor = .6;
            }
            if (diff > 150 && diff < 181)
            {
                scaleFactor = .7;
            }
            if (diff > 180 && diff < 281)
            {
                scaleFactor = .8;
            }
            if (diff > 280)
            {
                scaleFactor = .9;
            }

            // Condition for increase or decrease scaling 
            if (newValue > oldvalue)
            {
                imgTargetSec.ScaleX += scaleFactor;  
            }
            else
            {
                imgTargetSec.ScaleX -= scaleFactor;
            }
        }
        /// <summary>
        /// This Event scale the image on Y axis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slider_ValueChanged_1(object sender, RangeBaseValueChangedEventArgs e)
        {
            var imgTargetSec = (CompositeTransform)targetSecond.RenderTransform;
            var newValue = e.NewValue;
            var oldvalue = e.OldValue;
            var scaleFactor = .4;                              //Taken as default, it changes according to condition
            var diff = newValue - oldvalue;
            if (diff < 0)
            {
                diff = -(diff);                               // To make a poitive value if differece is negative
            }
            if (diff > 0 && diff < 31)
            {
                scaleFactor = .1;
            }
            // conditions to change the scale factor
            if (diff > 30 && diff < 61)
            {
                scaleFactor = .3;
            }
            if (diff > 60 && diff < 91)
            {
                scaleFactor = .4;
            }
            if (diff > 90 && diff < 121)
            {
                scaleFactor = .5;
            }
            if (diff > 120 && diff < 151)
            {
                scaleFactor = .6;
            }
            if (diff > 150 && diff < 181)
            {
                scaleFactor = .7;
            }
            if (diff > 180 && diff < 281)
            {
                scaleFactor = .8;
            }
            if (diff > 280)
            {
                scaleFactor = .9;
            }
            // Condiiton for increase or decraese the scaling 
            if (newValue > oldvalue)
            {
                imgTargetSec.ScaleY += scaleFactor;
            }
            else
            {
                imgTargetSec.ScaleY -= scaleFactor;
            }
        }
    
        /// <summary>
        /// This event saves the canvas screen(edited image) and then move to print form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnMoveTOPrint_Click(object sender, RoutedEventArgs e)
        {
            var objScreenShot = new ScreenShot();
            if (TextTransform.Text == "") { TextTransform.Visibility = Visibility.Collapsed; }
            if (isTopImageExist == false) { targetSecond.Visibility = Windows.UI.Xaml.Visibility.Collapsed; }
            try
            {
                bool ellipse = true;     //  Condition to save captured canvas(edited image) in application folder
                var bitmap = await objScreenShot.SaveScreenshotAsync(root, filename, ellipse);
                this.Frame.Navigate(typeof(ImageEditorNew));
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// used to drag second image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scrollViewer_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            double height = root.ActualHeight;
            double width = root.ActualWidth;
            try
            {
                var imgTargetSec = (CompositeTransform)scrollViewer.RenderTransform;
                var scaleY = -(imgTargetSec.ScaleY * 28);    //Set the Y axis boudary to move up
                if (screenTest == "Large")
                {
                    imgTargetSec.TranslateX = Boundary(imgTargetSec.TranslateX + e.Delta.Translation.X, 1000, 0);
                    imgTargetSec.TranslateY = Boundary(imgTargetSec.TranslateY + e.Delta.Translation.Y, 500, scaleY);
                }
                else
                {
                    imgTargetSec.TranslateX = Boundary(imgTargetSec.TranslateX + e.Delta.Translation.X, 800, 0);
                    imgTargetSec.TranslateY = Boundary(imgTargetSec.TranslateY + e.Delta.Translation.Y, 400, scaleY);
                }
                var imgTarget2 = imgTargetSec.TranslateX;
                if (imgTarget2 > 850.0)
                {
                    var imgTargetTest = "test";
                }

            }
            catch (Exception ex)
            {
                MessageDialog msgDailog = new MessageDialog(ex.Message);
                msgDailog.ShowAsync();
            }
        }


        /// <summary>
        /// Used to change view of second image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scrollViewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            if (Output1.Height != scrollViewer.ExtentHeight)
            {
                Output1.Height = scrollViewer.ExtentHeight;
                Output1.Width = scrollViewer.ExtentWidth;
            }
        }
                
    }
}
