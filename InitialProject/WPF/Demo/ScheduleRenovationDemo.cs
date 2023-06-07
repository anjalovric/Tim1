using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.OwnerViews;
using Xceed.Wpf.Toolkit;

namespace InitialProject.WPF.Demo
{
    public class ScheduleRenovationDemo : INotifyPropertyChanged
    {
        private ScheduleRenovationViewModel viewModel;
        private ScheduleRenovationView scheduleRenovationView;
        private int increment = -1;
        private Timer timer;
        private string fullText = "";
        private int currentIndex = 0;
        private WatermarkTextBox textBox;
        private Button button;
        public ScheduleRenovationDemo()
        {
            OwnerService ownerService = new OwnerService();
            Owner owner = ownerService.GetAll()[0];
            scheduleRenovationView = new ScheduleRenovationView(owner);
            viewModel = scheduleRenovationView.ViewModel;
            timer = new Timer(TimerLetters, null, 0, 200);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = scheduleRenovationView;
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
            Increment++;
            if(Increment == 1)
                viewModel.SelectedAccommodation = viewModel.Accommodations[0];
            if(Increment == 3)
                viewModel.StartDate = DateTime.Now.AddDays(1);
            if(Increment == 4)
                viewModel.EndDate = DateTime.Now.AddDays(6);
            if(Increment == 6)
                viewModel.Duration = 2;
            if(Increment == 8)
            {
                viewModel.SelectedDateRange = viewModel.DatesSuggestions[0];
            }
            if(Increment == 9)
            {
                textBox = scheduleRenovationView.Description;
                fullText = "renovation description";
                timer.Start();
            }
            if(Increment == 12)
            {
                button = scheduleRenovationView.ConfirmButton;
                button.IsEnabled = true;
            }
            if(Increment == 13)
                button.Foreground = new SolidColorBrush(Colors.Gray);

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
