using LottoryUWP.DataModel;
using LottoryUWP.Enum;
using LottoryUWP.Utils;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LottoryUWP.Panes
{
    public sealed partial class ContentPane : UserControl, INotifyPropertyChanged
    {


        public Duration interval;
        public Duration Interval
        {
            get
            {
                return interval;
            }

            private set
            {
                if (interval != value)
                {
                    interval = value;
                    OnPropertyChanged("Interval");
                }
            }
        }

        private RunningState drawRunningState = RunningState.Stopped;
        public RunningState DrawRunningState
        {
            get { return drawRunningState; }
            set
            {
                if (drawRunningState != value)
                {
                    drawRunningState = value;

                    OnPropertyChanged("IsStopEnabled");
                    OnPropertyChanged("IsStartEnabled");
                    OnPropertyChanged("IsNextEnabled");
                    OnPropertyChanged("RoundInfo");
                    OnPropertyChanged("RoundInfoDesc");
                }
            }
        }

        private DrawItem LastPickedDrawItem;


        public ContentPane()
        {
            this.InitializeComponent();

            this.DataContext = DrawData.Instance;

            this.Loaded += ContentPane_Loaded;

        }


        private void ContentPane_Loaded(object sender, RoutedEventArgs e)
        {
            var firstItem = DataModel.DrawData.Instance.DrawItems.FirstOrDefault();

            this.NameBlock.Text = firstItem != null ? firstItem.DisplayName : string.Empty;

            NextRoundSetup();

            DataModel.DrawData.Instance.OnDrawDataSourceUpdate += Instance_OnDrawDataSourceUpdate;

        }

        private void Instance_OnDrawDataSourceUpdate(object sender, EventArgs e)
        {
            var firstItem = DataModel.DrawData.Instance.DrawItems.FirstOrDefault();

            this.NameBlock.Text = firstItem != null ? firstItem.DisplayName : string.Empty;

        }

        public bool IsStopEnabled
        {
            get
            {
                return DrawRunningState != RunningState.Stopped;
            }
        }

        public bool IsStartEnabled
        {
            get
            {
                return DrawRunningState == RunningState.Stopped;
            }
        }

        public bool IsNextEnabled
        {
            get
            {
                return DrawRunningState == RunningState.Running;
            }
        }

        public string RoundInfo
        {
            get
            {
                string capacity;

                if (CapacityToggle.IsOn)
                    capacity = string.Format(LottoryUWP.Strings.Resources.InfoBar_Capacity_Applied, CapacitySilder.Value);
                else
                    capacity = Strings.Resources.InfoBar_Capacity_FreeDraw;


                switch (DrawRunningState)
                {
                    case RunningState.Stopped:
                        return string.Format(Strings.Resources.InfoBar_Stopped, this.RoundTitleText.Text, capacity);
                    case RunningState.Starting:
                        return string.Format(Strings.Resources.InfoBar_Starting, this.RoundTitleText.Text, capacity);
                    case RunningState.Running:
                        {
                            var group = DrawData.Instance.RecentGroup;

                            var drawInfo = CapacityToggle.IsOn ? String.Format(Strings.Resources.InfoBar_DrawInfo_Capacity, group?.Items.Count, CapacitySilder.Value) :
                                String.Format(Strings.Resources.InfoBar_DrawInfo_FreeDraw, group?.Items.Count);
                            return string.Format(Strings.Resources.InfoBar_Running, this.RoundTitleText.Text, capacity, drawInfo);
                        }
                    default:
                        return string.Format(Strings.Resources.InfoBar_Default, this.RoundTitleText.Text, capacity);

                }
            }
        }

        public string RoundInfoDesc { get
            {
                if (DrawRunningState == RunningState.Stopped)
                    return LottoryUWP.Strings.Resources.InfoBar_EventTitle_Desc;
                else
                    return string.Empty;
            }
        }

        public string RoundTitle
        {
            get { return this.RoundTitleText.Text; }
            set
            {
                this.RoundTitleText.Text = value;
                OnPropertyChanged("RoundInfo");
            }
        }
        private async void shuffle(RunningState state)
        {
            Interval = new Duration(TimeSpan.FromMilliseconds(600));

            this.DrawRunningState = state;


            while (this.DrawRunningState != RunningState.Stopped)
            {

                VisualStateManager.GoToState(this, "Hide", this.DrawRunningState != RunningState.Stopped);
                await Task.Delay(Interval.TimeSpan);

                var list = DataModel.DrawData.Instance.DrawItems;

                if (list.Count > 0)
                {
                    var next = RandomUtil.Instance.Next(list.Count);

                    LastPickedDrawItem = list[next];

                    this.NameBlock.Text = LastPickedDrawItem.DisplayName;
                }
                else
                {
                    Stop();
                }

                VisualStateManager.GoToState(this, "Show", this.DrawRunningState != RunningState.Stopped);
                await Task.Delay(Interval.TimeSpan);

                if (Interval.TimeSpan.TotalMilliseconds > 200)
                {
                    Interval = new Duration(Interval.TimeSpan - TimeSpan.FromMilliseconds(100));
                }
                else
                {
                    RandomUtil.Instance.Reset();
                    this.DrawRunningState = RunningState.Running;
                }
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        private void AppBarButtonStart_Click(object sender, RoutedEventArgs e)
        {
            if (this.DrawRunningState == RunningState.Stopped)
            {
                Start();
            }
            else
            {
                if (this.DrawRunningState == RunningState.Running)
                {
                    NextDraw();
                }
            }
        }

        private void AppBarButtonStop_Click(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void CapacitySilder_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            OnPropertyChanged("RoundInfo");
        }


        private void CapacityToggle_Toggled(object sender, RoutedEventArgs e)
        {
            OnPropertyChanged("RoundInfo");
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();

            dialog.Title = LottoryUWP.Strings.Resources.ContentDialog_Restart;
            dialog.Content = LottoryUWP.Strings.Resources.ContentDialog_Restart_content;
            dialog.PrimaryButtonText = LottoryUWP.Strings.Resources.ContentDialog_ResetAll_NO;
            dialog.SecondaryButtonText = LottoryUWP.Strings.Resources.DataSetting_Dialog_Ok;

            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                return;

            if (DrawRunningState != RunningState.Stopped)
                Stop();

            DrawData.Instance.ResetDrawData();

            NextRoundSetup();

            ADUtils.AdInstance.RandomShowAd(1);
        }
        private async void AppBarButtonReport_Click(object sender, RoutedEventArgs e)
        {
            var htmlstring = Utils.HTMLPageUtil.DataToHTMLCode(DrawData.Instance, SettingData.Instance);
            var file = await Utils.FileUtil.OpenFileForSave();

            if (await file.WriteTextAsync(htmlstring))
            {
                await Windows.System.Launcher.LaunchFileAsync(file);
            }
        }

        private void Element_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (DrawRunningState == RunningState.Stopped)
            {
                FrameworkElement element = sender as FrameworkElement;
                if (element != null)
                {
                    FlyoutBase.ShowAttachedFlyout(element);
                }
            }
        }

        private void RoundTitleText_LostFocus(object sender, RoutedEventArgs e)
        {
            OnPropertyChanged("RoundInfo");
        }

        private void Start()
        {

            var r = DrawData.Instance.StartNewRound(new DrawItemGroup() { GroupTitle = RoundTitle, GroupCapacity = CapacityToggle.IsOn ? new int?((int)CapacitySilder.Value) : null });

            if (r)
            {
                RearrangeHistoryControls();
                shuffle(RunningState.Starting);
            }
        }

        private async void NextDraw()
        {
            await Task.Delay(RandomUtil.Instance.RandomCore.Next(1000));

            bool r = DrawData.Instance.UpdateDrawData(LastPickedDrawItem);

            if (r)
            {
                OnPropertyChanged("RoundInfo");

                var group = DrawData.Instance.RecentGroup;

                if (group.GroupCapacity.HasValue && group.Items.Count >= group.GroupCapacity.Value)
                {
                    Stop();
                }
            }
        }

        private void Stop()
        {
            shuffle(RunningState.Stopped);

            var group = DrawData.Instance.RecentGroup;

            if (group != null && group.Items.Count == 0)
            {
                DrawData.Instance.DeleteGroupRecord(group);
            }

            //Setup for next round
            NextRoundSetup();
        }

        private void NextRoundSetup()
        {
            RoundTitle = String.Format("Round {0}", DrawData.Instance.RecentRoundIndex + 1);
            CapacitySilder.Maximum = DataModel.DrawData.Instance.DrawItems.Count;
        }


        private void RearrangeHistoryControls()
        {
            foreach (var item in listview.Items)
            {
                ListViewItem lvItem = listview.ContainerFromItem(item) as ListViewItem;

                if (lvItem != null)
                {
                    Expander expander = VisualTreeHelperUtil.GetFrameworkElementByName<Expander>(lvItem);

                    expander.IsExpanded = false;
                }
            }
        }

        public void KeyUpHandling(object sender, KeyRoutedEventArgs e)
        {

            if(e.Key == Windows.System.VirtualKey.F5)
            {
                if (this.DrawRunningState == RunningState.Stopped)
                {
                    Start();
                }              
            }

            if(e.Key == Windows.System.VirtualKey.F10)
            {
                if (this.DrawRunningState == RunningState.Running)
                {
                    NextDraw();
                }
            }

            if (e.Key == Windows.System.VirtualKey.F2)
            {
                if (this.DrawRunningState == RunningState.Stopped)
                {
                    FlyoutBase.ShowAttachedFlyout(RoundInfoPanel);
                }
            }

        }
    }
}



