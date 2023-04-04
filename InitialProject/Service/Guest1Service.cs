using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    public class Guest1Service
    {
        private Guest1Repository guest1Repository;

        public Guest1Service()
        {
            guest1Repository = new Guest1Repository();
        }

        public List<Guest1> GetAll()
        {
            return guest1Repository.GetAll();
        }

        public Guest1 GetByUsername(String username)
        {
            return guest1Repository.GetByUsername(username);
        }
    }

}
