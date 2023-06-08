using InitialProject.Domain.Model;
using InitialProject.Help;
using InitialProject.Model;
using InitialProject.ReportPatterns;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest2Views;
using InitialProject.WPF.Views.GuideViews;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

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
        private ObservableCollection<OrdinaryTourRequests> requests;
        public ObservableCollection<OrdinaryTourRequests> Requests
        {
            get => requests;
            set
            {
                if (value != requests)
                {
                    requests = value;
                    OnPropertyChanged();
                }
            }
        }
        private string ordinary;
        public string Ordinary
        {
            get => ordinary;
            set
            {
                if (value != ordinary)
                {
                    ordinary = value;
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<ComplexTourRequests> complexTourRequests;
        public ObservableCollection<ComplexTourRequests> ComplexTourRequests
        {
            get => complexTourRequests;
            set
            {
                if (value != complexTourRequests)
                {
                    complexTourRequests = value;
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<string> listSource;
        public ObservableCollection<string> ListSource
        {
            get { return listSource; }
            set
            {
                listSource = value;
                OnPropertyChanged();
            }
        }
        private ComplexTourRequests selectedItem;
        public ComplexTourRequests SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
              

               
                setListSource(selectedItem);

            }
        }
        private bool isComboBoxDropDownOpen;
        public bool IsComboBoxDropDownOpen
        {
            get { return isComboBoxDropDownOpen; }
            set
            {
                isComboBoxDropDownOpen = value;
                OnPropertyChanged(nameof(IsComboBoxDropDownOpen));
            }
        }
        private bool _isDropDownOpen;
        public bool IsDropDownOpen
        {
            get { return _isDropDownOpen; }
            set
            {
                _isDropDownOpen = value;
                OnPropertyChanged(nameof(IsDropDownOpen));
            }
        }
        int tCounter = 0;
        int sCounter = 0;
        int rCounter = 0;
        int aCounter = 0;
        int gCounter = 0;
        int hCounter = 0;
        int iCounter = 0;
        int bCounter = 0;
        int srbCounter = 0;
        int rusCounter = 0;
        int spCounter = 0;
        int engCounter = 0;
        int itCounter = 0;
        int arabCounter = 0;
        
        public string Status { get; set; }
        public string Name { get; set; }
        private OrdinaryTourRequestsService ordinaryTourRequestsService { get; set; }
        private APPLICATION.UseCases.ComplexTourRequestsService complexTourRequestsService { get; set; }
        public RelayCommand CreateCommand { get; set; }
        public RelayCommand StatisticsCommand { get; set; }
        private Guest2 Guest2;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private int selectedTabIndex;
        public int SelectedTabIndex
        {
            get { return selectedTabIndex; }
            set
            {
                selectedTabIndex = value;
                OnPropertyChanged(nameof(SelectedTabIndex));
            }
        }
        public ICommand HelpCommandInViewModel { get; }
        public RelayCommand ViewCommand { get; set; }
        public RelayCommand GenerateReportCommand { get; set; }
        private MyRequestsFormView org;
        public MyRequestsViewModel(Model.Guest2 guest2,MyRequestsFormView org)
        {
            Guest2 = guest2;
            ordinaryTourRequestsService = new OrdinaryTourRequestsService();
            ListSource = new ObservableCollection<string>();
            this.org = org;
            complexTourRequestsService = new APPLICATION.UseCases.ComplexTourRequestsService();
            Requests = new ObservableCollection<OrdinaryTourRequests>(ordinaryTourRequestsService.GetOrdinaryTourRequestsForComplexRequest(Guest2.Id));
            OrdinaryTourRequests = new ObservableCollection<OrdinaryTourRequests>(ordinaryTourRequestsService.GetOnlyOrdinaryRequestsByGuestId(Guest2.Id));
            ComplexTourRequests = new ObservableCollection<ComplexTourRequests>(complexTourRequestsService.GetByGuestId(Guest2.Id));
            checkStatus();
            CreateCommand = new RelayCommand(Create_Executed, CanExecute);
            StatisticsCommand = new RelayCommand(Statistics_Executed, CanExecute);
            ViewCommand = new RelayCommand(View_Executed, CanExecute);
            HelpCommandInViewModel = new RelayCommand(CommandBinding_Executed);
            GenerateReportCommand = new RelayCommand(GenerateReport_Executed, CanExecute);
            InvalidStatus();
            FindLocation();
            FindLanguage();
        }
        private void setListSource(ComplexTourRequests SelectedItem)
        {
            ListSource.Clear();
            foreach (OrdinaryTourRequests ordinaryTourRequests in ordinaryTourRequestsService.GetByGuestId(Guest2.Id))
            {
                if (SelectedItem != null && ordinaryTourRequests.ComplexId == SelectedItem.Id)
                {
                    ListSource.Add(ordinaryTourRequests.Name);
                }
            }
        }
        private void checkStatus()
        {
            int ordinaryCount = 0;
            int acceptedCount = 0;
            foreach (ComplexTourRequests complexTourRequests in ComplexTourRequests)
            {
                foreach (OrdinaryTourRequests ordinaryTourRequests in Requests)
                {
                    if (complexTourRequests.Id == ordinaryTourRequests.ComplexId)
                    {
                        ordinaryCount++;
                    }
                    if(complexTourRequests.Id == ordinaryTourRequests.ComplexId && ordinaryTourRequests.Status == Domain.Model.Status.ACCEPTED)
                    {
                        acceptedCount++;
                    }
                }
                if (ordinaryCount == acceptedCount)
                {
                    complexTourRequests.Status = Domain.Model.Type.ACCEPTED;
                    complexTourRequestsService.Update(complexTourRequests);
                }
                    
                ordinaryCount = 0;
                acceptedCount = 0;
            }
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void Create_Executed(object sender)
        {
            if (SelectedTabIndex==0)
            {
                CreateOrdinaryTourRequestView createOrdinaryTourRequest = new CreateOrdinaryTourRequestView(Guest2, OrdinaryTourRequests);
                createOrdinaryTourRequest.Show();
            }
            else if (SelectedTabIndex == 1)
            {
                CreateComplexTourRequestView createComplexTourRequest = new CreateComplexTourRequestView(Guest2,ComplexTourRequests);
                createComplexTourRequest.Show();
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    createComplexTourRequest.Activate(); 
                }));
            }  
        }
        private void View_Executed(object sender)
        {
            PartsOfComplexRequestTour partsOfComplexRequestTour = new PartsOfComplexRequestTour(SelectedItem);
            partsOfComplexRequestTour.Show();
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
                    request.Status = Domain.Model.Status.INVALID;
                    ordinaryTourRequestsService.Update(request);
                }
            }
        }
        private void CommandBinding_Executed(object sender)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                string str = ShowToursHelp.GetHelpKey((DependencyObject)focusedControl);
                ShowToursHelp.ShowHelpForRequests(str, org);
            }
        }
        public void FindLocation()
        {
            
            LocationService locationService = new LocationService();
            foreach (var o in OrdinaryTourRequests)
            {
                foreach (Model.Location location in locationService.GetAll())
                {
                    if (location.Country == "Turkey" && location.Id == o.Location.Id)
                        tCounter++;
                    else if (location.Country == "Serbia" && location.Id == o.Location.Id)
                        sCounter++;
                    else if (location.Country == "Russia" && location.Id == o.Location.Id)
                        rCounter++;
                    else if (location.Country == "Austria" && location.Id == o.Location.Id)
                        aCounter++;
                    else if (location.Country == "Greece" && location.Id == o.Location.Id)
                        gCounter++;
                    else if (location.Country == "Hungary" && location.Id == o.Location.Id)
                        hCounter++;
                    else if (location.Country == "Italy" && location.Id == o.Location.Id)
                        iCounter++;
                    else if (location.Country == "BiH" && location.Id == o.Location.Id)
                        bCounter++;
                }
            }
        }
        private void FindLanguage()
        {
            foreach (var o in OrdinaryTourRequests)
            {
                if (o.Language == "serbian")
                    srbCounter++;
                else if (o.Language == "russian")
                    rusCounter++;
                else if (o.Language == "arabic")
                    arabCounter++;
                else if (o.Language == "italian")
                    itCounter++;
                else if (o.Language == "english")
                    engCounter++;
                else if (o.Language == "spanish")
                    spCounter++;
            }

        }
        private void GenerateReport_Executed(object sender)
        {
            ReportGenerator reportGenerator = new ReportPatterns.Guest2ReportPattern(Guest2, tCounter, sCounter, rCounter, aCounter, gCounter, hCounter, iCounter, bCounter,itCounter,spCounter,srbCounter,engCounter,arabCounter,rusCounter);
            reportGenerator.GenerateReport();
            PDFPreviewView pdfPreviewView = new PDFPreviewView();
            Application.Current.Windows.OfType<Guest2Overview>().FirstOrDefault().Content = pdfPreviewView;
        }
    }
}
