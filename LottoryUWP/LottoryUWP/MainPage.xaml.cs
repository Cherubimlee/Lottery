using LottoryUWP.DataModel;
using LottoryUWP.Enum;
using LottoryUWP.FTUE;
using LottoryUWP.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LottoryUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private const string versionLevelKey = "VersionLevel";



        public MainPage()
        {
            this.InitializeComponent();

            this.Loaded += MainPage_Loaded;
            
        }

 

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //make title bar transparent
            ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            formattableTitleBar.ButtonBackgroundColor = Colors.Transparent;
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            var lastVersion = System.Enum.GetValues(typeof(VersionLevel)).Cast<VersionLevel>().LastOrDefault();

            object versionLevelObj;
            VersionLevel versionLevel = VersionLevel.Ver_1_1;

            ApplicationData.Current.LocalSettings.Values.TryGetValue(versionLevelKey, out versionLevelObj);
            System.Enum.TryParse<VersionLevel>(versionLevelObj?.ToString(), out versionLevel);

            if (versionLevel < lastVersion)
            {
                FTUEDialog dialog = new FTUEDialog(versionLevel);
                await dialog.ShowAsync();
                ApplicationData.Current.LocalSettings.Values[versionLevelKey] = lastVersion.ToString();
            }

        }

        private async  void Button_Click(object sender, RoutedEventArgs e)
        {

            if (Menu.Visibility == Visibility.Collapsed)
            {
                Menu.Visibility = Visibility.Visible;
                VisualStateManager.GoToState(this, "MenuShow", true);

                await Task.Delay(500);
                ADUtils.AdInstance.RandomShowAd(0.3);
            }
            else
            {
                VisualStateManager.GoToState(this, "MenuHide", false);
                Menu.Visibility = Visibility.Collapsed;
                ADUtils.AdInstance.RandomShowAd(0.6);
            }

          
        }



        public SettingData Settings { get { return SettingData.Instance; } }
    }
}
