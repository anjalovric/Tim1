﻿using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class GuideRepository
    {
        private const string FilePath = "../../../Resources/Data/guides.csv";

        private readonly Serializer<Guide> _serializer;

        private List<Guide> guides;

        public GuideRepository()
        {
            _serializer = new Serializer<Guide>();
            guides = _serializer.FromCSV(FilePath);
        }

        public List<Guide> GetAll()
        {
            return guides;
        }

        public Guide GetByUsername(string username)
        {
            Guide foundGuide = null;
            foreach(Guide guide in guides)
            {
                if(guide.Username == username)
                {
                    foundGuide = guide;
                }
            }
            return foundGuide;
        }

        public Guide GetById(int id) 
        {
            return guides.Find(x => x.Id == id);
        }

        public int NextId()
        {
            guides = _serializer.FromCSV(FilePath);
            if (guides.Count < 1)
            {
                return 1;
            }
            return guides.Max(c => c.Id) + 1;
        }
    }
}
