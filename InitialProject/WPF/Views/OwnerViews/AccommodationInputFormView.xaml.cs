using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using InitialProject.Model;
using InitialProject.WPF.ViewModels;
using InitialProject.WPF.ViewModels.OwnerViewModels;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for AccommodationInputFormView.xaml
    /// </summary>
    public partial class AccommodationInputFormView : Page
    {
        public AccommodationInputFormViewModel formViewModel { get; set; }
        private Owner owner;
        private Timer timer;
        private string fullText = "";
        private int currentIndex = 0;
        public AccommodationInputFormView(Owner owner)
        {
            InitializeComponent();
            this.owner = owner;
            formViewModel = new AccommodationInputFormViewModel(owner);
            DataContext = formViewModel;
        }

        public void InputName()
        {
            fullText = "House";
            timer = new Timer(TimerLetters, null, 0, 500);
        }
        private void TimerLetters(object state)
        {
            if (currentIndex < fullText.Length)
            {
                currentIndex++;
                Name.Dispatcher.Invoke(() =>
                 {
                     Name.Text = fullText.Substring(0, currentIndex);
                 });
            }
            else
            {
                timer.Dispose();
            }
        }

        public void PressAddImageButton()
        {
            AddImageButton.Background = new SolidColorBrush(Colors.AliceBlue);
        }
        public void LetAddImageButton()
        {
            AddImageButton.Background = new SolidColorBrush(Colors.LightGray);
        }

        public void PressConfirmButton()
        {
            OkButton.Background = new SolidColorBrush(Colors.AliceBlue);
        }
        public void SelectCountry()
        {
            ComboBoxCountry.SelectedItem = "Turkey";
        }
        public void SelectCity()
        {
            ComboBoxCity.SelectedItem = "Istanbul";
        }
    }
}
