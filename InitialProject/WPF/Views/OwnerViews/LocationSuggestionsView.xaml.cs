﻿using System;
using System.Collections.Generic;
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
using InitialProject.Model;
using InitialProject.WPF.ViewModels.OwnerViewModels;

namespace InitialProject.WPF.Views.OwnerViews
{
    /// <summary>
    /// Interaction logic for LocationSuggestionsView.xaml
    /// </summary>
    public partial class LocationSuggestionsView : Page
    {
        public LocationSuggestionsViewModel viewModel { get; set; }
        public LocationSuggestionsView(Owner owner)
        {
            InitializeComponent();
            viewModel = new LocationSuggestionsViewModel(owner);
            this.DataContext = viewModel;
        }
    }
}
