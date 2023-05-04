using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.OwnerViews;
using InitialProject.WPF.Views;
using System.Windows;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class CancellingRenovationViewModel
    {
        private AccommodationRenovation renovation;
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand CancelRenovationCommand { get; set; }
        public CancellingRenovationViewModel(AccommodationRenovation renovation)
        {
            this.renovation = renovation;
            CancelCommand = new RelayCommand(Cancel_Executed, CanExecute);
            CancelRenovationCommand = new RelayCommand(CancelRenovation_Executed, CanExecute);
        }

        private bool CanExecute(object sender)
        {
            return true;
        }
        private void Cancel_Executed(object sender)
        {
            Application.Current.Windows.OfType<CancellingRenovationView>().FirstOrDefault().Close();
        }

        private void CancelRenovation_Executed(object sender)
        {
            AccommodationRenovationService renovationService = new AccommodationRenovationService();
            renovationService.Delete(renovation);
            MakeCancelledNotification();

            Application.Current.Windows.OfType<CancellingRenovationView>().FirstOrDefault().Close();
            MyRenovationsView myRenovationsView = new MyRenovationsView(renovation.Accommodation.Owner);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = myRenovationsView;
        }

        private void MakeCancelledNotification()
        {
            OwnerNotificationsService notificationsService = new OwnerNotificationsService();
            notificationsService.Add(OwnerNotificationType.RENOVATION_CANCELLED, renovation.Accommodation.Owner);
        }

    }
}
