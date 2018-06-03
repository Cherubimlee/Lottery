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

        public event EventHandler OnDrawDataSourceUpdate;

        public DrawData()
        {
            InitDrawData();
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

        public async void InitDrawData()
        {
            var r = await SettingData.Instance.DrawDataSource.GenerateDataItems();

            if (r == null)
                return;

            if (OrignalDrawItems.Count > 0)
                OrignalDrawItems.Clear();

            OrignalDrawItems.AddRange(r.ItemList);

            this.ColumnTitles = r.ColumnTitles;

            ResetDrawData();

            this.OnDrawDataSourceUpdate?.Invoke(this, null);
        }

        public String[] ColumnTitles { get; set; } = new String[2];

        public static DrawData Instance { get { return instance; } }
    }
}
