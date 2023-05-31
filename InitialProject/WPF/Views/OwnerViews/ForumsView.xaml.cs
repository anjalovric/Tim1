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
    /// Interaction logic for ForumsView.xaml
    /// </summary>
    public partial class ForumsView : Page
    {
        public ForumsViewModel viewModel { get; set; }
        public ForumsView(Owner owner)
        {
            InitializeComponent();
            viewModel = new ForumsViewModel(owner);
            this.DataContext = viewModel;
        }
    }
}
