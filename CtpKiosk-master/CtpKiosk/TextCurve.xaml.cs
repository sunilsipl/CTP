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
    public sealed partial class TextCurve : Page
    {
        double oldValue;
        double newValue;
        int radius = 150;
        int sliderSpacing;
        int spacing = 11;
        double x;
        double y;

        Point center = new Point(125, 125);

        public TextCurve()
        {
            this.InitializeComponent();           
          
        }

        /// <summary>
        /// Formula for calculating angle
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        private double degrees2radians(int deg)
        {
           return (Math.PI * deg) / 180;            
        }



        /// <summary>
        /// For text curving
        /// </summary>
        public void TextCurving()
        {            
           // int length =Convert.ToInt32(txtCurveText.Text);
            string length = txtCurveText.Text;

            for (var i = 0; i <= length.Length-1; i++)
           {               
               if (sliderSpacing != 0)
               {
                    x = (radius * 2.5) * Math.Cos(degrees2radians(i * sliderSpacing)) + 20;
                    y = (radius) * Math.Sin(degrees2radians(i * sliderSpacing)) + 90;
               }
               else
               {
                    x = (radius * 2.5) * Math.Cos(degrees2radians(i * spacing)) + 100;
                    y = (radius * 1.5) * Math.Sin(degrees2radians(i * spacing)) + 90;
               }
                //Create textblock and add text into it
                TextBlock textBlock = new TextBlock();
              
                textBlock.Text =length[i].ToString();        
                textBlock.FontSize = 30;
                textBlock.Margin = new Thickness {Left=400,Right=0,Top=300,Bottom=0 };
                textBlock.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 192, 192, 192));

               //Set canvas by x and y coordinates
                Canvas.SetLeft(textBlock, -x);
                Canvas.SetTop(textBlock, -y);
                Canvas.SetZIndex(textBlock, 50);
               // tbTransformText.Text.Contains(textBlock.ToString());
                curve.Children.Add(textBlock);
             }
        
        }

        /// <summary>
        /// Call on button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sliderSpacing = 0;
            TextCurving();
            //curve.Children.Clear();
        }

        /// <summary>
        /// Call on changed of text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCurveText_TextChanged(object sender, TextChangedEventArgs e)
        {
            //TextCurving();    
           curve.Children.Clear();
        }


        /// <summary>
        /// Slider implementation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {            
             oldValue = e.OldValue;
             newValue = e.NewValue;             
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
             
            // curve.Children.Clear();
            // TextBlockCurving();
        }

        /// <summary>
        /// Textblock curving by slider  // Not used now
        /// </summary>
        public void TextBlockCurving()
        {
            double x = oldValue;
            double y = newValue;
            double xFactor = oldValue+10;
            double yFactor = newValue+13;
            // int length =Convert.ToInt32(txtCurveText.Text);
            string length = tbTransformText.Text;

            for (var i = 0; i <= length.Length - 1; i++)
            {
                var isEven = length.Length % 2;
                int halfLength = length.Length / 2;

                if (isEven == 1) { halfLength = halfLength + 1; }

                if (i <= halfLength)
                {
                    x = x + xFactor;
                    y = y - yFactor;
                }

                else
                {

                    // if (i == 4 || i == 5) { x = x + xFactor; }
                    // else
                    {
                        x = x + xFactor;
                        y = y + yFactor;
                    }
                }

                TextBlock textBlock = new TextBlock();

                //tbTransformText.Text = "";
                textBlock.Text = length[i].ToString();
               // tbTransformText.Text = length[i].ToString();
                textBlock.FontSize = 30;
                textBlock.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 192, 192, 192));

                //textBlock.Foreground = Foreground.
                Canvas.SetLeft(textBlock, x);
                Canvas.SetTop(textBlock, y);
                curve.Children.Add(textBlock);
                
            }

        }

       
    }
}
