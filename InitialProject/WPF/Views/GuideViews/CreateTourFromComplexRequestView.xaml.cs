using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.WPF.ViewModels.GuideViewModels;
using NPOI.HSSF.Record.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for CreateTourFromComplexRequestView.xaml
    /// </summary>
    public partial class CreateTourFromComplexRequestView : Page
    {
        public CreateTourFromComplexRequestView(ObservableCollection<TourInstance> todaysInstances,User loggedUser, ObservableCollection<TourInstance> futureInstances,OrdinaryTourRequests SelectedOrdinaryRequest,ObservableCollection<OrdinaryTourRequests> OrdinaryTourRequests, ObservableCollection<ComplexTourRequests> ComplexTourRequests,ComplexTourRequests Selected)
        {
            InitializeComponent();
            DataContext = new CreateTourFromComplexRequestViewModel(todaysInstances,loggedUser,futureInstances,SelectedOrdinaryRequest,OrdinaryTourRequests,ComplexTourRequests,Selected);
        }
    }
}
