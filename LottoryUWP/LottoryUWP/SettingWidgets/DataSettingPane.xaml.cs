using LottoryUWP.DataModel;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LottoryUWP.SettingWidgets
{
    public sealed partial class DataSettingPane : UserControl
    {
        public DataSettingPane()
        {
            this.InitializeComponent();

            this.SetDataModel();
        }

        private void RangeSelectorControl_ValueChanged(object sender, Microsoft.Toolkit.Uwp.UI.Controls.RangeChangedEventArgs e)
        {
            if(e.ChangedRangeProperty == Microsoft.Toolkit.Uwp.UI.Controls.RangeSelectorProperty.MaximumValue)
            {
                if (RangeSelectorControl.RangeMax - RangeSelectorControl.RangeMin > 200)
                    RangeSelectorControl.RangeMin = RangeSelectorControl.RangeMax - 200 + 1;
            }

            if (e.ChangedRangeProperty == Microsoft.Toolkit.Uwp.UI.Controls.RangeSelectorProperty.MinimumValue)
            {
                if (RangeSelectorControl.RangeMax - RangeSelectorControl.RangeMin > 200)
                    RangeSelectorControl.RangeMax = RangeSelectorControl.RangeMin + 200 - 1;
            }

            ApplyDataSource();
        }

        private void PrefixLabel_LostFocus(object sender, RoutedEventArgs e)
        {
            this.ApplyDataSource();
        }

        private async void ApplyDataSource()
        {

            ContentDialog dialog = new ContentDialog();
            dialog.Title = "Warning!";
            dialog.Content = "The changes will reload data source and clear all current draw history, do you want to continue?";
            dialog.PrimaryButtonText = "Contiune";
            dialog.SecondaryButtonText = "No";
            var r = await dialog.ShowAsync();

            if (r == ContentDialogResult.Primary)
            {
                if (this.NumberSource.IsChecked.HasValue && this.NumberSource.IsChecked.Value)
                {
                    DataSourceModel model = new DataSourceModel()
                    {
                        Start = (int)RangeSelectorControl.RangeMin,
                        End = (int)RangeSelectorControl.RangeMax,
                        Prefix = this.PrefixLabel.Text
                    };

                    SettingData.Instance.DrawDataSource = model;
                }

            }
            else
            {
                this.SetDataModel();
            }
        }

        private void SetDataModel()
        {
            var model = SettingData.Instance.DrawDataSource;

            if (model.IsNumberSource)
            {
                this.NumberSource.IsChecked = true;
                this.RangeSelectorControl.RangeMin = model.Start;
                this.RangeSelectorControl.RangeMax = model.End;
                this.PrefixLabel.Text = model.Prefix;
            }
        }
    }
}
