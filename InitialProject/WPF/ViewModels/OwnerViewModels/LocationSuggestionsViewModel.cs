using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class LocationSuggestionsViewModel
    {
        public Owner Owner { get; set; }
        public LocationSuggestionsViewModel(Owner owner)
        {
            Owner = owner;
        }
    }
}
