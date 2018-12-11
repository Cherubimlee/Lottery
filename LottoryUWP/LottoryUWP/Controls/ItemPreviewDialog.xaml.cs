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

namespace LottoryUWP.Controls
{
    public sealed partial class ItemPreviewDialog : ContentDialog
    {
     
        public ItemPreviewDialog()
        {
            this.InitializeComponent();

            this.Loaded += ItemPreviewDialog_Loaded;

            this.PrimaryButtonText = LottoryUWP.Strings.Resources.DataSetting_Dialog_Ok;

            ItemList = new ObservableCollection<String>();
        }

        private async void ItemPreviewDialog_Loaded(object sender, RoutedEventArgs e)
        {
          var items = (await SettingData.Instance.DrawDataSource.GenerateDataItems()).ItemList;
         
            foreach(var item in items)
            {
                ItemList.Add(item.DisplayName);
            }
        }


        ObservableCollection<String> ItemList { get; set; } 
    }
}
