using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LottoryUWP.FTUE
{
    public sealed partial class AnimatedImage : UserControl
    {
        public AnimatedImage()
        {
            this.InitializeComponent();
            this.Tapped += AnimatedImage_Tapped;
        }

        private void AnimatedImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.ImageEnd != null)
            {
                VisualStateManager.GoToState(this, "Normal", true);

                ActiveAnimation();
            }
        }

        public async void ActiveAnimation()
        {
            if (this.ImageEnd!=null)
            {
                await Task.Delay(3000);
                VisualStateManager.GoToState(this, "Running", true);
            }
        }

      

        public ImageSource ImageStart
        {
            get { return (ImageSource)GetValue(ImageStartProperty); }
            set { SetValue(ImageStartProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageStart.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageStartProperty =
            DependencyProperty.Register("ImageStart", typeof(ImageSource), typeof(AnimatedImage), new PropertyMetadata(null));




        public ImageSource ImageEnd
        {
            get { return (ImageSource)GetValue(ImageEndProperty); }
            set { SetValue(ImageEndProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageEnd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageEndProperty =
            DependencyProperty.Register("ImageEnd", typeof(ImageSource), typeof(AnimatedImage), new PropertyMetadata(null));


    }
}
