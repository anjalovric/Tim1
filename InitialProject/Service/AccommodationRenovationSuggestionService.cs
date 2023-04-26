using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain;
using InitialProject.Model;
using InitialProject.Domain.Model;
using Org.BouncyCastle.Asn1.Ocsp;

namespace InitialProject.Service
{
    public class AccommodationRenovationSuggestionService
    {
        private IAccommodationRenovationSuggestionRepository renovationSuggestionRepository = Injector.CreateInstance<IAccommodationRenovationSuggestionRepository>();
        private List<AccommodationRenovationSuggestion> suggestions;
        public AccommodationRenovationSuggestionService()
        {
            MakeSuggestions();
        }

        public List<AccommodationRenovationSuggestion> GetAll()
        {
            return suggestions;
        }

        public List<AccommodationRenovationSuggestion> GetByAccommodation(Accommodation accommodation)
        {
            List<AccommodationRenovationSuggestion> suggestionsByAccommodation = new List<AccommodationRenovationSuggestion>();
            foreach(var suggestion in suggestions)
            {
                if (suggestion.Reservation.Accommodation.Id == accommodation.Id)
                    suggestionsByAccommodation.Add(suggestion);
            }
            return suggestionsByAccommodation;
        }

        public AccommodationRenovationSuggestion GetById(int id)
        {
            return renovationSuggestionRepository.GetById(id);
        }
        public void Add(AccommodationRenovationSuggestion suggestion)
        {
            renovationSuggestionRepository.Add(suggestion);
        }

        private void MakeSuggestions()
        {
            suggestions = new List<AccommodationRenovationSuggestion>(renovationSuggestionRepository.GetAll());
            SetReservations();
        }
        private void SetReservations()
        {
            AccommodationReservationService accommodationReservationService = new AccommodationReservationService();
            List<AccommodationReservation> storedReservations = accommodationReservationService.GetAll();
            foreach (AccommodationRenovationSuggestion suggestion in suggestions)
                suggestion.Reservation = storedReservations.Find(n => n.Id == suggestion.Reservation.Id);
        }
    }
}
//napuniti rezervacije