using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

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

        private Color winnerColor = Colors.Transparent;
        [JsonProperty]
        public Color WinnerColor
        {
            get
            {
                return winnerColor;
            }
            set
            {
                winnerColor = value;
                OnPropertyChanged("WinnerColor");
            }
        }

        private bool isWinnerColorRandom = true;
        [JsonProperty]
        public bool IsWinnerColorRandom
        {
            get
            {
                return isWinnerColorRandom;
            }
            set
            {
                isWinnerColorRandom = value;
                OnPropertyChanged("IsWinnerColorRandom");
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
            items.Add(new SettingItem() { Title = "Customize Style", StyleType = Enum.SettingType.DisplayStyleSetting });


            settingGroups.Add(new SettingItemGroup()
            {
                Title = "Customize Event Style",
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
