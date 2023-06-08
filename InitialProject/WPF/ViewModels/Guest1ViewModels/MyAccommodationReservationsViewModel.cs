using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest1Views;
using LiveCharts.Wpf;
using LiveCharts;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class MyAccommodationReservationsViewModel : INotifyPropertyChanged
    {
        public Func<double, string> YAxisLabelFormatter => value => value.ToString("N1");
        private Guest1 guest1;
        private DateTime currentDate;
        private AccommodationReservationService accommodationReservationService;
        private CancelledAccommodationReservationService cancelledAccommodationReservationService;
        public List<string> Labels { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        private ObservableCollection<AccommodationReservation> completedReservations;
        public ObservableCollection<AccommodationReservation> CompletedReservations
        {
            get { return completedReservations; }
            set
            {
                if (value != completedReservations)
                    completedReservations = value;
                OnPropertyChanged("CompletedReservations");
            }

        }

        private ObservableCollection<AccommodationReservation> notCompletedReservations;
        public ObservableCollection<AccommodationReservation> NotCompletedReservations
        {
            get { return notCompletedReservations; }
            set
            {
                if (value != notCompletedReservations)
                    notCompletedReservations = value;
                OnPropertyChanged("NotCompletedReservations");
            }

        }

        private AccommodationReservation selectedCompletedReservation;
        public AccommodationReservation SelectedCompletedReservation
        {
            get { return selectedCompletedReservation; }
            set
            {
                if (value != selectedCompletedReservation)
                    selectedCompletedReservation = value;
                OnPropertyChanged("SelectedCompletedReservation");
            }

        }

        private AccommodationReservation selectedNotCompletedReservation;
        public AccommodationReservation SelectedNotCompletedReservation
        {
            get { return selectedNotCompletedReservation; }
            set
            {
                if (value != selectedNotCompletedReservation)
                    selectedNotCompletedReservation = value;
                OnPropertyChanged("SelectedNotCompletedReservation");
            }

        }
        public RelayCommand RateOwnerAndAccommodationCommand { get; set; }
        public RelayCommand CancelReservationCommand { get; set; }
        public RelayCommand RescheduleReservationCommand { get; set; }

        public MyAccommodationReservationsViewModel(Guest1 guest1)
        {
            this.guest1 = guest1;
            Initialize();
            SetChartData();
            MakeCommands();
        }
        private void Initialize()
        {
            accommodationReservationService = new AccommodationReservationService();
            cancelledAccommodationReservationService = new CancelledAccommodationReservationService();
            CompletedReservations = new ObservableCollection<AccommodationReservation>(accommodationReservationService.GetCompletedReservations(guest1));
            NotCompletedReservations = new ObservableCollection<AccommodationReservation>(accommodationReservationService.GetNotCompletedReservations(guest1));
            NotCompletedReservations = new ObservableCollection<AccommodationReservation>(NotCompletedReservations.OrderByDescending(x => x.Arrival > DateTime.Now).ToList());    //group: first will be shown reservations which haven't started yet
            currentDate = DateTime.Now;
        }
        private void MakeCommands()
        {
            RateOwnerAndAccommodationCommand = new RelayCommand(RateOwnerAndAccommodation_Executed, CanExecute);
            CancelReservationCommand = new RelayCommand(CancelReservation_Executed, CanExecute);
            RescheduleReservationCommand = new RelayCommand(RescheduleReservation_Executed, CanExecute);
        }

        //methods for diagram
        private void SetChartData()
        {
            LastYearAccommodationReservationsService lastYearAccommodationReservationsService = new LastYearAccommodationReservationsService();
            List<int> values = new List<int>();
            Labels = new List<string>();
            DateTime lastYear = currentDate.AddMonths(-13);
            while (lastYear <= currentDate)
            {
                values.Add(lastYearAccommodationReservationsService.GetReservationsNumberByMonthForGuest(lastYear, guest1, currentDate));
                Labels.Add(lastYear.ToString("MMM").ToUpper());
                lastYear = lastYear.AddMonths(1);
            }
            SetSeriesCollection(values);
        }

        private void SetSeriesCollection(List<int> values)
        {
            SeriesCollection = new SeriesCollection();
            ColumnSeries columnSeries = new ColumnSeries();
            columnSeries.Values = new ChartValues<int>();
            for (int i = 0; i < values.Count; i++)
            {
                ChartValues<int> columnValues = new ChartValues<int> { values[i] };
                columnSeries.Values.Add(values[i]);
                columnSeries.Title = "Reservations";
            }
            SeriesCollection.Add(columnSeries);
        }

   
        //other commands 
        private void ShowReviewForm()
        {
            OwnerAndAccommodationReviewFormView ownerAndAccommodationReviewForm = new OwnerAndAccommodationReviewFormView(guest1, SelectedCompletedReservation);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = ownerAndAccommodationReviewForm;
        }       
        
        public async Task<bool> ConfirmCancellationMessageBox()
        {
            var result = new TaskCompletionSource<bool>();
            Guest1YesNoMessageBoxView messageBox = new Guest1YesNoMessageBoxView("Do you want to cancel this reservation?", "/Resources/Images/qm.png", result);
            messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
            messageBox.ShowDialog();
            var returnedResult = await result.Task;
            return returnedResult;
        }
        private void ConfirmCancellation()
        {
            cancelledAccommodationReservationService.Add(SelectedNotCompletedReservation);
            accommodationReservationService.Delete(SelectedNotCompletedReservation);
            NotCompletedReservations.Remove(SelectedNotCompletedReservation);
        }
        //execute commands
        private void RateOwnerAndAccommodation_Executed(object sender)
        {
            OwnerReviewService ownerReviewService = new OwnerReviewService();
            if (ownerReviewService.HasReview(SelectedCompletedReservation))         //Validation- if reservation has review - can't be reviewed again
            {
                ShowMessageBoxForReviewedReservation();
                return;
            }
            if (!IsReservationValidToReview())                                      //if 5 days have passed - can't review
            {
                ShowMessageBoxForInvalidReview();
                return;
            }
            ShowReviewForm();
        }
        private void RescheduleReservation_Executed(object sender)
        {
            if (HasReservationStarted())
                ShowMessageBoxForReschedulingStartedReservation();
            else
            {
                ReschedulingAccommodationReservationFormView form = new ReschedulingAccommodationReservationFormView(SelectedNotCompletedReservation);
                form.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
                form.ShowDialog();
            }       
        }
        private async void CancelReservation_Executed(object sender)
        {
            if (HasReservationStarted())
            {
                ShowMessageBoxForStaredReservationCancellation();
                return;
            }
            if (!IsCancellationAllowed())
            {
                ShowMessageBoxForExpiredCancellationPeriod();
                return;
            }
            Task<bool> result = ConfirmCancellationMessageBox();
            bool IsYesClicked = await result;
            if (IsYesClicked)
                ConfirmCancellation();
        }
        // Validation for review (5 days)
        public bool IsReservationValidToReview()
        {
            return SelectedCompletedReservation.Departure >= DateTime.Now.AddDays(-5);
        }
        //Validation - can't cancel or reschedule started reservation
        private bool HasReservationStarted()
        {
            return DateTime.Now >= SelectedNotCompletedReservation.Arrival;
        }
        //Validation - Owner's conditions for cancellation
        private bool IsCancellationAllowed()
        {
            return DateTime.Now <= SelectedNotCompletedReservation.Arrival.AddHours(-24) && DateTime.Now <= SelectedNotCompletedReservation.Arrival.AddDays(-SelectedNotCompletedReservation.Accommodation.MinDaysToCancel);
        }
        //Message boxes for validation
        private void ShowMessageBoxForReschedulingStartedReservation()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("You can't reschedule this reservation because it has already started.", "/Resources/Images/exclamation.png");
            messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
            messageBox.ShowDialog();
        }
        private void ShowMessageBoxForStaredReservationCancellation()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("You can't cancel this reservation because it has already started.", "/Resources/Images/exclamation.png");
            messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
            messageBox.ShowDialog();
        }
        private void ShowMessageBoxForExpiredCancellationPeriod()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("You can't cancel this reservation because the cancellation period has expired.", "/Resources/Images/exclamation.png");
            messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
            messageBox.ShowDialog();
        }
        private void ShowMessageBoxForReviewedReservation()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("This reservation is already reviewed.", "/Resources/Images/exclamation.png");
            messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
            messageBox.ShowDialog();
        }
        private void ShowMessageBoxForInvalidReview()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("You can't rate this reservation because 5 days have passed since its departure.", "/Resources/Images/exclamation.png");
            messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
            messageBox.ShowDialog();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private bool CanExecute(object sender)
        {
            return true;
        }

    }
}
