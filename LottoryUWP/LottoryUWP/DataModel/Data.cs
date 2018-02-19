using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoryUWP.DataModel
{
    public class Data
    {

        public Data()
        {

                
                List<SettingItem> items = new List<SettingItem>();
                items.Add(new WinnerStyleSettingItem() { Title = "Winner Style" });


                SettingGroups.Add(new SettingItemGroup()
                {
                    Title = "Winner Display Style",
                    Items = new System.Collections.ObjectModel.ObservableCollection<SettingItem>(items)
                });


            for (int i = 0; i < 100; i++)
            {
                string N1 = string.Format("Name{0}", i);
                string N2 = string.Format("Id{0}", i);
                DrawItems.Add(new DrawItem() { MajorColumnValue = N1, SecondaryColumnValue = N2 });
            }
        }

        private static Data instance = new Data();

        static public Data Instance { get { return instance; } }

        public List<DrawItem> DrawItems { get; set; } = new List<DrawItem>();

        public List<SettingItemGroup> SettingGroups { get; set; } = new List<SettingItemGroup>();

        public List<DrawItemGroup> DrawHistory { get; set; } = new List<DrawItemGroup>();


    }
}
