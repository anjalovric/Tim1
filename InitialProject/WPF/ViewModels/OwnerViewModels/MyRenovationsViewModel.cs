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

        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<AccommodationRenovation> Renovations { get; set; }
        public RelayCommand NewRenovationCommand { get; set; }
        public MyRenovationsViewModel(Owner owner)
        {
            this.owner = owner;
            renovationService = new AccommodationRenovationService();
            Renovations = new ObservableCollection<AccommodationRenovation>(renovationService.GetAllByOwner(owner));
            NewRenovationCommand = new RelayCommand(NewRenovation_Executed, CanExecute);
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
    }
}
