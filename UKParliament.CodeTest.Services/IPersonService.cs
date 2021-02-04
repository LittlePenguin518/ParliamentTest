using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Domain;
using UKParliament.CodeTest.Services.Models;

namespace UKParliament.CodeTest.Services
{
    public interface IPersonService
    {
        Task<PersonInfo> GetAsync(int personId);
        Task SaveChangesAsync();
        Task<PersonInfo> Create(PersonInfo person);
        Task<PersonInfo> Update(PersonInfo person);
        Task<string> Delete(int personId);

    }

    public class PersonService : IPersonService
    {
        private readonly RoomBookingsContext _repository;

        public PersonService(RoomBookingsContext repository)
        {
            _repository = repository;


        }

        public async Task<PersonInfo> GetAsync(int personId)
        {

            var result = await _repository.People.FindAsync(personId);
            if (result != null)
            {
                PersonInfo personFound = new PersonInfo
                {
                    Id = result.Id,
                    Name = result.Name,
                    DateOfBirth = result.DateOfBirth
                };
                return personFound;
            }
            else
            {
                PersonInfo personNotFound = new PersonInfo
                {
                    Id = personId,
                    Name = "Error",
                    DateOfBirth = "Error"
                };
                return personNotFound;
            }

        }
        public async Task<PersonInfo> Create(PersonInfo person)
        {
            _repository.People.Add(new Data.Domain.Person
            { Id = person.Id, Name = person.Name, DateOfBirth = person.DateOfBirth });

            var numberOfItemsCreated = await _repository.SaveChangesAsync();

            if (numberOfItemsCreated == 1)
            {
                return person;
            }
            else
            {
                PersonInfo ErrorAddPerson = new PersonInfo
                {
                    Id = person.Id,
                    Name = "Error",
                    DateOfBirth = "Error"
                };
                return ErrorAddPerson;
            }

        }
        public async Task<PersonInfo> Update(PersonInfo person)
        {
            var ExistingPerson = await _repository.People.FindAsync(person.Id);
            PersonInfo ErrorUpdatePerson = new PersonInfo
            {
                Id = person.Id,
                Name = "Error",
                DateOfBirth = "Error"
            };

            if (ExistingPerson != null)
            {
                try
                {
                    Person NewUpdatePersonDetail = new Person
                    {

                        Id = person.Id,
                        Name = person.Name,
                        DateOfBirth = person.DateOfBirth
                    };

                    _repository.Entry(ExistingPerson).State = EntityState.Detached;
                    _repository.People.Attach(NewUpdatePersonDetail);
                    _repository.Entry(NewUpdatePersonDetail).State = EntityState.Modified;

                    var numberOfItemsUpdated = await _repository.SaveChangesAsync();

                    if (numberOfItemsUpdated == 1)
                    {
                        return person;
                    }
                    else
                    {
                        return ErrorUpdatePerson;
                    }
                }
                catch
                {
                    throw;
                }

            }

            else
            {
                ErrorUpdatePerson.Name = "Record does not exist";

                return ErrorUpdatePerson;
            }
        }
        public async Task<string> Delete(int personId)
        {
            var ExistingPerson = await _repository.People.FindAsync(personId);

            //get all upcoming booking related to the person that will be deleted
            List<RoomBooking> PersonRelatedBookings = _repository.RoomBookings.Where(r => r.PersonId == personId && r.BookingDateTimeStart > DateTime.Now).ToList();

            if (ExistingPerson != null)
            {
                try
                {
                    if (PersonRelatedBookings.Count != 0)
                    {
                        /* When person in used return message to ask user to delete upcoming booking made by this person or we can perform 
                         * automatic deletion of upcoming booking made
                        by this person in this section*/
                        return "Cannot delete Person Id " + personId + ". Please delete all upcoming booking made by this person";
                    }
                    else
                    {
                        _repository.People.Remove(ExistingPerson);
                        var numberOfItemsDeleted = await _repository.SaveChangesAsync();

                        if (numberOfItemsDeleted == 1)
                        {
                            return "Record with Person Id " + personId + " has been deleted";
                        }
                        else
                        {
                            return "Error occured when trying to delete record with person Id " + personId;
                        }
                    }
                }
                catch
                {
                    throw;
                }

            }

            else
            {
                return "Record does not exist. Please enter Existing Room Id";
            }

        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }


    }
}