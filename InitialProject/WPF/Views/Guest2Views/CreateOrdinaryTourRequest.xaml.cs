using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repository;
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
    /// Interaction logic for CreateOrdinaryTourRequest.xaml
    /// </summary>
    public partial class CreateOrdinaryTourRequest : Window
    {
        private int capacity;
        CreateOrdinaryTourRequestViewModel viewModel;
        public CreateOrdinaryTourRequest(Model.Guest2 guest2)
        {
            InitializeComponent();
            capacity = 1;
            viewModel = new CreateOrdinaryTourRequestViewModel(Capacity, Name,guest2, Language, Description, countryInput, cityInput);
            DataContext = viewModel;
        }
        private void CountryInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.CountryInput_SelectionChanged();
        }
    }
}
