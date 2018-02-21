using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoryUWP.DataModel
{
    public class Data
    {
        public String EventTitle { get; set; } = "New Event";
        public String MajorColumnTitle { get; set; } = "Major Column";
        public String SecondaryColumnTitle { get; set; } = "Secondary Column";
        private object listLock = new object();
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
                OrignalDrawItems.Add(new DrawItem() { MajorColumnValue = N1, SecondaryColumnValue = N2 });
            }

            ResetDrawData();
        }

        private static Data instance = new Data();

        static public Data Instance { get { return instance; } }

        public List<DrawItem> OrignalDrawItems { get; set; } = new List<DrawItem>();
        public List<DrawItem> DrawItems { get; set; } = new List<DrawItem>();

        public List<SettingItemGroup> SettingGroups { get; set; } = new List<SettingItemGroup>();

        public ObservableCollection<DrawItemGroup> DrawHistory { get; set; } = new ObservableCollection<DrawItemGroup>();

        public bool StartNewRound(DrawItemGroup newGroup)
        {
            if (DrawItems.Count == 0)
                return false;

            newGroup.groupIndex = DrawHistory.Count + 1;
            DrawHistory.Insert(0, newGroup);

            return true;
        }

        public DrawItemGroup RecentGroup
        {
            get
            {
                return DrawHistory.FirstOrDefault();
            }
        }

        public void DeleteGroupRecord(DrawItemGroup group)
        {
            DrawHistory.Remove(group);
        }

        public bool UpdateDrawData(DrawItem item)
        {
            lock (listLock)
            {
                var group = DrawHistory.FirstOrDefault();
                if (group != null)
                {
                    if ((!group.Items.Contains(item))&& DrawItems.Contains(item))
                    {
                        group.Items.Insert(0, item.ToWinnerItem(group.groupIndex));
                        DrawItems.Remove(item);
                        return true;
                    }
                }

                return false;
            }
        }

        public int RecentRoundIndex
        {
            get { return DrawHistory.Count; }
        }

        public void ResetDrawData()
        {
            lock (listLock)
            {
                DrawItems.Clear();
                DrawItems.AddRange(OrignalDrawItems);
                DrawHistory.Clear();
            }
        }

    }
}
