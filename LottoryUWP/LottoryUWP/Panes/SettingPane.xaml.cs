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

namespace LottoryUWP.Panes
{
    public sealed partial class SettingPane : UserControl
    {
        public SettingPane()
        {
            this.InitializeComponent();
            Groups = DataModel.SettingData.BuildSettingList();

            if (SettingData.Instance.IsAdFree)
            {
                adControl.Visibility = Visibility.Collapsed;
            }
        }
      
        public IEnumerable<SettingItemGroup> Groups
        {
            get;
            set;
        }

        private void List_GotFocus(object sender, RoutedEventArgs e)
        {
            ZoomControl.StartBringIntoView();
        }


        private void AdControl_ErrorOccurred(object sender, Microsoft.Advertising.WinRT.UI.AdErrorEventArgs e)
        {
            if(e.ErrorCode == Microsoft.Advertising.ErrorCode.NoAdAvailable)
            {
                adControl.Visibility = Visibility.Collapsed;
            }
            else
            {
                adControl.Resume();
            }
        }
    }
}
