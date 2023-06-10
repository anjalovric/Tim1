using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.OwnerViews;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
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
        public RelayCommand OKCommand { get; set; }
        private string stackPanelVisibility;
        private string stackPanelMessage;
        private bool isOkPressedInDemo;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ReservationReschedulingViewModel(Owner owner)
        {
            profileOwner = owner;
            reschedulingRequestService = new ReschedulingAccommodationRequestService();
            requestService = new RequestForReschedulingService();
            Requests = new ObservableCollection<RequestForReshcedulingViewModel>(requestService.GetPendingRequests(profileOwner));
            SelectedRequest = new RequestForReshcedulingViewModel(profileOwner);
            InitializeSelectedRequest();
            completedReschedulingRequestService = new CompletedAccommodationReschedulingRequestService();
            MakeCommands();
            DisplayNotificationPanel();
        }

        private void RefreshRequests()
        {
            requestService = new RequestForReschedulingService();
            Requests.Clear();
            foreach (var request in requestService.GetPendingRequests(profileOwner))
            {
                Requests.Add(request);
            }
            DisplayNotificationPanel();
        }

        private void MakeCommands()
        {
            DeclineCommand = new RelayCommand(Decline_Executed, CanExecute);
            ApproveCommand = new RelayCommand(Approve_Executed, CanExecute);
            OKCommand = new RelayCommand(OK_Executed, CanExecute);
        }
        private bool CanExecute(object sender)
        {
            return true;
        }

        private void OK_Executed(object sender)
        {
            StackPanelVisibility = "Hidden";
        }

        private void Decline_Executed(object sender)
        {
            if (SelectedRequest != null)
            {
                DecliningRequestView decliningRequestView = new DecliningRequestView(SelectedRequest.Request);
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = decliningRequestView;
                DeclineRequest(decliningRequestView.decliningRequestViewModel.ReschedulingAccommodationRequest);
            }
        }

        private void Approve_Executed(object sender)
        {
            if (SelectedRequest != null)
                ApproveRequest();
        }
        private void InitializeSelectedRequest()
        {
            if (Requests.Count > 0)
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
                if (value != selectedRequest)
                {
                    selectedRequest = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StackPanelMessage
        {
            get { return stackPanelMessage; }
            set
            {
                if (value != stackPanelMessage)
                {
                    stackPanelMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StackPanelVisibility
        {
            get { return stackPanelVisibility; }
            set
            {
                if (value != stackPanelVisibility)
                {
                    stackPanelVisibility = value;
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
            RefreshRequests();
        }

        public void ApproveRequest()
        {
            reschedulingRequestService.ChangeState(SelectedRequest.Request, State.Approved);
            completedReschedulingRequestService.ApproveRequest(SelectedRequest.Request);
            Requests.Remove(SelectedRequest);
            InitializeSelectedRequest();
            RefreshRequests();
        }

        public bool IsOkPressedInDemo
        {
            get { return isOkPressedInDemo; }
            set
            {
                if (value != isOkPressedInDemo)
                {
                    isOkPressedInDemo = value;
                    OnPropertyChanged();
                }
            }
        }
        private void DisplayNotificationPanel()
        {
            OwnerNotificationsService notificationsService = new OwnerNotificationsService();
            if (notificationsService.IsRequestAccepted(profileOwner))
            {
                StackPanelMessage = "Request successfully acceppted!";
                StackPanelVisibility = "Visible";
                notificationsService.Delete(Domain.Model.OwnerNotificationType.REQUEST_ACCEPPTED, profileOwner);
            }
            else if (notificationsService.IsRequestDeclined(profileOwner))
            {
                StackPanelMessage = "Request successfully declined!";
                StackPanelVisibility = "Visible";
                notificationsService.Delete(Domain.Model.OwnerNotificationType.REQUEST_DECLINED, profileOwner);
            }
            else
            {
                StackPanelVisibility = "Hidden";
            }
        }
    }
}
