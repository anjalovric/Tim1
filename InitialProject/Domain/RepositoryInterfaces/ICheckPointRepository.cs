﻿using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ICheckPointRepository:IGenericRepository<CheckPoint>
    {

        public CheckPoint Save(CheckPoint checkPoint);

        public CheckPoint Update(CheckPoint checkPoint);

        public void Delete(CheckPoint checkPoint);
        public List<CheckPoint> GetByInstance(int tourId);
    }
}
