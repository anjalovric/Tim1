using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.OwnerViews;

namespace InitialProject.WPF.Demo
{
    public class AccommodationDemo : INotifyPropertyChanged
    {
        private AccommodationView accommodationView;
        private AccommodationViewModel accommodationViewModel;
        private int increment = -1;
        private DemoIsOnView demoIsOnView;
        public AccommodationDemo()
        {
            accommodationView = (AccommodationView?)Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content;
            accommodationViewModel = accommodationView.ViewModel;
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
            if(Increment == 1)
            {
                demoIsOnView = new DemoIsOnView();
                demoIsOnView.Show();
            }
            if(Increment == 2)
            {
                demoIsOnView.Close();
                accommodationViewModel.IsNewAccommodationPressedInDemo = true;
            }
            if(Increment == 3)
            {
                accommodationViewModel.IsNewAccommodationPressedInDemo = false;
                timer.Stop();
                AccommodationInputDemo inputDemo = new AccommodationInputDemo(false);
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
