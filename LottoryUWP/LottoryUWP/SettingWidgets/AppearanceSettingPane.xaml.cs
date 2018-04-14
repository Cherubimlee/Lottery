using LottoryUWP.Common;
using LottoryUWP.DataModel;
using LottoryUWP.Utils;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LottoryUWP.SettingWidgets
{
    public sealed partial class AppearanceSettingPane : UserControl
    {
     
        public AppearanceSettingPane()
        {
            this.InitializeComponent();      
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Color color = ColorPicker.SelectedItem as Color;

            if(color != null)
            {
                SettingData.Instance.WinnerColor = color.ColorObj;
                this.SampleWinner.ControlColor = color.ColorObj;
            }

        }

        private void RandomColorToggle_Toggled(object sender, RoutedEventArgs e)
        {
            if(RandomColorToggle.IsOn)
                this.SampleWinner.ControlColor = RandomUtil.Instance.RandomColor();
        }

        private void SampleWinner_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(SettingData.Instance.IsWinnerColorRandom)
                this.SampleWinner.ControlColor = RandomUtil.Instance.RandomColor();
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Brush brush = e.ClickedItem as Brush;

            var Brushes = SettingData.Instance.BackgroundBrushes;

            var index = Brushes.IndexOf(brush);

            if (index == 0) return;

            if(index == Brushes.Count - 1)
            {
           
            }
            else
            {
                SettingData.Instance.SelectBackgroundColor(brush);
            }
          
        }
    }
}
