using System.Collections.Generic;
using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;

namespace InitialProject.Service
{
    public class AccommodationImageService
    {
        private IAccommodationImageRepository accommodationImageRepository = Injector.CreateInstance<IAccommodationImageRepository>();
        public AccommodationImageService()
        {
        }

        public List<AccommodationImage> GetAll()
        {
            return accommodationImageRepository.GetAll();
        }
        public int Add(AccommodationImage image)
        {
            return accommodationImageRepository.Add(image);
        }

        public AccommodationImage GetCoverImage(Accommodation accommodation)
        {
            return GetAll().Find(image => image.Accommodation.Id == accommodation.Id);
        }
        public List<AccommodationImage> GetAllByAccommodation(Accommodation accommodation)
        {
            List<AccommodationImage> images = new List<AccommodationImage>();
            foreach(var image in accommodationImageRepository.GetAll())
            {
                if(image.Accommodation.Id == accommodation.Id)
                {
                    images.Add(image);
                }
            }
            return images;
        }
    }
}
