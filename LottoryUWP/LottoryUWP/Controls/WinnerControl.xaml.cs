using LottoryUWP.DataModel;
using LottoryUWP.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LottoryUWP.Controls
{
    public sealed partial class WinnerControl : UserControl
    {
        public WinnerControl()
        {
            InitializeComponent();

            var color = RandomUtil.Instance.RandomColor();
           
            this.borader.Background = new SolidColorBrush(Color.FromArgb(0x80, color.R, color.G, color.B));
            this.borader.BorderBrush = new SolidColorBrush(color);
        }

        public WinnerControl(DrawItem item) : this()
        {
            this.SecondaryValue = item.SecondaryColumnValue;
            this.MajorValue = item.MajorColumnValue;
        }

      

        public string MajorValue
        {
            get { return (string)GetValue(MajorValueProperty); }
            set { SetValue(MajorValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PrimayValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MajorValueProperty =
            DependencyProperty.Register("MajorValue", typeof(string), typeof(WinnerControl), new PropertyMetadata(string.Empty));




        public string SecondaryValue
        {
            get { return (string)GetValue(SecondaryValueProperty); }
            set { SetValue(SecondaryValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SecondaryValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondaryValueProperty =
            DependencyProperty.Register("SecondaryValue", typeof(string), typeof(WinnerControl), new PropertyMetadata(string.Empty));



    }
}
