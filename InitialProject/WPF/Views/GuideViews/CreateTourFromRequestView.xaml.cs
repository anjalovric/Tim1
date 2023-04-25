using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.WPF.ViewModels.GuideViewModels;
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
    /// Interaction logic for CreateTourFromRequestView.xaml
    /// </summary>
    public partial class CreateTourFromRequestView : Page
    {
        public CreateTourFromRequestView(ObservableCollection<TourInstance>Tours,User user,ObservableCollection<TourInstance> Future,OrdinaryTourRequests request,ObservableCollection<OrdinaryTourRequests> Requests)
        {
            InitializeComponent();
            DataContext=new CreateTourFromRequestViewModel(Tours,user,Future,request,Requests);
        }

    }
}
