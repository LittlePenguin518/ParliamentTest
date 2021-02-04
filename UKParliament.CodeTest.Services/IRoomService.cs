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
    public interface IRoomService
    {
        Task<RoomInfo> GetAsync(int RoomId);
        Task SaveChangesAsync();
        Task<RoomInfo> Create(RoomInfo room);
        Task<RoomInfo> Update(RoomInfo room);
        Task<string> Delete(int RoomId, bool shiftBooking, string rName);


    }
    public class RoomService : IRoomService
    {
        private readonly RoomBookingsContext _repository;
        private readonly IRoomBookingService _roomBookingService;
        public RoomService(RoomBookingsContext repository)
        {
            _repository = repository;

        }

        public async Task<RoomInfo> GetAsync(int RoomId)
        {
            var result = await _repository.Room.FindAsync(RoomId);
            if (result != null)
            {
                RoomInfo roomFound = new RoomInfo
                {
                    Id = result.Id,
                    Name = result.Name,
                    //Bookings = result.Bookings
                };
                return roomFound;
            }
            else
            {
                RoomInfo roomNotFound = new RoomInfo
                {
                    Id = RoomId,
                    Name = "Error",

                };
                return roomNotFound;
            }
        }

        public async Task<RoomInfo> Create(RoomInfo room)
        {
            string tempName = room.Name;

            // make sure room id and name is unique
            var CheckRoomIdExist = await _repository.Room.FindAsync(room.Id);
            var CheckRoomNameExist = await _repository.Room.FirstOrDefaultAsync(e => e.Name == tempName);


            RoomInfo ErrorRoom = new RoomInfo
            {
                Id = room.Id,
                Name = "Error",

            };

            if (CheckRoomIdExist == null && CheckRoomNameExist == null)
            {
                _repository.Room.Add(new Room
                { Id = room.Id, Name = room.Name });

                var numberOfItemsCreated = await _repository.SaveChangesAsync();

                if (numberOfItemsCreated == 1)
                {
                    return room;
                }
                else
                {

                    return ErrorRoom;
                }
            }
            else
            {
                if (CheckRoomIdExist != null)
                {
                    ErrorRoom.Name = "Room id already exist";
                }
                if (CheckRoomNameExist != null)
                {
                    ErrorRoom.Name = "Room Name already exist. Room Name must be unique";
                }
                if (CheckRoomIdExist != null && CheckRoomNameExist != null)
                {
                    ErrorRoom.Name = "Room Id and Name already exist";
                }

                return ErrorRoom;
            }
        }
        public async Task<RoomInfo> Update(RoomInfo room)
        {

            var ExistingRoom = await _repository.Room.FindAsync(room.Id);


            RoomInfo ErrorUpdateRoom = new RoomInfo
            {
                Id = room.Id,
                Name = "Error",

            };

            if (ExistingRoom != null)
            {
                try
                {
                    Room NewUpdateRoomDetail = new Room();

                    string tempName = room.Name;

                    //check if the new room name is unique
                    var CheckRoomNameExist = _repository.Room.Where(e => e.Name == tempName && e.Id != room.Id).ToList().Count;

                    if (CheckRoomNameExist != 1)
                    {
                        NewUpdateRoomDetail.Id = room.Id;
                        NewUpdateRoomDetail.Name = room.Name;



                        _repository.Entry(ExistingRoom).State = EntityState.Detached;

                        _repository.Room.Attach(NewUpdateRoomDetail);
                        _repository.Entry(NewUpdateRoomDetail).State = EntityState.Modified;

                        var numberOfItemsUpdated = await _repository.SaveChangesAsync();

                        if (numberOfItemsUpdated == 1)
                        {
                            return room;
                        }
                        else
                        {
                            return ErrorUpdateRoom;
                        }
                    }

                    else
                    {
                        ErrorUpdateRoom.Name = "Room Name already exist. New Room Name must be Unique";
                        return ErrorUpdateRoom;
                    }
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                ErrorUpdateRoom.Name = "Record does not exist";

                return ErrorUpdateRoom;
            }
        }
        public async Task<string> Delete(int RoomId, bool shiftAllBookings, string rName)
        {
            var ExistingRoom = await _repository.Room.FindAsync(RoomId);

            //get all booking related to the room that will be deleted
            List<RoomBooking> RelatedBookings = _repository.RoomBookings.Where(r => r.RoomId == RoomId).ToList();

            // check if room exist
            if (ExistingRoom != null)
            {
                // check if user wish to shift all booking to another room
                if (shiftAllBookings == true)
                {
                    try
                    {
                        // move existing booking to another specified room before deleting
                        if (RelatedBookings.Count != 0)
                        {
                            // get room id for the newly specified room
                            int NewRoomId = _repository.Room.SingleOrDefault(n => n.Name == rName).Id;

                            // run through each booking of the to update new room
                            foreach (RoomBooking booking in RelatedBookings)
                            {
                                // get the booking
                                var getBooking = _repository.RoomBookings.FirstOrDefault(s => s.Id == booking.Id);

                                //assign the new specified room
                                getBooking.RoomId = NewRoomId;
                                //  getBooking.RoomDetail.Name = rName;

                                // update the booking detail
                                var result = await _roomBookingService.Update(getBooking);

                                if (result.Contains("Success") == false)
                                {
                                    // exit the loop and return error message when there is problem with updating booking record
                                    return "Error occured when trying to shift booking of current to the newly specified room";

                                }

                            }

                        }

                        // delete room 
                        _repository.Room.Remove(ExistingRoom);
                        var numberOfItemsDeleted = await _repository.SaveChangesAsync();

                        if (numberOfItemsDeleted == 1)
                        {
                            return "Record with Room Id " + RoomId + " has been deleted. All existing booking for this room has been automatically moved " +
                                "to the requested room";
                        }
                        else
                        {
                            return "Error occured when trying to delete record with Room Id " + RoomId;
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }

                // check if user select not to shift booking
                else
                {

                    if (RelatedBookings.Count != 0) // and if related booking exist. It will then return error message
                    {
                        // return error message if user did not choose to automatically shift all booking into the room which they specified.
                        return " Record cannot be deleted. This room has existing bookings that have not been moved or cancelled. Please set the option to shift booking" +
                           " automatically to true if you would like to shift all existing bookings automatically to another specified room. Otherwise, " +
                           "please manually move all booking and ensure that there is no more booking exist for this room before deleting";
                    }
                    else // if there is no related booking. Room will be deleted
                    {
                        _repository.Room.Remove(ExistingRoom);
                        var numberOfItemsDeleted = await _repository.SaveChangesAsync();

                        if (numberOfItemsDeleted == 1)
                        {
                            return "Record with Room Id " + RoomId + " has been deleted.";
                        }
                        else
                        {
                            return "Error occured when trying to delete record with Room Id " + RoomId;
                        }
                    }

                }
            }


            // return error message if room to update does not exist
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
