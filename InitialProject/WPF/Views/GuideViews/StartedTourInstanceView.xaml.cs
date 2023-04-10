﻿using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.WPF.ViewModels;
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

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for StartedTourInstanceView.xaml
    /// </summary>
    public partial class StartedTourInstanceView : Page 
    {
        StartedTourViewModel viewModel;
        public StartedTourInstanceView(TourInstance selectedInstance, ObservableCollection<TourInstance> tours, ObservableCollection<TourInstance> finishedInstances)
        {
            InitializeComponent();
            viewModel=new StartedTourViewModel(selectedInstance, tours, finishedInstances,this.Next,this.Finish,this.FinishMessage);
            DataContext = viewModel;

        }

        private void FinishTour_Click(object sender, RoutedEventArgs e)
        {
            viewModel.FinishTour();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
          viewModel.NextPoint();

        }
    }
}
