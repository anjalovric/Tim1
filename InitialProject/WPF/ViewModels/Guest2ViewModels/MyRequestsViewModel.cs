using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest2Views;
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
    public class MyRequestsViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<OrdinaryTourRequests> ordinaryTourRequests;
        public ObservableCollection<OrdinaryTourRequests> OrdinaryTourRequests
        {
            get => ordinaryTourRequests;
            set
            {
                if (value != ordinaryTourRequests)
                {
                    ordinaryTourRequests = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Status { get; set; }
        public string Name { get; set; }
        private OrdinaryTourRequestsService ordinaryTourRequestsService { get; set; }
        public RelayCommand CreateCommand { get; set; }
        public RelayCommand StatisticsCommand { get; set; }
        private Guest2 Guest2;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MyRequestsViewModel(Model.Guest2 guest2)
        {
            Guest2 = guest2;
            ordinaryTourRequestsService = new OrdinaryTourRequestsService();
            OrdinaryTourRequests = new ObservableCollection<OrdinaryTourRequests>(ordinaryTourRequestsService.GetByGuestId(Guest2.Id));
            CreateCommand = new RelayCommand(Create_Executed, CanExecute);
            StatisticsCommand = new RelayCommand(Statistics_Executed, CanExecute);
            InvalidStatus();
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void Create_Executed(object sender)
        {
            CreateOrdinaryTourRequestView createOrdinaryTourRequest = new CreateOrdinaryTourRequestView(Guest2);
            createOrdinaryTourRequest.Show();
        }
        private void Statistics_Executed(object sender)
        {
            TourRequestStatisticsView tourRequestStatisticsView = new TourRequestStatisticsView(Guest2);
            tourRequestStatisticsView.Show();
        }
        private void InvalidStatus()
        {

            foreach(OrdinaryTourRequests request in OrdinaryTourRequests)
            {
                if (request.StartDate.Day <= DateTime.Now.Day+2 && request.StartDate.Month==DateTime.Now.Month && request.StartDate.Year==DateTime.Now.Year && request.GuideId==-1)
                {
                    request.Status = "Invalid";
                    ordinaryTourRequestsService.Update(request);
                }
            }
        }
    }
}
