using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views;

namespace InitialProject.WPF.ViewModels
{
    public class AccommodationViewModel
    {
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        private Owner profileOwner;
        private AccommodationService accommodationService;

        public RelayCommand NewAccommodationCommand { get; set; }
        public AccommodationViewModel(Owner owner)
        {
            profileOwner = owner;
            accommodationService = new AccommodationService();
            Accommodations = accommodationService.GetAllByOwner(profileOwner);
            NewAccommodationCommand = new RelayCommand(NewAccommodation_Executed, CanExecute);
        }

        private bool CanExecute(object sender)
        {
            return true;
        }

        private void NewAccommodation_Executed(object sender)
        {
            AccommodationInputFormView accommodationInputFormView = new AccommodationInputFormView(profileOwner);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = accommodationInputFormView;
        }

    }
}
