using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using InitialProject.Model;
using InitialProject.Service;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class RequestForReshcedulingViewModel : INotifyPropertyChanged
    {
        private ReschedulingAccommodationRequest request;
        private AccommodationReservation accommodationReservation;
        private bool isAccommodationAvailable;
        private string availability;
        private Owner profileOwner;
        public RequestForReshcedulingViewModel(Owner owner)
        {
            Request = new ReschedulingAccommodationRequest();
            profileOwner = owner;
            //SetAvailability();
        }

        public ReschedulingAccommodationRequest Request
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

        public string Availability
        {
            get => availability;
            set
            {
                if (!value.Equals(availability))
                {
                    availability = value;
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

        public void SetAvailability()
        {
            AvailableDatesForAccommodationService datesService = new AvailableDatesForAccommodationService();
            isAccommodationAvailable = datesService.IsAvailableInDateRange(Request.Reservation, Request.NewArrivalDate, Request.NewDepartureDate);
            if (isAccommodationAvailable)
            {
                Availability = "available";
            }
            else
            {
                Availability = "not available";
            }
        }
    }
}
