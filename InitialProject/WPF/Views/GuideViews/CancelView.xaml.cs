using InitialProject.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for CancelView.xaml
    /// </summary>
    public partial class CancelView : Page
    {
        private ObservableCollection<TourInstance> tourInstances;
        public ObservableCollection<TourInstance> TourInstances
        {
            get { return tourInstances; }
            set
            {
                if (value != tourInstances)
                    tourInstances = value;
                OnPropertyChanged("TourInstances");
            }
        }
        private TourInstance selected;
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

        private User tourInstanceGuide;
        private CancelTourService cancelTourService;
        public CancelView(User guide)
        {
            InitializeComponent();
            DataContext = this;
            tourInstanceGuide = guide;

            cancelTourService = new CancelTourService();
            MakeCancelableTourList();
        }
        private void MakeCancelableTourList()
        {
            tourInstances = new ObservableCollection<TourInstance>(cancelTourService.FindCancelableTours());
            cancelTourService.SetLocationToTour();
            cancelTourService.SetTourToTourInstance();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            TourInstance currentTourInstance = (TourInstance)TourListDataGrid.CurrentItem;
            cancelTourService.CancelTourInstance(currentTourInstance, TourInstances, tourInstanceGuide);

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
