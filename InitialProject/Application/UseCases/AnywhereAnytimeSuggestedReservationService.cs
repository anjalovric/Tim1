using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Service;

namespace InitialProject.APPLICATION.UseCases
{
    public class AnywhereAnytimeSuggestedReservationService
    {
        AccommodationReservationService accommodationReservationService;
        AccommodationService accommodationService;
        SuggestedDatesForAccommodationReservationService suggestedDatesForAccommodationReservationService;
        public AnywhereAnytimeSuggestedReservationService()
        {
            accommodationReservationService = new AccommodationReservationService();
            accommodationService = new AccommodationService();
            suggestedDatesForAccommodationReservationService = new SuggestedDatesForAccommodationReservationService();
        }
        public List<Tuple<Accommodation, AvailableDatesForAccommodation>> GetAvailableDates(DateTime arrival, DateTime departure, int numberOfDays, int numberOfGuests)
        {
            List<Tuple<Accommodation, AvailableDatesForAccommodation>> retDates = new List<Tuple<Accommodation, AvailableDatesForAccommodation>>();
            List<Accommodation> storedAccommodations = new List<Accommodation>(accommodationService.GetAll());
            suggestedDatesForAccommodationReservationService.TakeInputParameters(arrival, departure, numberOfDays, numberOfGuests);

            foreach (Accommodation accommodation in storedAccommodations)
            {
                if(accommodation.Capacity>=numberOfGuests)
                    FillDateRanges(accommodation, retDates);             
            }
            return retDates;
        }
        private void FillDateRanges(Accommodation accommodation, List<Tuple<Accommodation, AvailableDatesForAccommodation>> retDates)
        {
            List<AvailableDatesForAccommodation> availableDatesForAccommodations = suggestedDatesForAccommodationReservationService.GetAvailableDatesForAnywhereSearch(accommodation);
            if (availableDatesForAccommodations != null)    //if returned null -> no available dates for this accommodation
            {
                foreach (AvailableDatesForAccommodation date in availableDatesForAccommodations)
                    retDates.Add(Tuple.Create(accommodation, date));
            }
        }
        public List<Tuple<Accommodation, AvailableDatesForAccommodation>> GetAvailableDatesNoInputDates(int numberOfDays, int numberOfGuests)
        {
            List<Tuple<Accommodation, AvailableDatesForAccommodation>> retDates = new List<Tuple<Accommodation, AvailableDatesForAccommodation>>();
            List<Accommodation> storedAccommodations = new List<Accommodation>(accommodationService.GetAll());
       
            foreach (Accommodation accommodation in storedAccommodations)
            {
                if (accommodation.Capacity >= numberOfGuests)
                    FillDateRangesNoInputDates(accommodation, retDates, numberOfDays, numberOfGuests);
            }
            return retDates;
        }
        private void FillDateRangesNoInputDates(Accommodation accommodation, List<Tuple<Accommodation, AvailableDatesForAccommodation>> retDates, int numberOfDays, int numberOfGuests)
        {
            List<AvailableDatesForAccommodation> availableDatesForAccommodations = suggestedDatesForAccommodationReservationService.GetAvailableDatesForAnywhereSearchNoInputDates(accommodation, numberOfDays, numberOfGuests);
            foreach (AvailableDatesForAccommodation date in availableDatesForAccommodations)
                retDates.Add(Tuple.Create(accommodation, date));
        }
    }
}
