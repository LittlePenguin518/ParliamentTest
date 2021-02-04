using System;
using System.Collections.Generic;

namespace UKParliament.CodeTest.Services.Models
{
    public class RoomBookingAvailabilityInfo
    {
        public DateTime BookingDateTimeStart { get; set; }

        public DateTime BookingDateTimeEnd { get; set; }

        public ICollection<RoomInfo> RoomAvailable { get; set; }
    

    }

}