using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.ViewModels.Guest1ViewModels;

namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for ChangeDateAccommodationReservationForm.xaml
    /// </summary>
    public partial class ReschedulingAccommodationReservationFormView : Window
    {
        private ReschedulingAccommodationReservationFormViewModel reschedulingAccommodationReservationFormViewModel;
        public ReschedulingAccommodationReservationFormView(AccommodationReservation SelectedNotCompletedReservation)
        {
            InitializeComponent();
            reschedulingAccommodationReservationFormViewModel = new ReschedulingAccommodationReservationFormViewModel(SelectedNotCompletedReservation);
            this.DataContext = reschedulingAccommodationReservationFormViewModel; 
            
            
        }
        

    }
}
