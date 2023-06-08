using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using InitialProject.WPF.Views;
using Timer = System.Threading.Timer;

namespace InitialProject.WPF.Demo
{
    public class AccommodationInputDemo : INotifyPropertyChanged
    {
        private AccommodationInputFormViewModel viewModel;
        private AccommodationInputFormView accommodationInputFormView;
        private int increment = -1;
        private Accommodation accommodation;
        private AccommodationView accommodationView;
        private AccommodationViewModel accommodationViewModel;
        public AccommodationInputDemo()
        {
            OwnerService ownerService = new OwnerService();
            Owner owner = ownerService.GetAll()[0];
            accommodation = new Accommodation();
            accommodationInputFormView = new AccommodationInputFormView(owner);
            viewModel = accommodationInputFormView.formViewModel;
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = accommodationInputFormView;
        }

        public event PropertyChangedEventHandler? PropertyChanged;


        public void PlayDemo()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += new EventHandler(Timer_Tick);
            dispatcherTimer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            AccommodationImageService imageService = new AccommodationImageService();
            Increment++;

            if (Increment == 1)
            {
                accommodation.Name = "My accommodation";
                accommodationInputFormView.InputName();
            }
            if (Increment == 5)
            {
                accommodationInputFormView.SelectCountry();
                viewModel.IsCityComboBoxEnabled = true;
                viewModel.EnableCityCommand.Execute(null);
                accommodation.Location = new Location();
                accommodation.Location.Country = "Turkey";
            }
            if (Increment == 7)
            {
                accommodationInputFormView.SelectCity();
                accommodation.Location.City = "Istanbul";
            }
            if (Increment == 8)
            {
                viewModel.Type = viewModel.AccommodationTypes[0];
                accommodation.Type = viewModel.Type;
            }
            if (Increment == 10)
            {
                viewModel.MinDaysForReservation = 2;
                accommodation.MinDaysForReservation = 2;
            }
            if (Increment == 12)
            {
                viewModel.MinDaysToCancel = 2;
                accommodation.MinDaysToCancel = 2;
            }
            if (Increment == 14)
            {
                accommodationInputFormView.PressAddImageButton();
            }
            if (Increment == 15)
            {
                accommodationInputFormView.LetAddImageButton();
                viewModel.Images.Add(imageService.GetAll()[0]);
                viewModel.ImageUrl = imageService.GetAll()[0].Url;
                accommodation.CoverImage = viewModel.Images[0];
            }
            if(Increment == 16)
            {
                accommodationInputFormView.PressConfirmButton();
            }
            if(Increment == 18)
            {
                accommodationView = new AccommodationView(viewModel.owner);
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = accommodationView;
                accommodationViewModel = accommodationView.ViewModel;
                accommodationViewModel.Accommodations.Add(accommodation);
                accommodationViewModel.StackPanelMessage = "New accommodation successfully added!";
                accommodationViewModel.StackPanelVisibility = "Visible";
            }
            if(Increment ==20)
            {
                accommodationViewModel.IsOkPressedInDemo = true;
            }
            if(Increment == 22)
            {
                accommodationViewModel.IsOkPressedInDemo = false;
                accommodationViewModel.StackPanelVisibility = "Hidden";
            }
            if(Increment == 24)
            {
                accommodationViewModel.Accommodations.Remove(accommodation);
            }
        }

        public int Increment
        {
            get { return increment; }
            set
            {
                if (value != increment)
                {
                    increment = value;
                    OnPropertyChanged();
                }
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
}
