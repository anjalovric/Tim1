using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid.Tags;
using System.Windows.Threading;
using System.Runtime.CompilerServices;
using InitialProject.WPF.Views;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using InitialProject.WPF.Views.OwnerViews;
using System.Windows;
using InitialProject.Service;
using InitialProject.Model;

namespace InitialProject.WPF.Demo
{
    public class DecliningRequestDemo : INotifyPropertyChanged
    {
        private int increment = -1;
        private ReservationReschedulingView view;
        private ReservationReschedulingViewModel viewModel;
        private DecliningRequestViewModel decliningViewModel;
        private DecliningRequestView decliningView;
        private RequestForReshcedulingViewModel request;
        private DemoIsOffView demoIsOffView;
        private DemoIsOnView demoIsOnView;
        public DecliningRequestDemo()
        {
            view = (ReservationReschedulingView?)Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content;
            viewModel = view.reservationReschedulingViewModel;
            MakeRequest();
            viewModel.Requests.Add(request);
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
                demoIsOnView = new DemoIsOnView();
                demoIsOnView.Show();
            }
            if (Increment == 3)
            {
                demoIsOnView.Close();
                viewModel.SelectedRequest = viewModel.Requests[viewModel.Requests.Count - 1];
                view.RequestsListBox.SelectedItem = viewModel.SelectedRequest;
            }
            if (Increment == 4)
            {
                decliningView = new DecliningRequestView(request.Request);
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = decliningView;
                decliningViewModel = decliningView.decliningRequestViewModel;
                decliningViewModel.ReschedulingAccommodationRequest = new ReschedulingAccommodationRequest();
                decliningViewModel.ReschedulingAccommodationRequest.Reservation = new AccommodationReservation();
                Guest1Service guest1Service = new Guest1Service();
                decliningViewModel.ReschedulingAccommodationRequest.Reservation.Guest = guest1Service.GetById(1);
            }
            if (Increment == 5)
            {
                decliningView.InputExplanation();
            }
            if (Increment == 10)
            {
                decliningViewModel.IsConfirmPressedInDemo = true;
            }
            if (Increment == 11)
            {
                decliningViewModel.IsConfirmPressedInDemo = false;
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = view;
                viewModel.Requests.Remove(request);
                viewModel.StackPanelMessage = "Request successfully declined!";
                viewModel.StackPanelVisibility = "Visible";
            }
            if (Increment == 12)
            {
                viewModel.IsOkPressedInDemo = true;
            }
            if (Increment == 13)
            {
                viewModel.IsOkPressedInDemo = false;
                viewModel.StackPanelVisibility = "Hidden";
                demoIsOffView = new DemoIsOffView();
                demoIsOffView.Show();
            }
            if(Increment == 15)
            {
                demoIsOffView.Close();
            }
        }

        private void MakeRequest()
        {
            OwnerService ownerService = new OwnerService();
            request = new RequestForReshcedulingViewModel(ownerService.GetById(1));
            request.OldArrival = DateOnly.Parse("7/10/2023");
            request.NewArrival = DateOnly.Parse("7/1/2023");
            request.OldDeparture = DateOnly.Parse("7/12/2023");
            request.NewDeparture = DateOnly.Parse("7/10/2023");
            AccommodationReservation reservation = new AccommodationReservation();
            Accommodation accommodation = new Accommodation();
            accommodation.Name= "House";
            reservation.Accommodation = accommodation;
            request.AccommodationReservation = reservation;
            LocationService locationService = new LocationService();
            request.AccommodationReservation.Accommodation.Location = locationService.GetById(1);
            request.IsAccommodationAvailable = true;
            request.Availability = "available";
            Guest1Service guest1Service = new Guest1Service();
            request.AccommodationReservation.Guest = guest1Service.GetById(1);
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
