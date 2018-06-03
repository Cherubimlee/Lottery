using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoryUWP.DataModel
{
    public class DataSourceModel
    {
        public string FilePath { get; set; }

        public int Start { get; set; }

        public int End { get; set; }

        public string Prefix { get; set; } = string.Empty;

        public bool IsNumberSource { get { return string.IsNullOrEmpty(this.FilePath); } }

        public List<DrawItem> GenerateDataItems()
        {
            List<DrawItem> dataSource = new List<DrawItem>();

            if (this.IsNumberSource)
            {
               
                string format = Prefix + (End < 10 ? "{0:0}" : End < 100 ? "{0:00}" : "{0:000}");

                for (int i = Start; i <= End; i++)
                    dataSource.Add(new DrawItem() { MajorColumnValue = String.Format(format, i) });

            }

            return dataSource;
        }
    }
}
