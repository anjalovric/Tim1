using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using InitialProject.WPF.Views;

namespace InitialProject.WPF.Demo
{
    public class AccommodationInputDemo
    {
        private AccommodationInputFormViewModel viewModel;
        public AccommodationInputDemo()
        {
            OwnerService ownerService = new OwnerService();
            Owner owner = ownerService.GetAll()[0];
            AccommodationInputFormView accommodationInputFormView = new AccommodationInputFormView(owner);
            viewModel = accommodationInputFormView.formViewModel;
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = accommodationInputFormView;
        }

        public void PlayDemo()
        {
            AccommodationImageService imageService = new AccommodationImageService();
            viewModel.Name = "My accommodation";
            Thread.Sleep(2000);
            viewModel.Location.Country = viewModel.Countries[0];
            Thread.Sleep(1000);
            viewModel.Location.City = "Istanbul";
            Thread.Sleep(1000);
            viewModel.Type = viewModel.AccommodationTypes[0];
            Thread.Sleep(1000);
            viewModel.Capacity = 2;
            Thread.Sleep(1000);
            viewModel.MinDaysForReservation = 1;
            Thread.Sleep(1000);
            viewModel.MinDaysToCancel = 1;
            Thread.Sleep(1000);
            viewModel.Images.Add(imageService.GetAll()[0]);
            viewModel.ImageUrl = imageService.GetAll()[0].Url;
        }
    }
}
