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
        public Owner Owner { get; set; }
        private AccommodationRenovationService renovationService;
        private int upcomingRenovations;
        private int renovatedObjects;
        private AccommodationRenovation selectedRenovation;
        private AccommodationRenovation renovationForDetails;
        private string stackPanelVisibility;
        private string stackPanelMessage;

        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<AccommodationRenovation> Renovations { get; set; }
        public RelayCommand NewRenovationCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand RenovationDetailsCommand { get; set; }
        public RelayCommand OKCommand { get; set; }
        private bool isDemoOn;
        private bool isNewRenovationPressedInDemo;
        public MyRenovationsViewModel(Owner owner)
        {
            this.Owner = owner;
            renovationService = new AccommodationRenovationService();
            Renovations = new ObservableCollection<AccommodationRenovation>(renovationService.GetAllByOwner(owner));
            MakeCommands();
            DisplayNotificationPanel();
        }

        private void MakeCommands()
        {
            NewRenovationCommand = new RelayCommand(NewRenovation_Executed, CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed, CanExecute);
            RenovationDetailsCommand = new RelayCommand(RenovationDetails_Executed, CanExecute);
            OKCommand = new RelayCommand(OK_Executed, CanExecute);
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
            ScheduleRenovationView scheduleRenovationView = new ScheduleRenovationView(Owner);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = scheduleRenovationView;
        }

        private void Cancel_Executed(object sender)
        {
            selectedRenovation = sender as AccommodationRenovation;
            CancellingRenovationView cancellingRenovationView = new CancellingRenovationView(selectedRenovation);
            cancellingRenovationView.Show();
        }
        private void RenovationDetails_Executed(object sender)
        {
            RenovationDetailsView renovationDetailsView = new RenovationDetailsView(SelectedRenovation);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = renovationDetailsView;
        }
        public int UpcomingRenovations
        {
            get => renovationService.CountUpcomingRenovations(Owner);
            set
            {
                if (value != upcomingRenovations)
                {
                    upcomingRenovations = value;
                    OnPropertyChanged();
                }
            }
        }

        public int UpcomingRenovationsDemo
        {
            get => renovationService.CountUpcomingRenovations(Owner) + 1;
            set
            {
                if (value != upcomingRenovations+1)
                {
                    upcomingRenovations = value -1;
                    OnPropertyChanged();
                }
            }
        }

        public int RenovatedObjects
        {
            get => renovationService.CountRenovatedObjects(Owner);
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

        public string StackPanelMessage
        {
            get { return stackPanelMessage; }
            set
            {
                if (value != stackPanelMessage)
                {
                    stackPanelMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StackPanelVisibility
        {
            get { return stackPanelVisibility; }
            set
            {
                if (value != stackPanelVisibility)
                {
                    stackPanelVisibility = value;
                    OnPropertyChanged();
                }
            }
        }
        private void OK_Executed(object sender)
        {
            StackPanelVisibility = "Hidden";
        }

        private void DisplayNotificationPanel()
        {
            OwnerNotificationsService notificationsService = new OwnerNotificationsService();
            if (notificationsService.IsRenovationScheduled(Owner))
            {
                StackPanelMessage = "New renovation successfully scheduled!";
                StackPanelVisibility = "Visible";
                notificationsService.Delete(OwnerNotificationType.RENOVATION_SCHEDULED, Owner);
            }
            else if (notificationsService.IsRenovationCancelled(Owner))
            {
                StackPanelMessage = "Renovation successfully cancelled!";
                StackPanelVisibility = "Visible";
                notificationsService.Delete(OwnerNotificationType.RENOVATION_CANCELLED, Owner);
            }
            else
            {
                StackPanelVisibility = "Hidden";
            }
        }

        public bool IsDemoOn
        {
            get { return isDemoOn; }
            set
            {
                if (value != isDemoOn)
                {
                    isDemoOn = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsNewRenovationPressedInDemo
        {
            get { return isNewRenovationPressedInDemo; }
            set
            {
                if (value != isNewRenovationPressedInDemo)
                {
                    isNewRenovationPressedInDemo = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
