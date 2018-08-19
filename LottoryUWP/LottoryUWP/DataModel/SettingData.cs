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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace LottoryUWP.DataModel
{
    public class SettingData : INotifyPropertyChanged
    {
        private const string loaclSettingKey = "SettingConfig";


        private static Object instanceLock = new Object();

        private static SettingData instance;
        public static SettingData Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        object jsonObj = null;
                        ApplicationData.Current.LocalSettings.Values.TryGetValue(loaclSettingKey, out jsonObj);

                        SettingData obj = SettingData.CreateFromJson(jsonObj as string);

                        if (obj != null)
                            instance = obj;
                        else
                            instance = new SettingData() {
                                backgroundBrushModels = new ObservableCollection<BrushModel>(BrushModel.GetBuiltInModels()),
                                drawDataSource = new DataSourceModel() {Start = 1, End=200 }
                            };
                    
                    }
                    return instance;
                }
            }
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

        public bool IsAdFree { get; set; } = false;


        private DataSourceModel drawDataSource;
        public DataSourceModel DrawDataSource {
            get { return drawDataSource; }
            set
            {
                this.drawDataSource = value;
                DrawData.Instance?.InitDrawData();

                OnPropertyChanged("DrawDataSource");
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
        public ObservableCollection<BrushModel> BackgroundBrushModels
        {
            get
            {
                if (backgroundBrushModels == null)
                {
                    backgroundBrushModels = new ObservableCollection<BrushModel>();
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

        public async Task ClearImgCache()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFolder imgFolder = null;

            try
            {
                imgFolder = await folder.GetFolderAsync("BackgroundImg");
            }
            catch { }

            if (imgFolder != null)
            {
              await imgFolder.DeleteAsync();
            }

            BackgroundBrushModels = new ObservableCollection<BrushModel>(BrushModel.GetBuiltInModels());
        }

        public async Task ResetAllSettingAndReboot()
        {
            await ClearImgCache();
            ApplicationData.Current.LocalSettings.Values[loaclSettingKey] = String.Empty;

            App app = App.Current as App;
            app.AppTheme = ApplicationTheme.Light;

            await Windows.ApplicationModel.Core.CoreApplication.RequestRestartAsync("");
        }

        public static List<SettingItemGroup>  BuildSettingList()
        {
            List<SettingItemGroup> settingGroups  = new List<SettingItemGroup>();

            settingGroups.Add(CreateItemGroup(SettingType.Data, LottoryUWP.Strings.Resources.SettingTitle_Event_Data));

            settingGroups.Add(CreateItemGroup(SettingType.DisplayStyleSetting, LottoryUWP.Strings.Resources.SettingTitle_Event_Appearance));

            settingGroups.Add(CreateItemGroup(SettingType.Support, LottoryUWP.Strings.Resources.SettingTitle_Event_Support));

            return settingGroups;
        }

        private static SettingItemGroup CreateItemGroup(SettingType type, String title )
        {
            List<SettingItem> items = new List<SettingItem>();
            items.Add(new SettingItem() { StyleType = type });

            return
            new SettingItemGroup()
            {
                Title = title,
                Items = new System.Collections.ObjectModel.ObservableCollection<SettingItem>(items)
            };
        }

        public static SettingData CreateFromJson(string jsonString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jsonString))
                    return null;
                else
                    return JsonConvert.DeserializeObject<SettingData>(jsonString);
            }
            catch
            {
                return null;
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));

            if (BackgroundBrushModels.Count != 0)
            {
                var str = this.Serialize();
                ApplicationData.Current.LocalSettings.Values[loaclSettingKey] = str;
            }
        }

        #endregion
    }
}
