using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Service;

namespace InitialProject.WPF.ViewModels
{
    public class AccommodationViewModel
    {
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        private Owner profileOwner;
        private AccommodationService accommodationService;
        public AccommodationViewModel(Owner owner)
        {
            profileOwner = owner;
            accommodationService = new AccommodationService();
            Accommodations = accommodationService.GetAllByOwner(profileOwner);
        }

    }
}
