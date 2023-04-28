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

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class MyAccommodationReservationsViewModel : INotifyPropertyChanged
    {

        private Guest1 guest1;
        private AccommodationReservationService accommodationReservationService;
        private CancelledAccommodationReservationService cancelledAccommodationReservationService;
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
            accommodationReservationService = new AccommodationReservationService();
            cancelledAccommodationReservationService = new CancelledAccommodationReservationService();

            CompletedReservations = new ObservableCollection<AccommodationReservation>(accommodationReservationService.GetCompletedReservations(guest1));
            NotCompletedReservations = new ObservableCollection<AccommodationReservation>(accommodationReservationService.GetNotCompletedReservations(guest1));
            MakeCommands();

        }

        private void MakeCommands()
        {
            RateOwnerAndAccommodationCommand = new RelayCommand(RateOwnerAndAccommodation_Executed, CanExecute);
            CancelReservationCommand = new RelayCommand(CancelReservation_Executed, CanExecute);
            RescheduleReservationCommand = new RelayCommand(RescheduleReservation_Executed, CanExecute);
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


        private void RateOwnerAndAccommodation_Executed(object sender)
        {
            OwnerReviewService ownerReviewService = new OwnerReviewService();
            if (ownerReviewService.HasReview(SelectedCompletedReservation))
            {
                Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("This reservation is already reviewed.", "/Resources/Images/exclamation.png");
                messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
                messageBox.ShowDialog();
                return;
            }
            if (!ownerReviewService.IsReservationValidToReview(SelectedCompletedReservation))
            {
                Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("You can't rate this reservation because 5 days have passed since its departure.", "/Resources/Images/exclamation.png");
                messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
                messageBox.ShowDialog();
                return;
            }
            ShowReviewForm();
        }

        private void ShowReviewForm()
        {
            OwnerAndAccommodationReviewFormView ownerAndAccommodationReviewForm = new OwnerAndAccommodationReviewFormView(guest1, SelectedCompletedReservation);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = ownerAndAccommodationReviewForm;

        }

        private async void CancelReservation_Executed(object sender)
        {
            if(cancelledAccommodationReservationService.HasReservationStarted(SelectedNotCompletedReservation))
            {
                Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("You can't cancel this reservation because it has already started.", "/Resources/Images/exclamation.png");
                messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
                messageBox.ShowDialog();
                return;
            }

            if (!cancelledAccommodationReservationService.IsCancellationAllowed(SelectedNotCompletedReservation))
            {
                Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("You can't cancel this reservation because the cancellation period has expired.", "/Resources/Images/exclamation.png");
                messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
                messageBox.ShowDialog();
                return;
            }
            
            Task<bool> result = cancelledAccommodationReservationService.ConfirmCancellationMessageBox();
            bool IsYesClicked = await result;
            if (IsYesClicked)
                ConfirmCancellation();
            


        }
        private void ConfirmCancellation()
        {
            cancelledAccommodationReservationService.Add(SelectedNotCompletedReservation);
            accommodationReservationService.Delete(SelectedNotCompletedReservation);
            NotCompletedReservations.Remove(SelectedNotCompletedReservation);

        }
        private void RescheduleReservation_Executed(object sender)
        {
            if (cancelledAccommodationReservationService.HasReservationStarted(SelectedNotCompletedReservation))
            {
                Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("You can't reschedule this reservation because it has already started.", "/Resources/Images/exclamation.png");
                messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
                messageBox.ShowDialog();
            }
            else
            {
                ReschedulingAccommodationReservationFormView form = new ReschedulingAccommodationReservationFormView(SelectedNotCompletedReservation);
                form.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
                form.ShowDialog();
            }
                
        }

    }
}
