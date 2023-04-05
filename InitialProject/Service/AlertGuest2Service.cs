﻿using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{

    public class AlertGuest2Service
    {
        private AlertGuest2Repository alertGuest2Repository;
        public AlertGuest2Service() 
        {
            alertGuest2Repository = new AlertGuest2Repository();
        }

        public List<AlertGuest2> GetAll()
        {
            return alertGuest2Repository.GetAll();
        }

        public AlertGuest2 Save(AlertGuest2 alert)
        {
            
            return alertGuest2Repository.Save(alert);
        }

        public AlertGuest2 Update(AlertGuest2 alert)
        {
            return alertGuest2Repository.Update(alert);
        }

        public void Delete(AlertGuest2 alert)
        {
            alertGuest2Repository.Delete(alert);
        }

        public List<AlertGuest2> GetByInstanceIdAndGuestId(int instanceId, int guestId)
        {

            return alertGuest2Repository.GetByInstanceIdAndGuestId(instanceId,guestId);
        }

    }

}