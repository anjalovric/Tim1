﻿using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class TourInstanceRepository
    {
        private const string FilePath = "../../../Resources/Data/tourInstances.csv";
       

        private readonly Serializer<TourInstance> _serializer;
     

        
        private List<TourInstance> _tourInstances;
       

        public TourInstanceRepository()
        {

            _serializer = new Serializer<TourInstance>();
            

            _tourInstances = _serializer.FromCSV(FilePath);
            
           
        }
        
        public List<TourInstance> GetAll()
        {
            return _tourInstances;
        }




        public TourInstance Save(TourInstance tour)
        {
            tour.Id = NextId();
            _tourInstances = _serializer.FromCSV(FilePath);
            _tourInstances.Add(tour);
            _serializer.ToCSV(FilePath, _tourInstances);
            return tour;
        }

        public int NextId()
        {
            _tourInstances = _serializer.FromCSV(FilePath);
            if (_tourInstances.Count < 1)
            {
                return 1;
            }
            return _tourInstances.Max(c => c.Id) + 1;
        }

        public void Delete(TourInstance tour)
        {
            _tourInstances = _serializer.FromCSV(FilePath);
            TourInstance founded = _tourInstances.Find(c => c.Id == tour.Id);
            _tourInstances.Remove(founded);
            _serializer.ToCSV(FilePath, _tourInstances);
        }

        public TourInstance Update(TourInstance tour)
        {
            _tourInstances = _serializer.FromCSV(FilePath);
            TourInstance current = _tourInstances.Find(c => c.Id == tour.Id);
            int index = _tourInstances.IndexOf(current);
            _tourInstances.Remove(current);
            _tourInstances.Insert(index, tour);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _tourInstances);
            return tour;
        }
        public List<TourInstance> GetByStart()
        {

            _tourInstances = _serializer.FromCSV(FilePath);
            List<TourInstance> list = new List<TourInstance>();
            foreach (TourInstance tour in _tourInstances)

                if(tour.StartDate.Date==DateTime.Now.Date && tour.Finished==false) 
                {   
                    string h = tour.StartClock.Split(':')[0];
                    string m = tour.StartClock.Split(":")[1];
                    string s = tour.StartClock.Split(":")[2];


                    if(Convert.ToInt32(h)> DateTime.Now.Hour)
                    {
                        list.Add(tour);
                    }else if(Convert.ToInt32(h)==DateTime.Now.Hour && Convert.ToInt32(m)>DateTime.Now.Minute)
                    {
                        list.Add(tour);
                    }else if(Convert.ToInt32(h)==DateTime.Now.Hour && Convert.ToInt32(m)==DateTime.Now.Minute && Convert.ToInt32(s) >DateTime.Now.Second)
                    {
                        list.Add(tour);
                    }
                }


            return list;
    
        }



        public List<TourInstance> GetInstancesLaterThan48hFromNow()
        {

            _tourInstances = _serializer.FromCSV(FilePath);
            List<TourInstance> list = new List<TourInstance>();
            foreach (TourInstance tour in _tourInstances)
            {
                if (tour.Finished == false && tour.StartDate>DateTime.Now.Date)
                {
                    var prevDate = Convert.ToDateTime(tour.StartDate.ToString().Split(" ")[0] +" "+ tour.StartClock);
                    var today = DateTime.Now;
                    var diffOfDates = today - prevDate;

                    if (diffOfDates.Days < -2 )
                        list.Add(tour);
                    else if (diffOfDates.Days == -2 && diffOfDates.Hours<0 )
                        list.Add (tour);
                    else if(diffOfDates.Days == -2 && diffOfDates.Hours==0 && diffOfDates.Minutes<0 )
                        list.Add (tour);
            }

            }
            return list;

        }

        public TourInstance GetById(int id)
        {
            return _tourInstances.Find(c => c.Id == id);
        }
        
    }
}
