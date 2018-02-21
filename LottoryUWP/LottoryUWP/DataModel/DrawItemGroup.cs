using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoryUWP.DataModel
{
    public class DrawItemGroup
    {
        public ObservableCollection<DrawItem> Items { get; set; } = new ObservableCollection<DrawItem>();

        public int? GroupCapacity { get; set; }

        public string GroupTitle { get; set; }

        public int groupIndex { get; set; }
    }
}
