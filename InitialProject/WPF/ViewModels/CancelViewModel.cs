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

        public CancelViewModel(User guide,DataGrid tourListDataGrid)
        {
            tourInstanceGuide = guide;
            TourListDataGrid= tourListDataGrid;

            tourInstanceService = new TourInstanceService();
            Guide loggdedGuide=guideService.GetByUsername(guide.Username);
            MakeCancelableTourList(loggdedGuide);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void MakeCancelableTourList(Guide guide)
        {
            tourInstances = new ObservableCollection<TourInstance>(tourInstanceService.FindCancelableTours(guide));
        }
        public void CancelTour()
        {
            TourInstance currentTourInstance = (TourInstance)TourListDataGrid.CurrentItem;
            tourInstanceService.CancelTourInstance(currentTourInstance, TourInstances, tourInstanceGuide);

        }
    }
}
