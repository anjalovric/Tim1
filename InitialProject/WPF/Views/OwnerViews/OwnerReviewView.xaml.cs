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
using InitialProject.WPF.ViewModels;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for OwnerReviewView.xaml
    /// </summary>
    public partial class OwnerReviewView : Page
    {
        private OwnerReviewViewModel ownerReviewViewModel;
        public OwnerReviewView(OwnerReview ownerReview)
        {
            InitializeComponent();
            ownerReviewViewModel = new OwnerReviewViewModel(ownerReview);
            DataContext = ownerReviewViewModel;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = NavigationService.GoBack;
        }

        private void NextImageButton_Click(object sender, RoutedEventArgs e)
        {
            ownerReviewViewModel.GetNextImage();
        }

        private void PreviousImageButton_Click(object sender, RoutedEventArgs e)
        {
            ownerReviewViewModel.GetPreviousImage();
        }
    }
}