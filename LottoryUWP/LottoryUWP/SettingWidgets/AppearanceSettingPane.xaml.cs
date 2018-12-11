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

            if (App.Current.RequestedTheme == ApplicationTheme.Light)
                LightRadioBtn.IsChecked = true;
            else
                DarkRadioBtn.IsChecked = true;

            this.Loaded += AppearanceSettingPane_Loaded;
        }

        private void AppearanceSettingPane_Loaded(object sender, RoutedEventArgs e)
        {
          ColorPicker.SelectedIndex = Color.IndexOfColors(SettingData.Instance.WinnerColor);
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
            BrushModel brush = e.ClickedItem as BrushModel;

            itemClick(brush);
        }

        private void RadioBtn_Checked(object sender, RoutedEventArgs e)
        {

            App app = App.Current as App;

            if (e.OriginalSource == LightRadioBtn)
                app.AppTheme = ApplicationTheme.Light;

            if (e.OriginalSource == DarkRadioBtn)
                app.AppTheme = ApplicationTheme.Dark;

            themeMsg.Visibility = (app.AppTheme != app.RequestedTheme) ? Visibility.Visible : Visibility.Collapsed;

        }

        private void ApplyAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Control uIElement = sender as Control;
            var brush = uIElement?.DataContext as BrushModel;

            itemClick(brush);

        }

        private void DeleteAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Control uIElement = sender as Control;
            var brush = uIElement?.DataContext as BrushModel;

            SettingData.Instance.RemoveBackgroundColor(brush);
        }

        private void itemClick(BrushModel brush)
        {
            var Brushes = SettingData.Instance.BackgroundBrushModels;

            var index = Brushes.IndexOf(brush);

            if (index == 0) return;


            SettingData.Instance.SelectBackgroundColor(brush);

        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {

            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".bmp");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                await SettingData.Instance.InsertBackgroundColor(file);
            }
        }
    }
}
