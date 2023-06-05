using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using InitialProject.APPLICATION.UseCases;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.OwnerViews;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class LocationSuggestionsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<MostPopularLocation> MostPopularLocations { get; set; }
        public ObservableCollection<LeastPopularLocation> LeastPopularAccommodations { get; set; }
        private LocationSuggestionsService locationSuggestionsService;
        public Owner Owner { get; set; }
        public RelayCommand RemoveCommand { get; set; }
        public RelayCommand NewAccommodationCommand { get; set; }
        private MostPopularLocation selectedLocation;
        private LeastPopularLocation selectedAccommodation;
        public event PropertyChangedEventHandler? PropertyChanged;
        public LocationSuggestionsViewModel(Owner owner)
        {
            Owner = owner;
            locationSuggestionsService = new LocationSuggestionsService();
            MakeObservableCollections();
            RemoveCommand = new RelayCommand(Remove_Executed, CanExecute);
            NewAccommodationCommand = new RelayCommand(NewAccommodation_Executed, CanExecute);
        }

        private void MakeObservableCollections()
        {
            MostPopularLocations = new ObservableCollection<MostPopularLocation>(locationSuggestionsService.GetMostPopularLocations(Owner));
            LeastPopularAccommodations = new ObservableCollection<LeastPopularLocation>(locationSuggestionsService.GetLeastPopularAccommodations(Owner));
            InitializeSelectedItems();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void InitializeSelectedItems()
        {
            if(MostPopularLocations.Count() >0)
                SelectedLocation = MostPopularLocations[0];
            if (LeastPopularAccommodations.Count() > 0)
                SelectedAccommodation = LeastPopularAccommodations[0];
        }
        public LeastPopularLocation SelectedAccommodation
        {
            get { return selectedAccommodation; }
            set
            {
                if (value != selectedAccommodation)
                {
                    selectedAccommodation = value;
                    OnPropertyChanged();
                }
            }
        }

        public MostPopularLocation SelectedLocation
        {
            get { return selectedLocation; }
            set
            {
                if (value != selectedLocation)
                {
                    selectedLocation = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool CanExecute(object sender)
        {
            return true;
        }

        private void Remove_Executed(object sender)
        {
            if (SelectedAccommodation != null)
            {
                DeletingAccommodationView deletingView = new DeletingAccommodationView(SelectedAccommodation.Accommodation);
                deletingView.Show();
            }
        }
        private void NewAccommodation_Executed(object sender)
        {
            if(SelectedLocation != null)
            {
                AccommodationInputFormView accommodationInputFormView = new AccommodationInputFormView(Owner);
                accommodationInputFormView.formViewModel.Location.Country = SelectedLocation.Location.Country;
                accommodationInputFormView.formViewModel.EnableCityCommand.Execute(null);
                foreach(var city in accommodationInputFormView.formViewModel.CitiesByCountry)
                {
                    if(city.Equals(SelectedLocation.Location.City))
                        accommodationInputFormView.formViewModel.Location.City = city;
                }
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = accommodationInputFormView;
            }
        }
    }
}
