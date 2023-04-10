using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.OwnerViews;
using InitialProject.WPF.Views;
using System.Windows;
using System.Linq;
using System.Windows.Controls;

namespace InitialProject.WPF.ViewModels
{
    public class ReservationReschedulingViewModel : INotifyPropertyChanged
    {
        private Owner profileOwner;
        public ObservableCollection<RequestForReshcedulingViewModel> Requests { get; set; }
        private RequestForReschedulingService requestService;
        private ReschedulingAccommodationRequestService reschedulingRequestService;
        private CompletedAccommodationReschedulingRequestService completedReschedulingRequestService;
        private RequestForReshcedulingViewModel selectedRequest;
        public RelayCommand DeclineCommand { get; set; }
        public RelayCommand ApproveCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ReservationReschedulingViewModel(Owner owner)
        {
            profileOwner = owner;
            requestService = new RequestForReschedulingService();
            reschedulingRequestService = new ReschedulingAccommodationRequestService();
            Requests = new ObservableCollection<RequestForReshcedulingViewModel>(requestService.GetPendingRequests(owner));
            SelectedRequest = new RequestForReshcedulingViewModel(profileOwner);
            InitializeSelectedRequest();
            completedReschedulingRequestService = new CompletedAccommodationReschedulingRequestService();
            MakeCommands();
        }

        private void MakeCommands()
        {
            DeclineCommand = new RelayCommand(Decline_Executed, CanExecute);
            ApproveCommand = new RelayCommand(Approve_Executed, CanExecute);
        }
        private bool CanExecute(object sender)
        {
            return true;
        }

        private void Decline_Executed(object sender)
        {
            DecliningRequestView decliningRequestView = new DecliningRequestView(SelectedRequest.Request);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = decliningRequestView;
            DeclineRequest(decliningRequestView.decliningRequestViewModel.ReschedulingAccommodationRequest);
        }

        private void Approve_Executed(object sender)
        {
            ApproveRequest();
        }
        private void InitializeSelectedRequest()
        {
            if(Requests.Count>0)
               SelectedRequest = Requests[0];
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RequestForReshcedulingViewModel SelectedRequest
        {
            get => selectedRequest;
            set
            {
                if (value !=selectedRequest)
                {
                    selectedRequest = value;
                    OnPropertyChanged();
                }
            }
        }

        public void DeclineRequest(ReschedulingAccommodationRequest request)
        {
            SelectedRequest.Request.OwnerExplanationForDeclining = request.OwnerExplanationForDeclining;
        }

        public void SaveDeclinedRequest(string ownersExplanationForDeclining)
        {
            SelectedRequest.Request.OwnerExplanationForDeclining = ownersExplanationForDeclining;
            reschedulingRequestService.ChangeState(SelectedRequest.Request, State.Declined);
            completedReschedulingRequestService.DeclineRequest(SelectedRequest.Request);
            Requests.Remove(SelectedRequest);
            InitializeSelectedRequest();
        }

        public void ApproveRequest()
        {
            reschedulingRequestService.ChangeState(SelectedRequest.Request, State.Approved);
            completedReschedulingRequestService.ApproveRequest(SelectedRequest.Request);
            Requests.Remove(SelectedRequest);
            InitializeSelectedRequest();
        }

    }
}
