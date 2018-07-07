using LottoryUWP.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LottoryUWP.FTUE
{
    public sealed partial class FTUEDialog : ContentDialog
    {
        public FTUEDialog()
        {
            this.InitializeComponent();

            Items = new ObservableCollection<FTUEItem>();

            this.Title = "What's New";

            Items.Add(new FTUEItem() { Description = "Hello World", ImagePath = @"ms-appx:///Assets/Img/Img1.jpg" });

            Items.Add(new FTUEItem() { Description = "Hello World", ImagePath= @"ms-appx:///Assets/Img/Img1.jpg", ExtImagePath= @"ms-appx:///Assets/Img/Img2.jpg" });

            Items.Add(new FTUEItem() { Description = "Hello World", ImagePath = @"ms-appx:///Assets/Img/Img2.jpg", ExtImagePath = @"ms-appx:///Assets/Img/Img3.jpg" });

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        public ObservableCollection<FTUEItem> Items { get; set; }

        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FlipView flip = sender as FlipView;

            var key = flip?.SelectedItem as FTUEItem;

            var animatedImage = FTUEkeyValuePairs.FirstOrDefault((x)=>x.Key == key).Value;

            animatedImage?.ActiveAnimation();

            if (flip.Items.Count == flip.SelectedIndex + 1)
                DoneButton.IsEnabled = true;
        }

        private Dictionary<FTUEItem, AnimatedImage> FTUEkeyValuePairs = new Dictionary<FTUEItem, AnimatedImage>();

        private void AnimatedImage_Loaded(object sender, RoutedEventArgs e)
        {
            AnimatedImage animatedImage = sender as AnimatedImage;

            if(animatedImage != null)
            {
               var key = animatedImage.Tag as FTUEItem;
                    
               if(key!=null)
                FTUEkeyValuePairs[key] = animatedImage;

                if (Items.FirstOrDefault() == key)
                    animatedImage.ActiveAnimation();
            }
        }

       }
}
