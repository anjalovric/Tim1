﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.OwnerViews;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class AccommodationViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        private Owner profileOwner;
        private AccommodationService accommodationService;
        public RelayCommand NewAccommodationCommand { get; set; }
        public RelayCommand ViewImagesCommand { get; set; }
        public RelayCommand OKCommand { get; set; }
        private Accommodation selectedAccommodation;
        private string stackPanelVisibility;
        private string stackPanelMessage;

        public event PropertyChangedEventHandler? PropertyChanged;

        public AccommodationViewModel(Owner owner)
        {
            profileOwner = owner;
            accommodationService = new AccommodationService();
            Accommodations = new ObservableCollection<Accommodation>(accommodationService.GetAllByOwner(profileOwner));
            InitializeSelectedAccommodation();
            MakeCommands();
            DisplayNotificationPanel();

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
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void InitializeSelectedAccommodation()
        {
            selectedAccommodation = new Accommodation();
            if (Accommodations.Count > 0)
                selectedAccommodation = Accommodations[0];
        }

        private void MakeCommands()
        {
            NewAccommodationCommand = new RelayCommand(NewAccommodation_Executed, CanExecute);
            ViewImagesCommand = new RelayCommand(ViewImages_Executed, CanExecute);
            OKCommand = new RelayCommand(OK_Executed, CanExecute);
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

        private void ViewImages_Executed(object sender)
        {
            if (SelectedAccommodation != null)
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = new AccommodationImagesView(SelectedAccommodation);
        }

        private void OK_Executed(object sender)
        {
            StackPanelVisibility = "Hidden";
        }

        private void DisplayNotificationPanel()
        {
            OwnerNotificationsService notificationsService = new OwnerNotificationsService();
            if(notificationsService.IsAccommodationAdded())
            {
                StackPanelMessage = "New accommodation successfully added!";
                StackPanelVisibility = "Visible";
                notificationsService.Delete(Domain.Model.OwnerNotificationType.ACCOMMODATION_ADDED);
            }
            else
            {
                StackPanelVisibility = "Hidden";
            }
        }
    }
}
