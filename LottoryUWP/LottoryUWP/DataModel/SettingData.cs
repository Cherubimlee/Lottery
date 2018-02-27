using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoryUWP.DataModel
{
    public class SettingData : INotifyPropertyChanged
    {
        private static SettingData instance = new SettingData();
        public static SettingData Instance { get { return instance; } }

        private String eventTitle = "New Event";
        [JsonProperty]
        public String EventTitle
        {
            get { return eventTitle; }
            set
            {
                if (!eventTitle.Equals(value))
                {
                    eventTitle = value;
                    OnPropertyChanged("EventTitle");
                }
            }
        }

        private String majorColumnTitle = "Name";
        [JsonProperty]
        public String MajorColumnTitle
        {
            get { return majorColumnTitle; }
            set
            {
                if (!majorColumnTitle.Equals(value))
                {
                    majorColumnTitle = value;
                    OnPropertyChanged("MajorColumnTitle");
                }
            }
        }
        private String secondaryColumnTitle = "Id";
        [JsonProperty]
        public String SecondaryColumnTitle
        {
            get { return secondaryColumnTitle; }
            set
            {
                if (!secondaryColumnTitle.Equals(value))
                {
                    secondaryColumnTitle = value;
                    OnPropertyChanged("SecondaryColumnTitle");
                }
            }
        }

        public String Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static List<SettingItemGroup>  BuildSettingList()
        {
            List<SettingItemGroup> settingGroups  = new List<SettingItemGroup>();
            List<SettingItem> items = new List<SettingItem>();
            items.Add(new SettingItem() { Title = "Winner Style", StyleType = Enum.SettingType.WinnerItemStyleDemo });


            settingGroups.Add(new SettingItemGroup()
            {
                Title = "Winner Display Style",
                Items = new System.Collections.ObjectModel.ObservableCollection<SettingItem>(items)
            });
            return settingGroups;
        }

        public static SettingData CreateFromJson(string jsonString)
        {
            return JsonConvert.DeserializeObject<SettingData>(jsonString);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
