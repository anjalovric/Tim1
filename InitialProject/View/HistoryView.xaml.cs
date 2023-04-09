using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
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
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for HistoryView.xaml
    /// </summary>
    public partial class HistoryView : Window, INotifyPropertyChanged
    {
        public int Year { get; set; }
        public ObservableCollection<TourInstance> Instances { get; set; }

        private TourInstance selected;
        private TourHistoryService historyService;
        public TourInstance Selected
        {
            get { return selected; }
            set
            {
                if (value != selected)
                    selected = value;
                
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public HistoryView(User user)
        {
            InitializeComponent();
            DataContext = this;

            Instances = new ObservableCollection<TourInstance>();

            historyService = new TourHistoryService();

            historyService.SetLocationToTour();
            historyService.SetTourToTourInstance();
            historyService.GetFinishedInsatnces(Instances);
            


        }
        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            TourInstance currentTourInstance = (TourInstance)TourListDataGrid.CurrentItem;
            CheckPointDetails checkPointDetails = new CheckPointDetails(currentTourInstance);
                checkPointDetails.Show();
            
        }

        private void MostVisited_Click(object sender, RoutedEventArgs e)
        {
            CheckPointDetails checkPointDetails = new CheckPointDetails(historyService.FindMostVisited());
            checkPointDetails.Show();
        }


        private void MostVisitedForYear_Click(object sender, RoutedEventArgs e)
        {

            if (Year > 0)
            {
         //       CheckPointDetails checkPointDetails = new CheckPointDetails(historyService.FindMostVisitedForChosenYear(Year,user));
             //   checkPointDetails.Show();
                ChosenYear.Text = null;
            }
        }
    }
}
