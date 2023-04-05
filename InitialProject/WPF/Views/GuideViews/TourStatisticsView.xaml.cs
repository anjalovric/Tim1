﻿using InitialProject.Model;
using InitialProject.Service;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
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
using System.Net;
using System.IO;
using System.DirectoryServices.ActiveDirectory;
using Image = System.Windows.Controls.Image;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for TourStatisticsView.xaml
    /// </summary>
    public partial class TourStatisticsView : Page
    {
        public int Year { get; set; }
        public ObservableCollection<TourInstance> Instances { get; set; }
        public ObservableCollection<Image> Photos { get; set; }


        private TourInstance selected;
        private TourHistoryService historyService;
        public TourInstance Selected
        {
            get { return selected; }
            set
            {
                if (value != selected)
                    selected = value;

                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public TourStatisticsView()
        {
            InitializeComponent();
            DataContext = this;

            Instances = new ObservableCollection<TourInstance>();
            Photos = new ObservableCollection<Image>();
            historyService = new TourHistoryService();
            
            historyService.SetLocationToTour();
            historyService.SetTourToTourInstance();
            historyService.GetFinishedInsatnces(Instances);
            SetImage();
        }

        private void SetImage()
        {
            foreach(TourInstance tourInstance in Instances)
            {
                string url = historyService.FindFirstImageOfTour(tourInstance.Tour.Id);
                Image image = new Image();
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(url); ;
                bitmapImage.EndInit();
                image.Source = bitmapImage;
                Photos.Add(image);

            }
        }
        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            TourInstance currentTourInstance = (TourInstance)TourListDataGrid.CurrentItem;
            CheckPointDetails checkPointDetails = new CheckPointDetails(currentTourInstance);
            checkPointDetails.Show();

        }

        private void MostVisited_Click(object sender, RoutedEventArgs e)
        {
            CheckPointDetails checkPointDetails = new CheckPointDetails(historyService.FindMostVisited());
            checkPointDetails.Show();
        }


        private void MostVisitedForYear_Click(object sender, RoutedEventArgs e)
        {

            if (Year > 0)
            {
                CheckPointDetails checkPointDetails = new CheckPointDetails(historyService.FindMostVisitedForChosenYear(Year));
                checkPointDetails.Show();
                ChosenYear.Text = null;
            }
        }
    }
}