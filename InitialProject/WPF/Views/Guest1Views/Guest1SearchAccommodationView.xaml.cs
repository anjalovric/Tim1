using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
using InitialProject.Serializer;
using InitialProject.Service;
using InitialProject.WPF.ViewModels.Guest1ViewModels;

namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for Guest1Overview.xaml
    /// </summary>
    public partial class Guest1SearchAccommodationView : Page
    {
        private Guest1SearchAccommodationViewModel guest1SearchAccommodationViewModel;
        
        public Guest1SearchAccommodationView(Guest1 guest1)
        {
            InitializeComponent();
            guest1SearchAccommodationViewModel = new Guest1SearchAccommodationViewModel(guest1);
            DataContext = guest1SearchAccommodationViewModel;
           
        }
        

    }
}
