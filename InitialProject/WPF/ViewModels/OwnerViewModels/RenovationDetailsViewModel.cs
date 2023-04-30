using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class RenovationDetailsViewModel
    {
        public AccommodationRenovation Renovation { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set ; }
        public RenovationDetailsViewModel(AccommodationRenovation renovation)
        {
            Renovation = renovation;
            StartDate = DateOnly.FromDateTime(Renovation.StartDate);
            EndDate = DateOnly.FromDateTime(Renovation.EndDate);
        }
    }
}
