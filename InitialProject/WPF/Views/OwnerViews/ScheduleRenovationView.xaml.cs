using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InitialProject.Model;
using InitialProject.WPF.ViewModels.OwnerViewModels;

namespace InitialProject.WPF.Views.OwnerViews
{
    /// <summary>
    /// Interaction logic for ScheduleRenovationView.xaml
    /// </summary>
    public partial class ScheduleRenovationView : Page
    {
        public ScheduleRenovationViewModel ViewModel { get; set; }
        private Timer timer;
        private string fullText = "";
        private int currentIndex = 0;
        public ScheduleRenovationView(Owner owner)
        {
            InitializeComponent();
            ViewModel = new ScheduleRenovationViewModel(owner);
            this.DataContext = ViewModel;
        }

        public void InputDescription()
        {
            fullText = "New renovation";
            timer = new Timer(TimerLetters, null, 0, 500);
        }
        private void TimerLetters(object state)
        {
            if (currentIndex < fullText.Length)
            {
                currentIndex++;
                Description.Dispatcher.Invoke(() =>
                {
                    Description.Text = fullText.Substring(0, currentIndex);
                });
            }
            else
            {
                timer.Dispose(); // Stop the timer when the entire text has been displayed
            }
        }

        public void PressConfirm()
        {
            ConfirmButton.IsEnabled = true;
            ConfirmButton.Background = new SolidColorBrush(Colors.AliceBlue);
        }
    }
}
