using InitialProject.Help;
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
using System.Windows.Controls;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class FinishedTourInstancesViewModel:INotifyPropertyChanged
    {
        private Guest2 guest2;
        private ObservableCollection<TourInstance> completedTours;
        public ObservableCollection<TourInstance> CompletedTours
        {
            get { return completedTours; }
            set
            {
                if (value != completedTours)
                    completedTours = value;
                OnPropertyChanged("CompletedTours");
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
                OnPropertyChanged("Selected");
            }

        }
        public RelayCommand RateCommand { get; set; }
        public ICommand HelpCommandInViewModel { get; }
        private GuideAndTourReviewService guideAndTourReviewService;
        private FinishedTourInstancesFormView org;
        public FinishedTourInstancesViewModel(Guest2 guest2,FinishedTourInstancesFormView org)
        {
            CompletedTours = new ObservableCollection<TourInstance>();
            this.guest2 = guest2;
            this.org = org;
            guideAndTourReviewService = new GuideAndTourReviewService();
            guideAndTourReviewService.SetTourInstances(CompletedTours, guest2);
            RateCommand = new RelayCommand(Rate_Executed, CanExecute);
            HelpCommandInViewModel = new RelayCommand(CommandBinding_Executed);
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
        public void Rate_Executed(object sender)
        {
            if (guideAndTourReviewService.HasReview(Selected,guest2))
            {
                MessageBox.Show("This reservation is already reviewed.");
                return;
            }
            GuideAndTourReviewFormView guideAndTourReview = new GuideAndTourReviewFormView(Selected, guest2);
            guideAndTourReview.Show();
        }
        private void CommandBinding_Executed(object sender)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                string str = ShowToursHelp.GetHelpKey((DependencyObject)focusedControl);
                ShowToursHelp.ShowHelpForFinished(str, org);
            }
        }
    }
}
