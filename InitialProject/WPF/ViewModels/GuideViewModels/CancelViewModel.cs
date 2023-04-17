using InitialProject.Model;
using InitialProject.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace InitialProject.WPF.ViewModels
{
    public class CancelViewModel
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
        private TourInstanceService tourInstanceService;
        private GuideService guideService=new GuideService();
  
        private DataGrid TourListDataGrid;
        public RelayCommand CancelCommand { get; set; }

        public CancelViewModel(User guide,DataGrid tourListDataGrid)
        {
            tourInstanceGuide = guide;
            TourListDataGrid= tourListDataGrid;

            tourInstanceService = new TourInstanceService();
            Guide loggdedGuide=guideService.GetByUsername(guide.Username);
            MakeCancelableTourList(loggdedGuide);
            CancelCommand = new RelayCommand(CancelExecuted, CanExecute);
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void MakeCancelableTourList(Guide guide)
        {
            TourInstanceCancelationService tourInstanceCancelationService = new TourInstanceCancelationService();
            tourInstances = new ObservableCollection<TourInstance>(tourInstanceCancelationService.FindCancelableTours(guide));
        }
        public void CancelExecuted(object sender)
        {
            TourInstanceCancelationService service = new TourInstanceCancelationService();
            TourInstance currentTourInstance = (TourInstance)TourListDataGrid.CurrentItem;
            service.CancelTourInstance(currentTourInstance, TourInstances, tourInstanceGuide);
        }
    }
}
