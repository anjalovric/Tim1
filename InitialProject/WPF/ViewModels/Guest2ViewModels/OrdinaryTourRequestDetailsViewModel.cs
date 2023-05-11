using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest2Views;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class OrdinaryTourRequestDetailsViewModel:INotifyPropertyChanged
    {
        private NewTourNotification Notification;
        private TourInstance tourInstance;
        private ObservableCollection<TourInstance> TourInstances;
        private TourInstanceService tourInstanceService;
        public TourInstance TourInstance
        {
            get { return tourInstance; }
            set
            {
                if (value != tourInstance)
                    tourInstance = value;
                OnPropertyChanged("TourInstance");
            }

        }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public OrdinaryTourRequestDetailsViewModel(NewTourNotification notification, Model.Guest2 guest2)
        {
            Notification = notification;
            tourInstanceService = new TourInstanceService();
            TourInstances = new ObservableCollection<TourInstance>(tourInstanceService.GetAll());
            TourInstance = SetTourInstance(Notification.TourInstance);
            StartDate = TourInstance.StartDate.ToString();
            EndDate= TourInstance.StartDate.AddHours(TourInstance.Tour.Duration).ToString();
            CloseCommand = new RelayCommand(Close_Executed,CanExecute);
        }
        private TourInstance SetTourInstance(TourInstance tourInstance)
        {
            TourInstanceTourLocationService tourInstanceTourLocationService = new TourInstanceTourLocationService();
            List<TourInstance> Instances = new List<TourInstance>(tourInstanceService.GetAll());
            tourInstanceTourLocationService.FillWithTours(Instances);
            foreach (TourInstance instance in Instances)
            {
                if (instance.Id == tourInstance.Id)
                    tourInstance = instance;
            }
            return tourInstance;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void Close_Executed(object sender)
        {
            Application.Current.Windows.OfType<OrdinaryTourRequestDetailsForm>().FirstOrDefault().Close();
        }
    }
}
