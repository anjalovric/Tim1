using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    public class AccommodationReviewImageService
    {
        private IAccommodationReviewImageRepository accommodationReviewImageRepository = Injector.CreateInstance<IAccommodationReviewImageRepository>();


        public AccommodationReviewImageService()
        {
        }

        public List<AccommodationReviewImage> GetAll()
        {
            return accommodationReviewImageRepository.GetAll();
        }
        public void Add(AccommodationReviewImage image)
        {
            accommodationReviewImageRepository.Add(image);
        }

        public void Delete(AccommodationReviewImage image)
        {
            accommodationReviewImageRepository.Delete(image);
        }

        public List<AccommodationReviewImage> GetAllByReservation(AccommodationReservation reservation)
        {
            List<AccommodationReviewImage> imagesForReservation = new List<AccommodationReviewImage>();

            foreach(var image in accommodationReviewImageRepository.GetAll())
            {
                if(image.Reservation.Id == reservation.Id)
                {
                    imagesForReservation.Add(image);
                }
            }
            return imagesForReservation;
        }

    }
}
