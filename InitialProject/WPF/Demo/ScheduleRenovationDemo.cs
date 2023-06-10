using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.OwnerViews;
using Xceed.Wpf.Toolkit;

namespace InitialProject.WPF.Demo
{
    public class ScheduleRenovationDemo : INotifyPropertyChanged
    {
        private ScheduleRenovationViewModel viewModel;
        private ScheduleRenovationView scheduleRenovationView;
        private MyRenovationsView myRenovationsView;
        private MyRenovationsViewModel myRenovationsViewModel;
        private AccommodationRenovation renovation;
        private int increment = -1;
        private bool showDemoMessage;
        private DemoIsOffView demoIsOffView;
        private DemoIsOnView demoIsOnView;
        public ScheduleRenovationDemo(bool showDemoMessage)
        {
            OwnerService ownerService = new OwnerService();
            Owner owner = ownerService.GetAll()[0];
            renovation = new AccommodationRenovation();
            renovation.Id = 0;
            scheduleRenovationView = new ScheduleRenovationView(owner);
            viewModel = scheduleRenovationView.ViewModel;
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = scheduleRenovationView;
            this.showDemoMessage = showDemoMessage;
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
                if(showDemoMessage)
                {
                    demoIsOnView = new DemoIsOnView();
                    demoIsOnView.Show();
                }
            }
            if(Increment == 3)
            {
                if (showDemoMessage)
                    demoIsOnView.Close();

                viewModel.SelectedAccommodation = viewModel.Accommodations[0];
                renovation.Accommodation = viewModel.SelectedAccommodation;
            }
            if(Increment == 5)
            {
                viewModel.StartDate = DateTime.Now.AddDays(1);
            }
            if(Increment == 6)
            {
                viewModel.EndDate = DateTime.Now.AddDays(6);
            }
            if(Increment == 8)
            {
                viewModel.Duration = 2;
            }
            if(Increment == 10)
            {
                viewModel.SelectedDateRange = viewModel.DatesSuggestions[0];
                renovation.StartDate = viewModel.SelectedDateRange.Arrival;
                renovation.EndDate = viewModel.SelectedDateRange.Departure;
            }
            if(Increment == 11)
            {
                scheduleRenovationView.InputDescription();
                renovation.Description = "New renovation";
            }
            if (Increment == 17)
            {
                scheduleRenovationView.PressConfirm();
            }
            if(Increment == 18)
            {
                myRenovationsView = new MyRenovationsView(viewModel.owner);
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = myRenovationsView;
                myRenovationsViewModel = myRenovationsView.ViewModel;
                myRenovationsViewModel.Renovations.Add(renovation);
                myRenovationsViewModel.IsDemoOn = true;
                myRenovationsViewModel.StackPanelMessage = "New renovation successfully scheduled!";
                myRenovationsViewModel.StackPanelVisibility = "Visible";
            }
            if(Increment == 19)
            {
                myRenovationsView.PressOkInDemo();
            }
            if(Increment== 20)
            {
                myRenovationsViewModel.StackPanelVisibility = "Hidden";
            }
            if(Increment == 22)
            {
                myRenovationsViewModel.IsDemoOn = false;
                myRenovationsViewModel.Renovations.Remove(renovation);
                demoIsOffView = new DemoIsOffView();
                demoIsOffView.Show();
            }
            if(Increment == 24)
            {
                demoIsOffView.Close();
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
