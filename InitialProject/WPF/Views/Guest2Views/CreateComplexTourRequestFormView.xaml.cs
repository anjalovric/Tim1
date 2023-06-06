using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.WPF.ViewModels.Guest2ViewModels;
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
using System.Windows.Shapes;

namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for CreateComplexTourRequestFormView.xaml
    /// </summary>
    public partial class CreateComplexTourRequestView : Window
    {
        public CreateComplexTourRequestView(Guest2 guest2, ObservableCollection<ComplexTourRequests> complexTourRequests)
        {
            InitializeComponent();
            
            this.DataContext = new CreateComplexTourRequestViewModel(guest2, complexTourRequests,this);
        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
        }

    }
}
