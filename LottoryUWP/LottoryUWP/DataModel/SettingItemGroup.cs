using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoryUWP.DataModel
{
    public class SettingItemGroup
    {
        public string UniqueId { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }

        public ObservableCollection<SettingItem> Items { get;  set; }
        public override string ToString()
        {
            return this.Title;
        }
    }
}
