using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for AccommodationReservationForm.xaml
    /// </summary>
    public partial class AccommodationReservationFormView : Window
    {
        private AccommodationReservationFormViewModel accommodationReservationFormViewModel;
        public AccommodationReservationFormView(Accommodation currentAccommodation, Guest1 guest1)
        {
            InitializeComponent();
            accommodationReservationFormViewModel = new AccommodationReservationFormViewModel(guest1, currentAccommodation);
            this.DataContext = accommodationReservationFormViewModel;
            
        }
        
    }   
}

