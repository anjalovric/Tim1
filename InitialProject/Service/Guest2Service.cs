using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class Guest2Service
    {
        private Guest2Repository repository;
        public Guest2Service() 
        {
            repository = new Guest2Repository();
        }
        public List<Guest2> GetAll()
        {
            return repository.GetAll();
        }
        public Guest2 GetByUsername(string userName)
        {
            return repository.GetAll() .Find(n => n.Username.Equals(userName));
        }
        public Guest2 GetById(int id)
        {
            return repository.GetAll().Find(n => n.Id == id);
        }
    }
}
