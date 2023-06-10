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
using InitialProject.WPF.Views.OwnerViews;
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
        private bool showDemoMessage;
        private DemoIsOnView demoIsOnView;
        private DemoIsOffView demoIsOffView;
        public AccommodationInputDemo(bool showDemoMessage)
        {
            this.showDemoMessage = showDemoMessage;
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
            if(Increment == 1)
            {
                if(showDemoMessage)
                {
                    demoIsOnView = new DemoIsOnView();
                    demoIsOnView.Show();
                }
            }
            if (Increment == 3)
            {
                if (showDemoMessage)
                    demoIsOnView.Close();
                accommodation.Name = "My accommodation";
                accommodationInputFormView.InputName();
            }
            if (Increment == 7)
            {
                accommodationInputFormView.SelectCountry();
                viewModel.IsCityComboBoxEnabled = true;
                viewModel.EnableCityCommand.Execute(null);
                accommodation.Location = new Location();
                accommodation.Location.Country = "Turkey";
            }
            if (Increment == 9)
            {
                accommodationInputFormView.SelectCity();
                accommodation.Location.City = "Istanbul";
            }
            if (Increment == 10)
            {
                viewModel.Type = viewModel.AccommodationTypes[0];
                accommodation.Type = viewModel.Type;
            }
            if (Increment == 12)
            {
                viewModel.MinDaysForReservation = 2;
                accommodation.MinDaysForReservation = 2;
            }
            if (Increment == 14)
            {
                viewModel.MinDaysToCancel = 2;
                accommodation.MinDaysToCancel = 2;
            }
            if (Increment == 16)
            {
                accommodationInputFormView.PressAddImageButton();
            }
            if (Increment == 17)
            {
                accommodationInputFormView.LetAddImageButton();
                viewModel.Images.Add(imageService.GetAll()[0]);
                viewModel.ImageUrl = imageService.GetAll()[0].Url;
                accommodation.CoverImage = viewModel.Images[0];
                viewModel.IsDemoOn = true;
            }
            if(Increment == 18)
            {
                accommodationInputFormView.PressConfirmButton();
            }
            if(Increment == 20)
            {
                accommodationView = new AccommodationView(viewModel.owner);
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = accommodationView;
                accommodationViewModel = accommodationView.ViewModel;
                accommodationViewModel.Accommodations.Add(accommodation);
                accommodationViewModel.StackPanelMessage = "New accommodation successfully added!";
                accommodationViewModel.StackPanelVisibility = "Visible";
            }
            if(Increment ==22)
            {
                accommodationViewModel.IsOkPressedInDemo = true;
            }
            if(Increment == 24)
            {
                accommodationViewModel.IsOkPressedInDemo = false;
                accommodationViewModel.StackPanelVisibility = "Hidden";
            }
            if(Increment == 26)
            {
                demoIsOffView = new DemoIsOffView();
                demoIsOffView.Show();
                accommodationViewModel.Accommodations.Remove(accommodation);
            }
            if(Increment == 28)
            {
                demoIsOffView.Close();
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
