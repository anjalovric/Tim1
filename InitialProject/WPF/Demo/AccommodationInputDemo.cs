using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.Linq;
using DotLiquid.Tags;
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
        private Timer timer;
        private string fullText = "";
        private int currentIndex = 0;
        private TextBox textBox;
        public AccommodationInputDemo()
        {
            OwnerService ownerService = new OwnerService();
            Owner owner = ownerService.GetAll()[0];
            accommodationInputFormView = new AccommodationInputFormView(owner);
            viewModel = accommodationInputFormView.formViewModel;
            timer = new Timer(TimerLetters, null, 0, 300);
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
                fullText = "My accommodation";
                currentIndex = 0;
                textBox = (TextBox)Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.FindName("Name");
                timer.Start();
            }

            viewModel.Name = "My accommodation";
            if (Increment == 5)
            {
                viewModel.Location.Country = viewModel.Countries[0];
                viewModel.IsCityComboBoxEnabled = true;
                viewModel.EnableCityCommand.Execute(null);
            }
            if (Increment == 7)
                viewModel.Location.City = viewModel.CitiesByCountry[0];
            if (Increment == 8)
                viewModel.Type = viewModel.AccommodationTypes[0];
            if (Increment == 10)
                viewModel.MinDaysForReservation = 2;
            if (Increment == 12)
                viewModel.MinDaysToCancel = 2;
            if (Increment == 14)
            {
                Button buttonImage = accommodationInputFormView.AddImageButton;
                buttonImage.Foreground = new SolidColorBrush(Colors.Gray);
            }
            if (Increment == 15)
            {
                viewModel.Images.Add(imageService.GetAll()[0]);
                viewModel.ImageUrl = imageService.GetAll()[0].Url;
            }
            if(Increment == 16)
            {
                Button buttonImage = accommodationInputFormView.OkButton;
                buttonImage.SetValue(Button.BackgroundProperty, new SolidColorBrush(Colors.Gray));
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

        private void TimerLetters(object state)
        {
            if (currentIndex < fullText.Length)
            {
                currentIndex++;
                textBox.Dispatcher.Invoke(() =>
                {
                    textBox.Text = fullText.Substring(0, currentIndex);
                });
            }
            else
            {
                timer.Dispose(); // Stop the timer when the entire text has been displayed
            }
        }
    }
}
