using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.Domain.Model;
using Microsoft.Win32;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.WPF.Views;
using InitialProject.WPF.ViewModels.Guest1ViewModels;

namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for OwnerAndAccommodationReviewForm.xaml
    /// </summary>
    public partial class OwnerAndAccommodationReviewFormView : Page
    {
        OwnerAndAccommodationReviewFormViewModel ownerAndAccommodationReviewFormViewModel;
        public OwnerAndAccommodationReviewFormView(Guest1 guest1,AccommodationReservation SelectedCompletedReservation)
        {
            InitializeComponent();
            ownerAndAccommodationReviewFormViewModel = new OwnerAndAccommodationReviewFormViewModel(guest1,SelectedCompletedReservation);
            this.DataContext = ownerAndAccommodationReviewFormViewModel;
        }
       
    }
}