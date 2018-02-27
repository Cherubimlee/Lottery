using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoryUWP.DataModel
{
    public class DrawData
    {
       
        private static DrawData instance = new DrawData();
        private object listLock = new object();
        public DrawData()
        {

            for (int i = 0; i < 100; i++)
            {
                string N1 = string.Format("Name{0}", i);
                string N2 = string.Format("Id{0}", i);
                OrignalDrawItems.Add(new DrawItem() { MajorColumnValue = N1, SecondaryColumnValue = N2 });
            }

            ResetDrawData();
        }

        public List<DrawItem> OrignalDrawItems { get; set; } = new List<DrawItem>();
        public List<DrawItem> DrawItems { get; set; } = new List<DrawItem>();
        public ObservableCollection<DrawItemGroup> DrawHistory { get; set; } = new ObservableCollection<DrawItemGroup>();
        public DrawItemGroup RecentGroup
        {
            get
            {
                return DrawHistory.FirstOrDefault();
            }
        }
        public int RecentRoundIndex
        {
            get { return DrawHistory.Count; }
        }


        public bool StartNewRound(DrawItemGroup newGroup)
        {
            if (DrawItems.Count == 0)
                return false;

            newGroup.groupIndex = DrawHistory.Count + 1;
            DrawHistory.Insert(0, newGroup);

            return true;
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

        public void ResetDrawData()
        {
            lock (listLock)
            {
                DrawItems.Clear();
                DrawItems.AddRange(OrignalDrawItems);
                DrawHistory.Clear();
            }
        }

        public static DrawData Instance { get { return instance; } }
    }
}
