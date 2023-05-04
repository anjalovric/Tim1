using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DotLiquid.Tags;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.OwnerViews;

namespace InitialProject.WPF.Demo
{
    public class ScheduleRenovationDemo : INotifyPropertyChanged
    {
        private ScheduleRenovationViewModel viewModel;
        private int increment = -1;
        public ScheduleRenovationDemo()
        {
            OwnerService ownerService = new OwnerService();
            Owner owner = ownerService.GetAll()[0];
            ScheduleRenovationView scheduleRenovationView = new ScheduleRenovationView(owner);
            viewModel = scheduleRenovationView.viewModel;
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = scheduleRenovationView;
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
                viewModel.SelectedAccommodation = viewModel.Accommodations[0];
            if(Increment == 3)
                viewModel.StartDate = DateTime.Now.AddDays(1);
            if(Increment == 4)
                viewModel.EndDate = DateTime.Now.AddDays(6);
            if(Increment == 6)
                viewModel.Duration = 2;
            if(Increment == 10)
            {
                viewModel.SelectedDateRange = viewModel.DatesSuggestions[0];
                viewModel.ConfirmCommand.Execute(null);
            }
            if(Increment == 8)
                viewModel.Description = "new renovation";
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
