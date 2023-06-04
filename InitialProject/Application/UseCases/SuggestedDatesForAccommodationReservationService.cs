using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.WPF.Views.Guest1Views;

namespace InitialProject.Service
{
    public class SuggestedDatesForAccommodationReservationService
    {
        private List<AccommodationReservation> reservations;
        private List<DateTime> availableDates;
        private List<DateTime> availableDatesHelp;
        private List<List<DateTime>> availableDateRanges;
        private int NumberOfDays;
        private int NumberOfGuests;
        private DateTime Arrival;
        private DateTime Departure;
        private TimeSpan lengthOfStay;
        AccommodationReservationService accommodationReservationService;
        public SuggestedDatesForAccommodationReservationService()
        {
            accommodationReservationService = new AccommodationReservationService();
            reservations = accommodationReservationService.GetAll();
        }
        private bool IsDateAvailable(int currentAccommodationId, DateTime date)
        {
            foreach (AccommodationReservation reservation in reservations)  //if reserved on Date "date"
            {
                if (currentAccommodationId == reservation.Accommodation.Id)
                {
                    if (date >= reservation.Arrival && date <= reservation.Departure)
                        return false;
                }
            }
            AccommodationRenovationService accommodationRenovationService = new AccommodationRenovationService();
            return accommodationRenovationService.IsDateAvailableForReservation(currentAccommodationId, date);

        }
        private void AddAvailableDateToList(DateTime date)
        {
            availableDates.Add(date);
            availableDatesHelp.Add(date);
            if (availableDates.Count == NumberOfDays)
            {
                availableDateRanges.Add(availableDatesHelp);
                availableDates.Remove(availableDates[0]);
                availableDatesHelp = new List<DateTime>(availableDates);
            }
        }
        private void AddAvailableDateOutRangeToList(DateTime date)
        {
            availableDates.Add(date);
            availableDatesHelp.Add(date);
            if (availableDates.Count == NumberOfDays)
            {
                if (AreAvailableDatesConsecutive(availableDates))
                    availableDateRanges.Add(availableDatesHelp);

                availableDates.Remove(availableDates[0]);
                availableDatesHelp = new List<DateTime>(availableDates);
            }
        }
        private bool AreAvailableDatesConsecutive(List<DateTime> dates)
        {
            for (int i = 0; i < NumberOfDays - 1; i++)
            {
                if (dates[i + 1].Subtract(dates[i]).TotalDays > 1)
                {
                    return false;
                }
            }
            return true;
        }
        public void TakeInputParameters(DateTime Arrival, DateTime Departure, int NumberOfDays, int NumberOfGuests)
        {
            this.Arrival = Arrival;
            this.Departure = Departure;
            this.NumberOfDays = NumberOfDays;
            this.NumberOfGuests = NumberOfGuests;
        }
        public List<AvailableDatesForAccommodation> GetAvailableDates(Accommodation currentAccommodation, Guest1 guest1)
        {
            InitializeAvailableDatesLists();
            lengthOfStay = Departure.Subtract(Arrival);
            FillDateRangesList(currentAccommodation.Id);
            List<AvailableDatesForAccommodation> availableDatesForAccommodations = new List<AvailableDatesForAccommodation>();
            if (availableDateRanges.Count > 0 && AvailableDateRangeExists(ref availableDatesForAccommodations))
                return availableDatesForAccommodations;
            
            else
            {
                FindAvailableDatesOutRange(currentAccommodation.Id);
                return GetAvailableDatesOutRange(currentAccommodation, guest1);
            }
        }
        private void FillDateRangesList(int currentAccommodationId)
        {
            DateTime start = Arrival;
            for (int i = 0; i <= lengthOfStay.TotalDays; i++)
            {
                if (IsDateAvailable(currentAccommodationId, start))
                    AddAvailableDateToList(start);
                start = start.AddDays(1);
            }
        }
        private bool AvailableDateRangeExists(ref List<AvailableDatesForAccommodation> availableDatesForAccommodations)
        {
            bool existed = false;
            foreach (List<DateTime> dates in availableDateRanges)
            {
                if (AreAvailableDatesConsecutive(dates))
                {
                    existed = true;
                    DateTime arrival = dates[0];
                    DateTime departure = dates[NumberOfDays - 1];
                    AvailableDatesForAccommodation newAvailableDate = new AvailableDatesForAccommodation(arrival, departure);
                    availableDatesForAccommodations.Add(newAvailableDate);
                }
            }
            return existed;
        }
        private void FindAvailableDatesOutRange(int currentAccommodationId)
        {
            InitializeAvailableDatesLists();
            DateTime start = Departure;
            while (IsDateAvailable(currentAccommodationId, start))
                start = start.AddDays(-1);
            start = start.AddDays(1);
            FillDatesOutRangeList(ref start, currentAccommodationId);
        }
        private void FillDatesOutRangeList(ref DateTime start, int currentAccommodationId)
        {
            while (availableDateRanges.Count < 3)
            {
                if (IsDateAvailable(currentAccommodationId, start))
                    AddAvailableDateOutRangeToList(start);
                start = start.AddDays(1);
            }
        }
        private List<AvailableDatesForAccommodation> GetAvailableDatesOutRange(Accommodation currentAccommodation, Guest1 guest1)
        {
            List<AvailableDatesForAccommodation> availableDatesForAccommodations = new List<AvailableDatesForAccommodation>();
            foreach (List<DateTime> dates in availableDateRanges)
            {
                DateTime arrival = dates[0];
                DateTime departure = dates[NumberOfDays - 1];
                AvailableDatesForAccommodation newAvailableDate = new AvailableDatesForAccommodation(arrival, departure);
                availableDatesForAccommodations.Add(newAvailableDate);
            }
            return availableDatesForAccommodations;
        }
        private void InitializeAvailableDatesLists()
        {
            availableDates = new List<DateTime>();
            availableDatesHelp = new List<DateTime>();
            availableDateRanges = new List<List<DateTime>>();
        }
        public List<AvailableDatesForAccommodation> GetAvailableDatesForAnywhereSearch(Accommodation currentAccommodation)
        {
            reservations = accommodationReservationService.GetAll();
            InitializeAvailableDatesLists();
            lengthOfStay = Departure.Subtract(Arrival);
            FillDateRangesList(currentAccommodation.Id);
            List<AvailableDatesForAccommodation> availableDatesForAccommodations = new List<AvailableDatesForAccommodation>();
            if (availableDateRanges.Count > 0 && AvailableDateRangeExists(ref availableDatesForAccommodations))
                return availableDatesForAccommodations;
            return null;
        }

        public List<AvailableDatesForAccommodation> GetAvailableDatesForAnywhereSearchNoInputDates(Accommodation currentAccommodation, int NumberOfDays, int NumberOfGuests)
        {
            this.NumberOfDays = NumberOfDays;
            this.NumberOfGuests = NumberOfGuests;
            reservations = accommodationReservationService.GetAll();
            InitializeAvailableDatesLists();
            FindFirstThreeAvailableDateRanges(currentAccommodation.Id);       
            List<AvailableDatesForAccommodation> availableDatesForAccommodations = new List<AvailableDatesForAccommodation>();
            AvailableDateRangeExists(ref availableDatesForAccommodations);
            return availableDatesForAccommodations;
        }

        private void FindFirstThreeAvailableDateRanges(int currentAccommodationId)
        {
            InitializeAvailableDatesLists();
            DateTime start = DateTime.Now.Date.AddDays(1);  //provjeriti jel ovo ponoc sutra
            while (availableDateRanges.Count < 3)
            {
                if (IsDateAvailable(currentAccommodationId, start))
                    AddAvailableDateOutRangeToList(start);
                start = start.AddDays(1);
            }
        }
    }
}
