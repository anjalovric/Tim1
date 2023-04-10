﻿using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
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
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for ActiveInstanceView.xaml
    /// </summary>
    public partial class ActiveInstanceView : Page
    {
        ActiveInstanceViewModel viewModel;
        public ActiveInstanceView(TourInstance active, ObservableCollection<TourInstance> tours, ObservableCollection<TourInstance> finishedInstances,HomeView homeview)
        {
            InitializeComponent();
            viewModel = new ActiveInstanceViewModel(active, tours, finishedInstances,homeview);
            DataContext = viewModel;
        }
    }
}
