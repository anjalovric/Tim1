using InitialProject.Model;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using System.Threading;
using System.Windows.Controls;

namespace InitialProject.WPF.Views.OwnerViews
{
    /// <summary>
    /// Interaction logic for DecliningRequestView.xaml
    /// </summary>
    public partial class DecliningRequestView : Page
    {
        public DecliningRequestViewModel decliningRequestViewModel;
        private Timer timer;
        private string fullText = "";
        private int currentIndex = 0;
        public DecliningRequestView(ReschedulingAccommodationRequest request)
        {
            InitializeComponent();
            decliningRequestViewModel = new DecliningRequestViewModel(request);
            DataContext = decliningRequestViewModel;
        }

        public void InputExplanation()
        {
            fullText = "Not free";
            timer = new Timer(TimerLetters, null, 0, 500);
        }
        private void TimerLetters(object state)
        {
            if (currentIndex < fullText.Length)
            {
                currentIndex++;
                ExplanationTextBox.Dispatcher.Invoke(() =>
                {
                    ExplanationTextBox.Text = fullText.Substring(0, currentIndex);
                });
            }
            else
            {
                timer.Dispose();
            }
        }
    }
}
