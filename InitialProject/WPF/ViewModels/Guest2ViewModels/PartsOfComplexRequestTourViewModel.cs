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
using System.Windows.Input;
using InitialProject.Domain.Model;
using InitialProject.Help;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest2Views;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class PartsOfComplexRequestTourViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<OrdinaryTourRequests> OrdinaryTourRequests { get; set; }
        private OrdinaryTourRequestsService requestsService;
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (!value.Equals(name))
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Status { get; set; }
        private DateTime Start;
        public DateTime StartDate
        {
            get => Start;
            set
            {
                if (value != Start)
                {
                    Start = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsEnabled { get; set; }
        public RelayCommand ViewCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public ICommand HelpCommandInViewModel { get;}
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private PartsOfComplexRequestTour org;
        public PartsOfComplexRequestTourViewModel(ComplexTourRequests complex,PartsOfComplexRequestTour org)
        {
            requestsService = new OrdinaryTourRequestsService();
            this.org = org;
            OrdinaryTourRequests = new ObservableCollection<OrdinaryTourRequests>(requestsService.GetOrdinaryTourRequestsByComplex(complex.Id));
            ViewCommand = new RelayCommand(View_Executed, CanExecute);
            CloseCommand = new RelayCommand(Close_Executed, CanExecute);
            HelpCommandInViewModel = new RelayCommand(CommandBinding_Executed);
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void View_Executed(object sender)
        {
            OrdinaryTourRequests current = ((Button)sender).DataContext as OrdinaryTourRequests;
            if (current.TourInstanceId != -1)
            {
                DetailsFormView details = new DetailsFormView(current);
                details.Show();
            }
            else
            {
                MessageBox.Show("This tour has not yet been accepted to view details.");
            }
        }
        private void Close_Executed(object sender)
        {
            Application.Current.Windows.OfType<PartsOfComplexRequestTour>().FirstOrDefault().Close();
        }
        private void CommandBinding_Executed(object sender)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                string str = ShowToursHelp.GetHelpKey((DependencyObject)focusedControl);
                ShowToursHelp.ShowHelpForParts(str, org);
            }
        }
    }
}
