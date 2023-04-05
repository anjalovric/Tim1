﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    public class AccommodationService
    {
        private AccommodationRepository accommodationRepository;
        private List<Accommodation> accommodations;
        public AccommodationService()
        {
            accommodationRepository = new AccommodationRepository();
            MakeAccommodations();
        }

        public List<Accommodation> GetAll()
        {
            return accommodationRepository.GetAll();
        }



        public void Add(Accommodation accommodation)
        {
            accommodationRepository.Add(accommodation);
        }

        public Accommodation GetById(int id)
        {
            return accommodationRepository.GetById(id);
        }

            private void MakeAccommodations()
            {
                accommodations = accommodationRepository.GetAll();
                AddOwners();
                AddLocations();
                AddTypes();
            }


            private void AddOwners()
            {
                OwnerService ownerService = new OwnerService();
                List<Owner> allOwners = ownerService.GetAll();
                foreach (Accommodation accommodation in accommodations)
                {
                    Owner accommodationOwner = allOwners.Find(n => n.Id == accommodation.Owner.Id);
                    if (accommodationOwner != null)
                    {
                        accommodation.Owner = accommodationOwner;
                    }
                }
            }

            private void AddLocations()
            {
                LocationService locationService = new LocationService();
                List<Location> allLocations = locationService.GetAll();
                foreach (Accommodation accommodation in accommodations)
                {
                    Location accommodationLocation = allLocations.Find(n => n.Id == accommodation.Location.Id);
                    if (accommodationLocation != null)
                    {
                        accommodation.Location = accommodationLocation;
                    }
                }
            }

            private void AddTypes()
            {
                AccommodationTypeService accommodationTypeService = new AccommodationTypeService();
                List<AccommodationType> allTypes = accommodationTypeService.GetAll();
                foreach(Accommodation accommodation in accommodations)
                {
                    AccommodationType accommodationType = allTypes.Find(n => n.Id == accommodation.Type.Id);
                    if(accommodationType != null)
                    {
                        accommodation.Type = accommodationType;
                    }
                }
            }
            public ObservableCollection<Accommodation> GetAllByOwner(Owner owner)
            {
                ObservableCollection<Accommodation> accommodationsByOwner = new ObservableCollection<Accommodation>();

                foreach (Accommodation accommodation in accommodations)
                {
                    if (accommodation.Owner.Id == owner.Id)
                    {
                        accommodationsByOwner.Add(accommodation);
                    }
                }
                return accommodationsByOwner;
            }
        }
    }

