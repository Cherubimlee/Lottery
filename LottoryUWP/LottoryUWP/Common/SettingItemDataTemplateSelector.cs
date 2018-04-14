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
        public DataTemplate StyleSettingTemplate { get; set; }
        public DataTemplate GeneralSettingTemplate { get; set; }
       
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {

            if (item is SettingItem settingItem)
             switch(settingItem.StyleType)
                {
                    case Enum.SettingType.DisplayStyleSetting:
                        return StyleSettingTemplate;
                    case Enum.SettingType.General:
                    default:
                        return GeneralSettingTemplate;
                }
            else
                return GeneralSettingTemplate;
            
        }
    }
}
