using LottoryUWP.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace LottoryUWP.DataModel
{
    public class SettingData : INotifyPropertyChanged
    {
    
        private static SettingData instance = new SettingData();
       
        public static SettingData Instance { get { return instance; } }

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



        private TextPositionState eventTitleState = TextPositionState.Left;
        public TextPositionState EventTitleState
        {
            get
            {
                return eventTitleState;
            }

            set
            {
                if(eventTitleState != value)
                {
                    eventTitleState = value;

                    if (value != TextPositionState.None)
                        OnPropertyChanged("EventTitlePosition");
                    else
                        OnPropertyChanged("IsEventTitleEnable");

                    OnPropertyChanged("IsEventTitleEnable");
                }
            }
        }
        [JsonIgnore]
        public Windows.UI.Xaml.Visibility IsEventTitleEnable
        {
            get { return EventTitleState == TextPositionState.None ? Windows.UI.Xaml.Visibility.Collapsed : Windows.UI.Xaml.Visibility.Visible; }
        }

        [JsonIgnore]
        public Windows.UI.Xaml.HorizontalAlignment EventTitlePosition
        {
            get
            {
                switch (EventTitleState)
                {
                    case TextPositionState.Left:
                        return Windows.UI.Xaml.HorizontalAlignment.Left;
                    case TextPositionState.Center:
                        return Windows.UI.Xaml.HorizontalAlignment.Center;
                    case TextPositionState.Right:
                        return Windows.UI.Xaml.HorizontalAlignment.Right;
                    default:
                        return Windows.UI.Xaml.HorizontalAlignment.Left;
                }
            }
        }

        private ObservableCollection<BrushModel> backgroundBrushModels;
        public ObservableCollection<BrushModel> BackgroundBrushModels { get
            {
                if (backgroundBrushModels == null)
                {
                    backgroundBrushModels = new ObservableCollection<BrushModel>(BrushModel.GetBuiltInModels());
                }

                return backgroundBrushModels;
            }
            set
            {
                if (backgroundBrushModels == null)
                    backgroundBrushModels = new ObservableCollection<BrushModel>();
                else
                    backgroundBrushModels.Clear();

                foreach(var item in value)
                {
                    backgroundBrushModels.Add(item);
                }
            }
        }
    

        [JsonIgnore]
        public Brush BackgroundBrush { get { return BackgroundBrushModels.FirstOrDefault()?.BrushObj; } }

        public void SelectBackgroundColor(BrushModel brush)
        {
            BackgroundBrushModels.Remove(brush);  
            BackgroundBrushModels.Insert(0, brush);
          
            OnPropertyChanged("BackgroundBrush");
        }

        public void RemoveBackgroundColor(BrushModel brush)
        {
            int index = BackgroundBrushModels.IndexOf(brush);

            if (index >= 0)
                BackgroundBrushModels.RemoveAt(index);

            if(index == 0)
                OnPropertyChanged("BackgroundBrush");

            if(brush.IsAllowDelete)
            {
                Task.Run(async () =>
                {
                    var file = await StorageFile.GetFileFromPathAsync(brush.URIString);
                    if (file != null)
                        await file.DeleteAsync();
                });
            }
        }

        
        public async Task InsertBackgroundColor(StorageFile file)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFolder imgFolder = null;

            try
            {
                imgFolder = await folder.GetFolderAsync("BackgroundImg");
            }
            catch { }

            if(imgFolder == null)
            {
                imgFolder = await folder.CreateFolderAsync("BackgroundImg");
            }

            var copiedFile = await file.CopyAsync(imgFolder,file.Name,NameCollisionOption.GenerateUniqueName);

            BrushModel brush = new BrushModel() { URIString = copiedFile.Path };
            BackgroundBrushModels.Add(brush);

        }

        public String Serialize()
        {
                return JsonConvert.SerializeObject(this);
        }

        public static List<SettingItemGroup>  BuildSettingList()
        {
            List<SettingItemGroup> settingGroups  = new List<SettingItemGroup>();
            List<SettingItem> items = new List<SettingItem>();
            items.Add(new SettingItem() { StyleType = Enum.SettingType.DisplayStyleSetting });


            settingGroups.Add(new SettingItemGroup()
            {
                Title = LottoryUWP.Strings.Resources.SettingTitle_Event_Appearance,
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
