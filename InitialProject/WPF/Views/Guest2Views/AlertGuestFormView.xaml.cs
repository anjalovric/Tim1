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
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using InitialProject.WPF.ViewModels.Guest2ViewModels;
namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for AlertGuestForm.xaml
    /// </summary>
    public partial class AlertGuestFormView : Window
    {
        private int AlertId;
        public AlertGuestFormView(int alertId)
        {
            InitializeComponent();
            AlertId = alertId;
            AlertGuestViewModel alertGuestViewModel= new AlertGuestViewModel(AlertId, PointLabel);
            DataContext = alertGuestViewModel;
            if (alertGuestViewModel.CloseAction == null)
                alertGuestViewModel.CloseAction = new Action(this.Close);
        }
    }
}
