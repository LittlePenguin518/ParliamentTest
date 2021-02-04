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
    public interface IRoomBookingService
    {
        Task<RoomBookingInfo> GetAsync(int BookingId);
        Task<RoomBookingAvailabilityInfo> GetBookingAvailability(DateTime dateTimeSearchS, DateTime dateTimeSearchE);
        Task SaveChangesAsync();
        Task<RoomBookingInfo> Create(RoomBookingInfo roomBooking);
        Task<bool> Delete(int RoomBookingId);
        Task<string> Update(RoomBooking roomBooking);

    }
    public class RoomBookingService : IRoomBookingService
    {
        private readonly RoomBookingsContext _repository;
        public RoomBookingService(RoomBookingsContext repository)
        {
            _repository = repository;
        }
        public async Task<RoomBookingInfo> GetAsync(int BookingId)
        {
            var result = await _repository.RoomBookings.FindAsync(BookingId);
            if (result != null)
            {
                var getRoomName = _repository.Room.FirstOrDefault(z => z.Id == result.RoomId);
                var getPersonDetail = _repository.People.FirstOrDefault(x => x.Id == result.PersonId);

                RoomBookingInfo roomBookingFound = new RoomBookingInfo
                {
                    Id = result.Id,
                    PersonName = getPersonDetail.Name,
                    PersonDOB = getPersonDetail.DateOfBirth,
                    RoomName = getRoomName.Name,
                    BookingDateTimeStart = result.BookingDateTimeStart,
                    lengthBookingMin = result.lengthBookingMin,
                    BookingNote = result.BookingNote
                };
                return roomBookingFound;
            }
            else
            {
                RoomBookingInfo roomBookingNotFound = new RoomBookingInfo
                {
                    Id = BookingId,
                    //  BookingDate = "Error"

                };
                return roomBookingNotFound;
            }
        }
        public async Task<RoomBookingAvailabilityInfo> GetBookingAvailability(DateTime dtSearchStart, DateTime dtSearchEnd)
        {
            RoomBookingAvailabilityInfo AvailableRoom = new RoomBookingAvailabilityInfo();
            AvailableRoom.BookingDateTimeStart = dtSearchStart;
            AvailableRoom.BookingDateTimeEnd = dtSearchEnd;

            // define all booking within the given time
            List<RoomBooking> BookingWithinPeriod = _repository.RoomBookings.Where(r => r.BookingDateTimeStart > dtSearchStart && r.BookingDateTimeEnd < dtSearchEnd).ToList();

            // get all room
            List<Room> allRoom = await _repository.Room.ToListAsync();


            // get all room that is not booked within the given time
            List<RoomInfo> FreeRoomName = new List<RoomInfo>();


            foreach (Room room in allRoom)
            {

                string a = Convert.ToString(room.Id);
                // check if the room name is within the list of existing booking in the given time period
                var check = BookingWithinPeriod.FirstOrDefault(y => y.RoomId == room.Id);

                // seperate the free rooms and non-free rooms
                if (check == null)
                {
                    RoomInfo ConvertRoom = new RoomInfo()
                    { Id = room.Id, Name = room.Name };

                    FreeRoomName.Add(ConvertRoom);

                }

            }

            AvailableRoom.RoomAvailable = FreeRoomName;

            return AvailableRoom;
        }
        public async Task<RoomBookingInfo> Create(RoomBookingInfo roomBooking)
        {
            var CheckBookingIdExist = await _repository.RoomBookings.FindAsync(roomBooking.Id);

            // Get the Person detail in Person Table with Id
            Person getPersonDetail = new Person();
            getPersonDetail = _repository.People.FirstOrDefault(z => z.Name == roomBooking.PersonName && z.DateOfBirth == roomBooking.PersonDOB);

            // Get the room detail in Room Table with Id
            Room getRoomDetail = new Room();
            getRoomDetail = _repository.Room.FirstOrDefault(y => y.Name == roomBooking.RoomName);

            // Pass error message
            RoomBookingInfo ErrorRoom = new RoomBookingInfo
            {
                Id = roomBooking.Id,

            };


            if (CheckBookingIdExist == null)
            {
                if (getPersonDetail == null)
                {
                    ErrorRoom.BookingNote = "Error in creating record. Person record does not exist";
                    return ErrorRoom;
                }
                if (getRoomDetail == null)
                {
                    ErrorRoom.BookingNote = "Error in creating record. Room Name does not exist";
                    return ErrorRoom;
                }
                else
                {

                    roomBooking.BookingDateTimeEnd = roomBooking.BookingDateTimeStart.AddMinutes(roomBooking.lengthBookingMin);
                    _repository.RoomBookings.Add(new RoomBooking
                    {
                        Id = roomBooking.Id,
                        PersonId = getPersonDetail.Id,
                        RoomId = getRoomDetail.Id,
                        BookingDateTimeStart = roomBooking.BookingDateTimeStart,
                        BookingDateTimeEnd = roomBooking.BookingDateTimeEnd,
                        lengthBookingMin = roomBooking.lengthBookingMin,
                        BookingNote = roomBooking.BookingNote
                    });

                    var numberOfItemsCreated = await _repository.SaveChangesAsync();

                    if (numberOfItemsCreated == 1)
                    {
                        return roomBooking;
                    }
                    else
                    {
                        ErrorRoom.BookingNote = "Error in creating record";
                        return ErrorRoom;
                    }
                }
            }
            else
            {

                ErrorRoom.BookingNote = "Booking id already exist";
                return ErrorRoom;
            }
        }

        public async Task<string> Update(RoomBooking booking)
        {

            var ExistingRoomBooking = await _repository.RoomBookings.FindAsync(booking.Id);

            if (ExistingRoomBooking != null)
            {
                try
                {
                    _repository.Entry(ExistingRoomBooking).State = EntityState.Detached;
                    _repository.RoomBookings.Attach(booking);
                    _repository.Entry(ExistingRoomBooking).State = EntityState.Modified;

                    var numberOfItemsUpdated = await _repository.SaveChangesAsync();

                    if (numberOfItemsUpdated == 1)
                    {
                        return "Success";
                    }
                    else
                    {
                        return "Error in updating booking record";
                    }


                }
                catch
                {
                    throw;
                }
            }
            else
            {


                return "Booking record does not exist";
            }
        }

        public async Task<bool> Delete(int RoomBookingId)
        {
            var ExistingBooking = await _repository.RoomBookings.FindAsync(RoomBookingId);


            if (ExistingBooking != null)
            {
                try
                {
                    _repository.RoomBookings.Remove(ExistingBooking);
                    var numberOfItemsDeleted = await _repository.SaveChangesAsync();

                    if (numberOfItemsDeleted == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    throw;
                }

            }

            else
            {
                return false;
            }
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }


    }
}
