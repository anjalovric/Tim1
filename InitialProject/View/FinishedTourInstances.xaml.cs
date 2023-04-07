using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for FinishedTourInstances.xaml
    /// </summary>
    
    public partial class FinishedTourInstances : UserControl
    {
        private Guest2 guest2;
        private ObservableCollection<TourInstance> completedTours;
        public ObservableCollection<TourInstance> CompletedTours 
        {
            get { return completedTours; }
            set
            {
                if (value != completedTours)
                    completedTours = value;
                OnPropertyChanged("CompletedTours");
            }

        }
        private GuideAndTourReviewService guideAndTourReviewService;
        public FinishedTourInstances(Guest2 guest2)
        {
            CompletedTours = new ObservableCollection<TourInstance>();
            InitializeComponent();
            DataContext = this;
            this.guest2 = guest2;

           // tourInstanceService = new TourInstanceService();
           // tourReservationService = new TourReservationService();
           // tourReservations = new ObservableCollection<TourReservation>(tourReservationService.GetAll());
            guideAndTourReviewService = new GuideAndTourReviewService();
            guideAndTourReviewService.SetTourInstances(CompletedTours, guest2);
           // CompletedTours =guideAndTourReviewService.CompletedTours;

           // guideAndTourReviewService = new GuideAndTourReviewService(guest2);
           // CompletedTours=guideAndTourReviewService.CompletedTours;

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void Rate_Click(object sender, RoutedEventArgs e)
        {
            TourInstance currentTourInstance = (TourInstance)TourListDataGrid.CurrentItem;
            GuideAndTourReviewForm guideAndTourReview = new GuideAndTourReviewForm(currentTourInstance,guest2);
            guideAndTourReview.Show();
        }
    }
}
