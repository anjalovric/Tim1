using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.View;
using InitialProject.WPF.ViewModels.Guest2ViewModels;
using Org.BouncyCastle.Crypto.Tls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for FinishedTourInstances.xaml
    /// </summary>
    
    public partial class FinishedTourInstancesFormView : UserControl
    {
        private Guest2 guest2;
        public FinishedTourInstancesFormView(Guest2 guest2)
        {
            InitializeComponent();
            this.guest2 = guest2;
            DataContext = new FinishedTourInstancesViewModel(guest2);
        }
    }
}
