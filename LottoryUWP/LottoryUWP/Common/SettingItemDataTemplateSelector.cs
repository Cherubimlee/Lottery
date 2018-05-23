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
        public DataTemplate SupportSettingTemplate { get; set; }
        public DataTemplate DataSettingTemplate { get; set; }


        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {

            if (item is SettingItem settingItem)
             switch(settingItem.StyleType)
                {
                    case Enum.SettingType.DisplayStyleSetting:
                        return StyleSettingTemplate;
                    case Enum.SettingType.Support:
                        return SupportSettingTemplate;
                    case Enum.SettingType.Data:
                        return DataSettingTemplate;
                    default:
                        return GeneralSettingTemplate;
                }
            else
                return GeneralSettingTemplate;
            
        }
    }
}
