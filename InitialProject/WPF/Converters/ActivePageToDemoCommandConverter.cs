﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using InitialProject.WPF.Demo;

namespace InitialProject.WPF.Converters
{
    public class ActivePageToDemoCommandConverter : IValueConverter
    {
        public RelayCommand AccommodationInputCommand { get; set; }
        public RelayCommand ScheduleRenovationCommand { get; set; }
        public RelayCommand GuestReviewCommand { get; set; }
        public RelayCommand ForumReportCommentCommand { get; set; }
        public RelayCommand NewCommentCommand { get; set; }
        public RelayCommand MyProfileCommand { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MakeCommands();
            if(value is Page activePage)
            {
                if (activePage.Title.Equals("Enter Accommodation"))
                    return AccommodationInputCommand;
                if(activePage.Title.Equals("Schedule Renovation"))
                    return ScheduleRenovationCommand;
                if(activePage.Title.Equals("Guest Review"))
                    return GuestReviewCommand;
                if(activePage.Title.Equals("Forums"))
                    return ForumReportCommentCommand;
                if(activePage.Title.Equals("Forum"))
                    return NewCommentCommand;
                if (activePage.Title.Equals("My Profile"))
                    return MyProfileCommand;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private void MakeCommands()
        {
            AccommodationInputCommand = new RelayCommand(AccommodationInput_Executed, CanExecute);
            ScheduleRenovationCommand = new RelayCommand(ScheduleRenovation_Executed, CanExecute);
            GuestReviewCommand = new RelayCommand(GuestReviw_Executed, CanExecute);
            ForumReportCommentCommand = new RelayCommand(ForumReportComment_Executed, CanExecute);
            NewCommentCommand = new RelayCommand(NewComment_Executed, CanExecute);
            MyProfileCommand = new RelayCommand(MyProfile_Executed, CanExecute);
        }

        private bool CanExecute(object sender)
        {
            return true;
        }

        private void AccommodationInput_Executed(object sender)
        {
            AccommodationInputDemo accommodationInputDemo = new AccommodationInputDemo();
            accommodationInputDemo.PlayDemo();
        }

        private void ScheduleRenovation_Executed(object sender)
        {
            ScheduleRenovationDemo scheduleRenovationDemo = new ScheduleRenovationDemo();
            scheduleRenovationDemo.PlayDemo();
        }

        private void GuestReviw_Executed(object sender)
        {
            GuestReviewDemo guestReviewDemo = new GuestReviewDemo();
            guestReviewDemo.PlayDemo();
        }

        private void ForumReportComment_Executed(object sender)
        {
            ForumReportCommentDemo reportCommentDemo = new ForumReportCommentDemo();
            reportCommentDemo.PlayDemo();
        }

        private void NewComment_Executed(object sender)
        {
            NewCommentDemo newCommentDemo = new NewCommentDemo();
            newCommentDemo.PlayDemo();
        }

        private void MyProfile_Executed(object sender)
        {
            MyProfileDemo profileDemo = new MyProfileDemo();
            profileDemo.PlayDemo();
        }
    }
}