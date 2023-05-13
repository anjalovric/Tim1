using InitialProject.Model;
using InitialProject.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace InitialProject.WPF.ViewModels
{
    public class CancelViewModel:INotifyPropertyChanged
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
        private string toastVisibility;
        public string ToastVisibility
        {
            get { return toastVisibility; }
            set
            {
                if (value != toastVisibility)
                    toastVisibility = value;
                OnPropertyChanged();
            }
        }
        private string tourName;
        public string TourName
        {
            get { return tourName; }
            set
            {
                if (value != tourName)
                    tourName = value;
                OnPropertyChanged();
            }
        }
        private User tourInstanceGuide;
        private TourInstanceService tourInstanceService;
        private GuideService guideService=new GuideService();
  
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand YesCommand { get; set; }
        public RelayCommand NoCommand { get; set; }

        public CancelViewModel(User guide)
        {
            tourInstanceGuide = guide;

            tourInstanceService = new TourInstanceService();
            Guide loggdedGuide=guideService.GetByUsername(guide.Username);
            MakeCancelableTourList(loggdedGuide);
            MakeCommands();
            ToastVisibility = "Hidden";
        }

        private void MakeCommands()
        {
            CancelCommand = new RelayCommand(CancelExecuted, CanExecute);
            YesCommand= new RelayCommand(Yes_Executed, CanExecute);
            NoCommand= new RelayCommand(No_Executed, CanExecute);

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
        private void CancelExecuted(object sender)
        {
            ToastVisibility = "Visible";
            TourService tourService= new TourService();
            tourService.SetTour(Selected);
            TourName=Selected.Tour.Name;
        }

        private void Yes_Executed(object sender)
        {
            TourInstanceCancelationService service = new TourInstanceCancelationService();
            TourInstance currentTourInstance = Selected;
            service.CancelTourInstance(currentTourInstance, TourInstances, tourInstanceGuide);
            ToastVisibility = "Hidden";
        }

        private void No_Executed(object sender)
        {
            ToastVisibility = "Hidden";
        }

    }
}
