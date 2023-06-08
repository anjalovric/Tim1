using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DotLiquid.Tags;
using InitialProject.Model;
using InitialProject.Service;
using System.Windows.Threading;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using InitialProject.WPF.Views;
using System.Windows;

namespace InitialProject.WPF.Demo
{
    public class AccommodationDemo : INotifyPropertyChanged
    {
        private AccommodationView accommodationView;
        private AccommodationViewModel accommodationViewModel;
        private int increment = -1;
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
                accommodationViewModel.IsNewAccommodationPressedInDemo = true;
            }
            if(Increment == 3)
            {
                accommodationViewModel.IsNewAccommodationPressedInDemo = false;
                timer.Stop();
                AccommodationInputDemo inputDemo = new AccommodationInputDemo();
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
