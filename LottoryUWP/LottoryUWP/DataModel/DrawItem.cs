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
    }
}
