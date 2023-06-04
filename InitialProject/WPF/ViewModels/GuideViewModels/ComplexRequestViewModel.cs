using InitialProject.APPLICATION.UseCases;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.GuideViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels.GuideViewModels
{
    public class ComplexRequestViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<ComplexTourRequests> ComplexTourRequests { get; set; }
        public ObservableCollection<OrdinaryTourRequests> OrdinaryTourRequests { get; set; }

        private ComplexTourRequestsService complexTourRequestsService;
        private OrdinaryTourRequestsService ordinaryTourRequestsService;
        private GuideService guideService;

        private User loggedUser;
        public ObservableCollection<TourInstance> TodaysInstances;
        public ObservableCollection<TourInstance> FutureInstances;
        public RelayCommand ViewOrdinaryRequestCommand { get; private set; }
        public RelayCommand ShowDescriptionCommand { get; private set; }
        public RelayCommand CreateTourCommand { get; private set; }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (value != description)
                    description = value;
                OnPropertyChanged("Description");
            }
        }

        private ComplexTourRequests selected;
        public ComplexTourRequests Selected
        {
            get { return selected; }
            set
            {
                if (value != selected)
                    selected = value;
                OnPropertyChanged("Selected");
            }
        }

        private OrdinaryTourRequests selectedOrdinaryRequest;
        public OrdinaryTourRequests SelectedOrdinaryRequest
        {
            get { return selectedOrdinaryRequest; }
            set
            {
                if (value != selectedOrdinaryRequest)
                    selectedOrdinaryRequest = value;
                OnPropertyChanged("SelectedOrdinaryRequest");
            }
        }
        public ComplexRequestViewModel(User loggeduser, ObservableCollection<TourInstance> todaysIntances, ObservableCollection<TourInstance> futureInstances) 
        {
            loggedUser = loggeduser;
            TodaysInstances = todaysIntances;
            FutureInstances = futureInstances;
            complexTourRequestsService = new ComplexTourRequestsService();
            ordinaryTourRequestsService= new OrdinaryTourRequestsService();
            guideService = new GuideService();  
            ComplexTourRequests = new ObservableCollection<ComplexTourRequests>(complexTourRequestsService.GetOnWaitingComplexRequests(guideService.GetByUsername(loggedUser.Username)));
            OrdinaryTourRequests = new ObservableCollection<OrdinaryTourRequests>();
            MakeCommands();


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

        private void MakeCommands()
        {
            ViewOrdinaryRequestCommand = new RelayCommand(ViewOrdinaryRequestCommand_Executed, CanExecute);
            ShowDescriptionCommand = new RelayCommand(ShowDescriptionCommand_Executed, CanExecute);
            CreateTourCommand = new RelayCommand(CreateTourCommand_Executed, CanExecute);
        }
        private void ViewOrdinaryRequestCommand_Executed(object sender)
        {
            OrdinaryTourRequests.Clear();
            foreach(OrdinaryTourRequests ordinaryRequest in ordinaryTourRequestsService.GetOnWaitingRequestByComplexId(Selected.Id))
                OrdinaryTourRequests.Add(ordinaryRequest);
        }

        private void ShowDescriptionCommand_Executed(object sender)
        {
            Description = SelectedOrdinaryRequest.Description;
        }

        private void CreateTourCommand_Executed(object sender)
        {
            CreateTourFromComplexRequestView createTourFromComplexRequestViewModel = new CreateTourFromComplexRequestView(TodaysInstances, loggedUser, FutureInstances, SelectedOrdinaryRequest, OrdinaryTourRequests, ComplexTourRequests, Selected);
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = createTourFromComplexRequestViewModel;
            Description = "";

        }


    }
}
