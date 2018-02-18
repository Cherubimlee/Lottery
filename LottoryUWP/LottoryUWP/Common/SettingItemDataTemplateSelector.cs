using LottoryUWP.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LottoryUWP.Common
{
    class SettingItemDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate WinnerStyleTemplate { get; set; }
        public DataTemplate GeneralSettingTemplate { get; set; }
       
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {

            if (item is WinnerStyleSettingItem)
                return WinnerStyleTemplate;
            else
                return GeneralSettingTemplate;
            
        }
    }
}
