using LottoryUWP.DataModel;
using LottoryUWP.Utils;
using System;
using System.Collections.Generic;
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
                }
            }
        }

        private DrawItem LastPickedDrawItem;

        private List<DrawItem> DrawHistory = new List<DrawItem>();

        public ContentPane()
        {
            this.InitializeComponent();


            this.Loaded += ContentPane_Loaded;
        }

       
        private void ContentPane_Loaded(object sender, RoutedEventArgs e)
        {
            var firstItem =  DataModel.Data.Instance.DrawItems.FirstOrDefault();

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
                string groupTitle = "Round 1", capacity;

                if (CapacityToggle.IsOn)
                    capacity = "Applied Group Capacity " + CapacitySilder.Value;
                else
                    capacity = "Free Draw";


                return string.Format("{0} | {1}", groupTitle, capacity);
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
               
                VisualStateManager.GoToState(this, "Show", this.DrawRunningState != RunningState.Stopped);
                await Task.Delay(Interval.TimeSpan);

                if(Interval.TimeSpan.TotalMilliseconds > 200)
                {
                    Interval = new Duration(Interval.TimeSpan - TimeSpan.FromMilliseconds(50));
                }
                else
                {
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

        private async void AppBarButtonStart_Click(object sender, RoutedEventArgs e)
        {
            if (this.DrawRunningState == RunningState.Stopped)
                shuffle(RunningState.Starting);
            else
            {
                if (this.DrawRunningState == RunningState.Running)
                {
                    await Task.Delay(RandomUtil.Instance.Next(1000));

                    var item = LastPickedDrawItem;

                    if (!DrawHistory.Contains(item))
                        DrawHistory.Add(item);
                }
            }
        }

        private void AppBarButtonStop_Click(object sender, RoutedEventArgs e)
        {
          shuffle(RunningState.Stopped);
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

        }
    }
}



