﻿using InitialProject.Model;
using InitialProject.WPF.ViewModels.GuideViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for RequestsStatistisYearly.xaml
    /// </summary>
    public partial class RequestsStatistisYearly : Page
    {
        public YearlyTourStatistics viewModel;
        public RequestsStatistisYearly(ObservableCollection<TourInstance> todayInstances,User loggedUser,ObservableCollection<TourInstance>futureInstances)
        {
            InitializeComponent();
            viewModel = new YearlyTourStatistics(todayInstances,loggedUser,futureInstances );
            DataContext = viewModel;
        }
        private void ComboBoxCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.ComboBoxCountry_SelectionChanged();
        }
    }
}
