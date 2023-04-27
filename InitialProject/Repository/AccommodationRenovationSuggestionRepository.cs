using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Repository
{
    public class AccommodationRenovationSuggestionRepository : IAccommodationRenovationSuggestionRepository
    {
        private const string FilePath = "../../../Resources/Data/renovationSuggestions.csv";

        private readonly Serializer<AccommodationRenovationSuggestion> _serializer;

        private List<AccommodationRenovationSuggestion> _suggestions;

        public AccommodationRenovationSuggestionRepository()
        {
            _serializer = new Serializer<AccommodationRenovationSuggestion>();
            _suggestions = _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            _suggestions = _serializer.FromCSV(FilePath);
            if (_suggestions.Count < 1)
            {
                return 1;
            }
            return _suggestions.Max(c => c.Id) + 1;
        }

        public void Add(AccommodationRenovationSuggestion suggestion)
        {
            suggestion.Id = NextId();
            _suggestions.Add(suggestion);
            _serializer.ToCSV(FilePath, _suggestions);
        }

        public List<AccommodationRenovationSuggestion> GetAll()
        {
            return _suggestions;
        }


        public AccommodationRenovationSuggestion GetById(int id)
        {
            return _suggestions.Find(n => n.Id == id);
        }

        public void Delete(AccommodationRenovationSuggestion suggestion)
        {
            _suggestions = _serializer.FromCSV(FilePath);
            AccommodationRenovationSuggestion founded = _suggestions.Find(c => c.Id == suggestion.Id);
            _suggestions.Remove(founded);
            _serializer.ToCSV(FilePath, _suggestions);
        }
    }
}
