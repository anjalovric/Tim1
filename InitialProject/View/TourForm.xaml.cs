using InitialProject.Model;
using System;
using System.Collections.Generic;
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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for TourForm.xaml
    /// </summary>
    public partial class TourForm : Window,INotifyPropertyChanged
    {
        public Tour Tour { get; set; }
       

        public TourForm()
        {
            InitializeComponent();
            Tour=new Tour();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));  
        }

        private void StartedTour_Checked(object sender, RoutedEventArgs e)
        {
            Tour.Started = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
