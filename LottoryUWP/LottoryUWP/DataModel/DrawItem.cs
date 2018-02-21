using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoryUWP.DataModel
{
    public class DrawItem
    {
       public string MajorColumnValue { get; set; }
       public string SecondaryColumnValue { get; set; }
       public int DrawWeight { get; set; } = 1;
      
   
       public string DisplayName
        {
            get
            {
                return string.Format("{0} {1}", MajorColumnValue, SecondaryColumnValue);
            }
        }

        public WinnerItem ToWinnerItem(int histroyGroupIndex)
        {
            return new WinnerItem(this, histroyGroupIndex);
        }
    }

    public class WinnerItem : DrawItem
    {
        public DateTime? DrawnTimeStamp { get; set; }
        public int HistroyGroupIndex { get; set; }
        public WinnerItem(DrawItem item, int historyGroupIndex)
        {
            this.MajorColumnValue = item.MajorColumnValue;
            this.SecondaryColumnValue = item.SecondaryColumnValue;
            this.DrawWeight = item.DrawWeight;
            this.DrawnTimeStamp = DateTime.Now;
            this.HistroyGroupIndex = historyGroupIndex;
        }
    }
}
