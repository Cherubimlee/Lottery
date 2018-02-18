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
            Groups = getSettingItems();
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

        private IEnumerable<SettingItemGroup> getSettingItems()
        {
            List<SettingItemGroup> group = new List<SettingItemGroup>();

            List<SettingItem> items = new List<SettingItem>();
            items.Add(new WinnerStyleSettingItem() { Title = "Winner Style" });
           

            group.Add(new SettingItemGroup()
            {
                Title = "Winner Display Style",
                Items = new System.Collections.ObjectModel.ObservableCollection<SettingItem>(items)
            });

         

            return group;
        }
    }
}
