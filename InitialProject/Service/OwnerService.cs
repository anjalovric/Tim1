using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    internal class OwnerService
    {
        private OwnerRepository ownerRepository;

        public OwnerService()
        {
            ownerRepository = new OwnerRepository();
        }

        public List<Owner> GetAll()
        {
            return ownerRepository.GetAll();
        }

        public Owner GetByUsername(String username)
        {
            return ownerRepository.GetByUsername(username);
        }
    }
}
