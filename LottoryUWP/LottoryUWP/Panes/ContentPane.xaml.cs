using LottoryUWP.DataModel;
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
        public enum RunningState
        {
            Stopped,
            Running,
            Starting
        }

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
                }
            }
        }

        private DrawItem LastPickedDrawItem;

       


        public ContentPane()
        {
            this.InitializeComponent();

            this.DataContext = Data.Instance;

            this.Loaded += ContentPane_Loaded;
        }

       
        private void ContentPane_Loaded(object sender, RoutedEventArgs e)
        {
            var firstItem =  DataModel.Data.Instance.DrawItems.FirstOrDefault();

            this.NameBlock.Text = firstItem != null ? firstItem.DisplayName : string.Empty;

            NextRoundSetup();
         
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
                    capacity = "Applied Group Capacity " + CapacitySilder.Value;
                else
                    capacity = "Free Draw";
               

                switch(DrawRunningState)
                {
                    case RunningState.Stopped:
                        return string.Format("Next: {0} | {1}", this.RoundTitleText.Text, capacity);
                    case RunningState.Starting:
                        return string.Format("Starting {0} | {1}", this.RoundTitleText.Text, capacity);                  
                    case RunningState.Running:
                        {
                            var group = Data.Instance.RecentGroup;

                            var drawInfo = CapacityToggle.IsOn ? String.Format("{0} out of {1} Lucky Winner(s)", group?.Items.Count, CapacitySilder.Value):
                                String.Format("{0} Lucky Winner(s)", group?.Items.Count);
                            return string.Format("{0} | {1} | {2}", this.RoundTitleText.Text, capacity, drawInfo);
                        }
                    default:
                        return string.Format("{0} | {1}", this.RoundTitleText.Text, capacity);
                      
                }
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
            Interval = new Duration(TimeSpan.FromMilliseconds(500));

            this.DrawRunningState = state;
           

            while (this.DrawRunningState != RunningState.Stopped)
            {
                
                VisualStateManager.GoToState(this, "Hide", this.DrawRunningState !=  RunningState.Stopped);
                await Task.Delay(Interval.TimeSpan);

                var list = DataModel.Data.Instance.DrawItems;

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

                if(Interval.TimeSpan.TotalMilliseconds > 200)
                {
                    Interval = new Duration(Interval.TimeSpan - TimeSpan.FromMilliseconds(50));
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

        protected void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        private  void AppBarButtonStart_Click(object sender, RoutedEventArgs e)
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

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (DrawRunningState != RunningState.Stopped)
                Stop();

            Data.Instance.ResetDrawData();
            NextRoundSetup();
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

            var r = Data.Instance.StartNewRound(new DrawItemGroup() { GroupTitle = RoundTitle, GroupCapacity = CapacityToggle.IsOn ? new int?((int)CapacitySilder.Value) : null });

            if (r)
            {
                RearrangeHistoryControls();
                shuffle(RunningState.Starting);
            }
        }

        private async void NextDraw()
        {
            await Task.Delay(RandomUtil.Instance.RandomCore.Next(1000));

            bool r = Data.Instance.UpdateDrawData(LastPickedDrawItem);

            if (r)
            {
                OnPropertyChanged("RoundInfo");

                var group = Data.Instance.RecentGroup;

                if (group.GroupCapacity.HasValue && group.Items.Count >= group.GroupCapacity.Value)
                {
                    Stop();
                }
            }
        }

        private void Stop()
        {
            shuffle(RunningState.Stopped);

            var group = Data.Instance.RecentGroup;

            if(group != null && group.Items.Count == 0)
            {
                Data.Instance.DeleteGroupRecord(group);
            }

            //Setup for next round
            NextRoundSetup();
        }

        private void NextRoundSetup()
        {
            RoundTitle = String.Format("Round {0}", Data.Instance.RecentRoundIndex + 1);
            CapacitySilder.Maximum = DataModel.Data.Instance.DrawItems.Count;
        }
       

        private void RearrangeHistoryControls()
        {
            foreach(var item in listview.Items)
            {
                ListViewItem lvItem = listview.ContainerFromItem(item) as ListViewItem;

                if(lvItem != null)
                {
                    Expander expander =  VisualTreeHelperUtil.GetFrameworkElementByName<Expander>(lvItem);

                    expander.IsExpanded = false;
                }
            }
        }

        
    }
}



