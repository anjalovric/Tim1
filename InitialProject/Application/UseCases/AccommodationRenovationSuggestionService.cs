using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain;
using InitialProject.Model;
using InitialProject.Domain.Model;

namespace InitialProject.Service
{
    public class AccommodationRenovationSuggestionService
    {
        private IAccommodationRenovationSuggestionRepository renovationSuggestionRepository = Injector.CreateInstance<IAccommodationRenovationSuggestionRepository>();
        private AccommodationReservationService accommodationReservationService;
        public AccommodationRenovationSuggestionService()
        {
            accommodationReservationService = new AccommodationReservationService();
        }

        public List<AccommodationRenovationSuggestion> GetAll()
        {
            List<AccommodationRenovationSuggestion> suggestions = new List<AccommodationRenovationSuggestion>(renovationSuggestionRepository.GetAll());
            SetReservations(suggestions);
            return suggestions;
        }

        public List<AccommodationRenovationSuggestion> GetByAccommodation(Accommodation accommodation)
        {
            List<AccommodationRenovationSuggestion> suggestions = new List<AccommodationRenovationSuggestion>(renovationSuggestionRepository.GetAll());
            SetReservations(suggestions);
            List<AccommodationRenovationSuggestion> suggestionsByAccommodation = new List<AccommodationRenovationSuggestion>();
            foreach (var suggestion in suggestions)
            {
                if (suggestion.Reservation.Accommodation.Id == accommodation.Id)
                    suggestionsByAccommodation.Add(suggestion);
            }
            return suggestionsByAccommodation;
        }

        private void SetReservations(List<AccommodationRenovationSuggestion> suggestions)
        {
            List<AccommodationReservation> storedReservations = accommodationReservationService.GetAll();
            foreach (AccommodationRenovationSuggestion suggestion in suggestions)
                suggestion.Reservation = storedReservations.Find(n => n.Id == suggestion.Reservation.Id);
        }


        public void Add(AccommodationRenovationSuggestion suggestion)
        {
            renovationSuggestionRepository.Add(suggestion);
        }

        

        public void Delete(AccommodationRenovationSuggestion suggestion)
        {
            renovationSuggestionRepository.Delete(suggestion);
        }
    }
}
