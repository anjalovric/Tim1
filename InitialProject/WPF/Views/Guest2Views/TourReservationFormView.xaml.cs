using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Cache;
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
using DotLiquid.Tags;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.WPF.ViewModels.Guest2ViewModels;
using InitialProject.Serializer;
using InitialProject.Service;
using InitialProject.WPF.ViewModels.Guest2ViewModels;
using NPOI.SS.Formula.Functions;

namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for TourReservationForm.xaml
    /// </summary>
    public partial class TourReservationFormView : Window
    {
        public TourReservationFormView(TourInstance currentTourInstance, Guest2 guest2, ObservableCollection<TourInstance> TourInstance, TourInstanceRepository tourInstanceRepository, Label label)
        {
            InitializeComponent();
            DataContext = new TourReservationViewModel(currentTourInstance, guest2, TourInstance, tourInstanceRepository, label, capacityNumber);
        }
    }
}
