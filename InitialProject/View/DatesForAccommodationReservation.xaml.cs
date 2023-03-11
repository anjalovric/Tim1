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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using InitialProject.Model;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for DatesForAccommodationReservation.xaml
    /// </summary>
    public partial class DatesForAccommodationReservation : Window
    {
        public ObservableCollection<FreeDatesForAccommodationReservation> freeDatesForAccommodations { get; set; }
       
        public DatesForAccommodationReservation()
        {
            InitializeComponent();
            this.DataContext = this;
            freeDatesForAccommodations = new ObservableCollection<FreeDatesForAccommodationReservation>();
            
        }
        public void AddNewDateRange(DateTime startDate, DateTime endDate)
        {
            freeDatesForAccommodations.Add(new FreeDatesForAccommodationReservation(startDate, endDate));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
