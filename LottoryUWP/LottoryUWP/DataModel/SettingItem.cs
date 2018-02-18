using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoryUWP.DataModel
{
    public class SettingItem
    {

        public string UniqueId { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    public class WinnerStyleSettingItem : SettingItem
    {
        public DrawItem SampleWinner { get; set; } = new DrawItem() { MajorColumnValue = "Major", SecondaryColumnValue = "Secondary" };

    }

}
