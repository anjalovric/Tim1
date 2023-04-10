using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repository;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain
{
    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
        {
        { typeof(ICheckPointRepository), new CheckPointRepository() },
        { typeof(ITourRepository), new TourRepository() },
        { typeof(IVoucherRepository), new VoucherRepository() },
        { typeof(ITourInstanceRepository), new TourInstanceRepository() },
        { typeof(ITourReservationRepository), new TourReservationRepository() },
        { typeof(IAlertGuest2Repository), new AlertGuest2Repository() },
        { typeof(ILocationRepository), new LocationRepository() },
        { typeof(ITourImageRepository), new TourImageRepository() },
        { typeof(IGuideRepository), new GuideRepository() },
        { typeof(IOwnerRepository), new OwnerRepository() },
        { typeof(IUserRepository), new UserRepository() },
        { typeof(IGuideAndTourReviewsRepository), new GuideAndTourReviewRepository() },
        { typeof(ITourReviewImageRepository), new TourReviewImageRepository() }
    //    { typeof(TourDetailsService), new TourDetailsService() },
  //       { typeof(TourInstanceService), new TourInstanceService() }
        };

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            if (_implementations.ContainsKey(type))
            {
                return (T)_implementations[type];
            }

            throw new ArgumentException($"No implementation found for type {type}");
        }
    }
}
