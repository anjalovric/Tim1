using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.WPF.Views.OwnerViews;
using InitialProject.WPF.Views;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class DecliningRequestViewModel : INotifyPropertyChanged
    {
        public ReschedulingAccommodationRequest ReschedulingAccommodationRequest { get; set; }
        private string explanation { get; set; }
        public RelayCommand ConfirmCommand { get; set; }
        public DecliningRequestViewModel(ReschedulingAccommodationRequest request)
        {
            ReschedulingAccommodationRequest = request;
            ConfirmCommand = new RelayCommand(Confirm_Executed, CanExecute);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool CanExecute(object sender)
        {
            return true;
        }

        private void Confirm_Executed(object sender)
        {
            ReservationReschedulingView reservationReschedulingView = new ReservationReschedulingView(ReschedulingAccommodationRequest.Reservation.Accommodation.Owner);
            reservationReschedulingView.reservationReschedulingViewModel.SaveDeclinedRequest(Explanation);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = reservationReschedulingView;
        }

        public string Explanation

        {
            get => explanation;
            set
            {
                if (!value.Equals(explanation))
                {
                    explanation = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
