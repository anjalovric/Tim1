using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using InitialProject.Model;

namespace InitialProject.WPF.ViewModels
{
    public class RequestForReshcedulingViewModel : INotifyPropertyChanged
    {
        private ChangeAccommodationReservationDateRequest request;
        private AccommodationReservation accommodationReservation;
        private bool isAccommodationAvailable;
        public RequestForReshcedulingViewModel()
        {
            Request = new ChangeAccommodationReservationDateRequest();
        }

        public ChangeAccommodationReservationDateRequest Request
        {
            get => request;
            set
            {
                if (!value.Equals(request))
                {
                    request = value;
                    OnPropertyChanged();
                }
            }
        }
        public AccommodationReservation AccommodationReservation
        {
            get => accommodationReservation;
            set
            {
                if (!value.Equals(accommodationReservation))
                {
                    accommodationReservation = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsAccommodationAvailable
        {
            get => isAccommodationAvailable;
            set
            {
                if (!value.Equals(isAccommodationAvailable))
                {
                    isAccommodationAvailable = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
