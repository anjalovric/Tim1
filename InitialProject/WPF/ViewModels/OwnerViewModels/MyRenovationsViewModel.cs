using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.OwnerViews;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class MyRenovationsViewModel : INotifyPropertyChanged
    {
        private Owner owner;
        private AccommodationRenovationService renovationService;
        private int upcomingRenovations;
        private int renovatedObjects;
        private AccommodationRenovation selectedRenovation;
        private AccommodationRenovation renovationForDetails;

        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<AccommodationRenovation> Renovations { get; set; }
        public RelayCommand NewRenovationCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand RenovationDetailsCommand { get; set; }
        public MyRenovationsViewModel(Owner owner)
        {
            this.owner = owner;
            renovationService = new AccommodationRenovationService();
            Renovations = new ObservableCollection<AccommodationRenovation>(renovationService.GetAllByOwner(owner));
            NewRenovationCommand = new RelayCommand(NewRenovation_Executed, CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed, CanExecute);
            RenovationDetailsCommand = new RelayCommand(RenovationDetails_Executed, CanExecute);    
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private bool CanExecute(object sender)
        {
            return true;
        }

        private void NewRenovation_Executed(object sender)
        {
            ScheduleRenovationView scheduleRenovationView = new ScheduleRenovationView(owner);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = scheduleRenovationView;
        }

        private void Cancel_Executed(object sender)
        {
            selectedRenovation = sender as AccommodationRenovation;
            Renovations.Remove(selectedRenovation);
            renovationService.Delete(selectedRenovation);
            UpdateRenovationsNumber();
        }
        private void RenovationDetails_Executed(object sender)
        {
            RenovationDetailsView renovationDetailsView = new RenovationDetailsView(SelectedRenovation);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = renovationDetailsView;
        }
        public int UpcomingRenovations
        {
            get => renovationService.CountUpcomingRenovations(owner);
            set
            {
                if (value != upcomingRenovations)
                {
                    upcomingRenovations = value;
                    OnPropertyChanged();
                }
            }
        }

        public int RenovatedObjects
        {
            get => renovationService.CountRenovatedObjects(owner);
            set
            {
                if (value != renovatedObjects)
                {
                    renovatedObjects = value;
                    OnPropertyChanged();
                }
            }
        }

        public AccommodationRenovation SelectedRenovation
        {
            get => renovationForDetails;
            set
            {
                if (value != renovationForDetails)
                {
                    renovationForDetails = value;
                    OnPropertyChanged();
                }
            }
        }
        private void UpdateRenovationsNumber()
        {
            UpcomingRenovations = renovationService.CountUpcomingRenovations(owner);
            RenovatedObjects = renovationService.CountRenovatedObjects(owner);
        }
    }
}
