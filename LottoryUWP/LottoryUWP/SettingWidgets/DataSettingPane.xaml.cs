using LottoryUWP.DataModel;
using LottoryUWP.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
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

            this.FileSource.Checked += this.Radio_Checked;
            this.NumberSource.Checked += this.Radio_Checked;
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

            bool canProcess = DrawData.Instance.DrawHistory.Count == 0;

            if (!canProcess)
            {
                ContentDialog dialog = new ContentDialog();
                dialog.Title = Strings.Resources.DataSetting_Dialog_Warning;
                dialog.Content = Strings.Resources.DataSetting_Dialog_Reload;
                dialog.PrimaryButtonText = Strings.Resources.DataSetting_Dialog_Contiune;
                dialog.SecondaryButtonText = Strings.Resources.DataSetting_Dialog_No;
                canProcess = await dialog.ShowAsync() == ContentDialogResult.Primary;
            }

            if (canProcess)
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
                else
                {
                    DataSourceModel model = new DataSourceModel()
                    {
                        FilePath = this.FilePathText.Text,
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
            else
            {
                this.FilePathText.Text = model.FilePath;
                this.FileSource.IsChecked = true;
            }
        }

        private async void FileButton_Click(object sender, RoutedEventArgs e)
        {
         
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".csv");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                StorageFolder folder = ApplicationData.Current.TemporaryFolder;

                var copiedFile = await file.CopyAsync(folder, file.Name, NameCollisionOption.ReplaceExisting);

                var r = await DataSourceModel.ReadFileForDrawItem(copiedFile);

                if (r != null)
                {
                    if (r.ItemList.Count > 200 || r.ItemList.Count == 0)
                    {
                        ContentDialog dialog = new ContentDialog();
                        dialog.Title = Strings.Resources.DataSetting_Dialog_Error;
                        dialog.Content = Strings.Resources.DataSetting_Dialog_Exceed;

                        dialog.PrimaryButtonText = Strings.Resources.DataSetting_Dialog_Ok;
                        await dialog.ShowAsync();
                    }
                    else
                    {
                        this.FilePathText.Text = copiedFile.Name;
                        ApplyDataSource();
                        return;
                    }
                }
                else
                {
                    ContentDialog dialog = new ContentDialog();
                    dialog.Title = Strings.Resources.DataSetting_Dialog_Error;
                    dialog.Content = Strings.Resources.DataSetting_Dialog_FileError;

                    dialog.PrimaryButtonText = Strings.Resources.DataSetting_Dialog_Ok;
                    await dialog.ShowAsync();
                }

                SetDataModel();        
            }
          
        }

        private void Radio_Checked(object sender, RoutedEventArgs e)
        {
            if (sender == this.FileSource)
            {
                if (String.IsNullOrEmpty(FilePathText.Text))
                {
                    FileButton_Click(sender, e);
                    return;
                }
            }
            
            ApplyDataSource();
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFolder assets = await appInstalledFolder.GetFolderAsync("Assets");
            StorageFolder fileTemplate = await assets.GetFolderAsync("FileTemplate");

            var file = await fileTemplate.GetFileAsync("example.csv");

            FileSavePicker savePicker = new FileSavePicker();

            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
           
            savePicker.FileTypeChoices.Add(Strings.Resources.DataSetting_FileType, new List<string>() { ".csv" });

            savePicker.SuggestedFileName = "example";

            var newfile = await savePicker.PickSaveFileAsync();

            await file.CopyAndReplaceAsync(newfile);

            await Windows.System.Launcher.LaunchFileAsync(newfile);
        }
    }
}
