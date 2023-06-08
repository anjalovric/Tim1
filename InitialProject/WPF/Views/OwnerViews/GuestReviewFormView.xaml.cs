using DotLiquid.Tags;
using InitialProject.Model;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using InitialProject.WPF.Views.Guest1Views;
using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;
using Xceed.Wpf.Toolkit;

namespace InitialProject.WPF.Views.OwnerViews
{
    /// <summary>
    /// Interaction logic for GuestReviewFormView.xaml
    /// </summary>
    public partial class GuestReviewFormView : Page
    {
        public GuestReviewFormViewModel ViewModel { get; set; }
        private string fullText = "";
        private int currentIndex = 0;
        private int increment;
        public GuestReviewFormView(AccommodationReservation reservation)
        {
            InitializeComponent();
            ViewModel = new GuestReviewFormViewModel(reservation);
            DataContext = ViewModel;
        }

        public void InputCommentInDemo()
        {
            fullText = "Great guest!";
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(500);
            dispatcherTimer.Tick += new EventHandler(Timer_Tick);
            dispatcherTimer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            increment++;
            if (currentIndex < fullText.Length)
            {
                currentIndex++;
                Comment.Dispatcher.Invoke(() =>
                {
                    Comment.Text = fullText.Substring(0, currentIndex);
                });
            }
        }
    }
}
