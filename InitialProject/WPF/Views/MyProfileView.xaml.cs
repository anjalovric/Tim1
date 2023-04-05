﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.ViewModels;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for MyProfileView.xaml
    /// </summary>
    public partial class MyProfileView : Page
    {
        private MyProfileViewModel myProfileViewModel;
        public MyProfileView(User user)
        {
            InitializeComponent();
            myProfileViewModel = new MyProfileViewModel(user);
            DataContext = myProfileViewModel;
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            object buttonDataContext = (sender as Button).DataContext;
            myProfileViewModel.SelectedOwnerReview = (OwnerReview)buttonDataContext;
            OwnerReviewView ownerReviewView = new OwnerReviewView(myProfileViewModel.SelectedOwnerReview);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = ownerReviewView;
        }
    }
}
