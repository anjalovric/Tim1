using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
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
using InitialProject.WPF.ViewModels.Guest2ViewModels;
namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for OrdinaryTourRequestDetailsForm.xaml
    /// </summary>
    public partial class OrdinaryTourRequestDetailsForm : Window
    { 
        public OrdinaryTourRequestDetailsForm(NewTourNotification notification,Model.Guest2 guest2)
        {
            InitializeComponent();
            DataContext = new OrdinaryTourRequestDetailsViewModel(notification, guest2,this);
        }
    }
}
