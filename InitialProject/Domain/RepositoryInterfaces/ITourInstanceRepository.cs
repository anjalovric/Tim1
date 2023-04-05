﻿using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourInstanceRepository:IGenericRepository<TourInstance>
    {
        public List<TourInstance> GetByStart();

        public List<TourInstance> GetInstancesLaterThan48hFromNow();


        public TourInstance Save(TourInstance tour);


        public void Delete(TourInstance tour);

        public void Update(TourInstance tour);

    }
}