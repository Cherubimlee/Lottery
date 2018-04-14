using LottoryUWP.Common;
using LottoryUWP.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoryUWP.DataModel
{
    public class SettingItem
    {

        public string Title { get; set; }
        public string ImagePath { get; set; }

        public SettingData DataContext { get { return SettingData.Instance; } }

        public SettingType StyleType { get; set; }

        public IEnumerable<Color> ColorList {get {return Color.Colors;} }

        public override string ToString()
        {
            return this.Title;
        }
    }

}
