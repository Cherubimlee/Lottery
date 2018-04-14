using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace LottoryUWP.DataModel
{
    public class SettingData : INotifyPropertyChanged
    {
    
        private static SettingData instance = new SettingData();
       
        public static SettingData Instance { get { return instance; } }

        public SettingData()
        {
            BackgroundBrushes = new ObservableCollection<Brush>(this.BrushModels.Select(x=>x.ToBrush()));

        }


        private String eventTitle = "New Event";

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

        private List<BrushModel> brushModels;
        public List<BrushModel> BrushModels { get
            {
                if (brushModels == null)
                {
                    brushModels = new List<BrushModel>((LottoryUWP.Common.Color.Colors.Select(x => new BrushModel() { SolidBrushColor = x.ColorObj})));

                    brushModels.Add(new BrushModel() { URIString = @"ms-appx:///Assets/Img/BingImg1.jpg" });
                    brushModels.Add(new BrushModel() { URIString = @"ms-appx:///Assets/Img/BingImg2.jpg" });
                    brushModels.Add(new BrushModel() { URIString = @"ms-appx:///Assets/Img/BingImg3.jpg" });
                    brushModels.Add(new BrushModel() { URIString = @"ms-appx:///Assets/Img/BingImg4.jpg" });
                    brushModels.Add(new BrushModel() { URIString = @"ms-appx:///Assets/Add.png" });
                }

                return brushModels;
            }
            set
            {
                brushModels = value;
            }
        }
    
        [JsonIgnore]
        public ObservableCollection<Brush> BackgroundBrushes { get; set; }

        [JsonIgnore]
        public Brush BackgroundBrush { get { return BackgroundBrushes.FirstOrDefault(); } }

        public void SelectBackgroundColor(Brush brush)
        {
            var index = BackgroundBrushes.IndexOf(brush);
            var brushModel = this.BrushModels[index];

            BackgroundBrushes.RemoveAt(index);
            BrushModels.RemoveAt(index);
            BackgroundBrushes.Insert(0, brush);
            BrushModels.Insert(0,brushModel);

            OnPropertyChanged("BackgroundBrush");
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
