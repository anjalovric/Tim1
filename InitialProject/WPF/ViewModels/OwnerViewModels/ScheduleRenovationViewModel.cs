using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.OwnerViews;
using InitialProject.WPF.Views;
using System.Windows;
using InitialProject.Domain.Model;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class ScheduleRenovationViewModel : INotifyPropertyChanged
    {
        private Owner owner;
        private Accommodation selectedAccommodation;
        private DateTime startDate;
        private DateTime endDate;
        private int duration;
        private string description;
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ConfirmCommand { get; set; }
        public ScheduleRenovationViewModel(Owner owner)
        {
            this.owner = owner;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            MakeListOfAccommodation(owner);
            MakeCommands();
        }

        private void MakeCommands()
        {
            CancelCommand = new RelayCommand(Cancel_Executed, CanExecute);
            ConfirmCommand = new RelayCommand(Confirm_Executed, CanExecute);
        }

        private void MakeListOfAccommodation(Owner owner)
        {
            AccommodationService accommodationService = new AccommodationService();
            Accommodations = new ObservableCollection<Accommodation>(accommodationService.GetAllByOwner(owner));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Accommodation SelectedAccommodation
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

        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                if (value != startDate)
                {
                    startDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                if (value != endDate)
                {
                    endDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Duration
        {
            get { return duration; }
            set
            {
                if (value != duration)
                {
                    duration = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (!value.Equals(description))
                {
                    description = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool CanExecute(object sender)
        {
            return true;
        }

        private void Cancel_Executed(object sender)
        {
            MyRenovationsView myRenovationsView = new MyRenovationsView(owner);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = myRenovationsView;
        }

        private void Confirm_Executed(object sender)
        {
            ScheduleRenovation();
            MyRenovationsView myRenovationsView = new MyRenovationsView(owner);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = myRenovationsView;
        }

        private void ScheduleRenovation()
        {
            AccommodationRenovation renovation = new AccommodationRenovation();
            renovation.Accommodation = SelectedAccommodation;
            renovation.StartDate = StartDate;
            renovation.EndDate = EndDate;
            renovation.Description = Description;

            AccommodationRenovationService renovationService = new AccommodationRenovationService();
            renovationService.Add(renovation);
        }
    }
}
