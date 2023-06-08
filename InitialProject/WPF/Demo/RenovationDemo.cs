using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.OwnerViews;

namespace InitialProject.WPF.Demo
{
    public class RenovationDemo : INotifyPropertyChanged
    {
        private MyRenovationsView view;
        private MyRenovationsViewModel viewModel;
        private int increment = -1;
        public RenovationDemo()
        {
            view = (MyRenovationsView?)Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content;
            viewModel = view.ViewModel;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void PlayDemo()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += new EventHandler(Timer_Tick);
            dispatcherTimer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            Increment++;
            if (Increment == 1)
            {
                viewModel.IsNewRenovationPressedInDemo = true;
            }
            if (Increment == 3)
            {
                viewModel.IsNewRenovationPressedInDemo = false;
                timer.Stop();
                ScheduleRenovationDemo inputDemo = new ScheduleRenovationDemo();
                inputDemo.PlayDemo();
            }

        }

        public int Increment
        {
            get { return increment; }
            set
            {
                if (value != increment)
                {
                    increment = value;
                    OnPropertyChanged();
                }
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
