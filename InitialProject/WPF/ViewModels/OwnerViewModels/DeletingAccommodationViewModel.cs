using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.OwnerViews;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class DeletingAccommodationViewModel
    {
        private Accommodation accommodation;
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public DeletingAccommodationViewModel(Accommodation accommodation)
        {
            this.accommodation = accommodation;
            CancelCommand = new RelayCommand(Cancel_Executed, CanExecute);
            DeleteCommand = new RelayCommand(Delete_Executed, CanExecute);
        }

        private bool CanExecute(object sender)
        {
            return true;
        }

        private void Cancel_Executed(object sender)
        {
            Application.Current.Windows.OfType<DeletingAccommodationView>().FirstOrDefault().Close();
        }

        private void Delete_Executed(object sender)
        {
            DeletingAccommodationService deletingAccommodationService = new DeletingAccommodationService();
            deletingAccommodationService.Delete(accommodation);
            MakeDeletedNotification();

            Application.Current.Windows.OfType<DeletingAccommodationView>().FirstOrDefault().Close();
            AccommodationView accommodationView = new AccommodationView(accommodation.Owner);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = accommodationView;
        }

        private void MakeDeletedNotification()
        {
            OwnerNotificationsService notificationsService = new OwnerNotificationsService();
            notificationsService.Add(Domain.Model.OwnerNotificationType.ACCOMMODATION_DELETED, accommodation.Owner);
        }
    }
}
