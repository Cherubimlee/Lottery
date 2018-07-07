using LottoryUWP.DataModel;
using LottoryUWP.FTUE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public MainPage()
        {
            this.InitializeComponent();

            this.Loaded += MainPage_Loaded;

        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //make title bar transparent
            ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            formattableTitleBar.ButtonBackgroundColor = Colors.Transparent;
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            FTUEDialog dialog = new FTUEDialog();
            dialog.ShowAsync();
        }

        private  void Button_Click(object sender, RoutedEventArgs e)
        {

            if (Menu.Visibility == Visibility.Collapsed)
            {
                Menu.Visibility = Visibility.Visible;
                VisualStateManager.GoToState(this, "MenuShow", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "MenuHide", false);
                Menu.Visibility = Visibility.Collapsed;
            }
               
        }

        public SettingData Settings { get { return SettingData.Instance; } }
    }
}
