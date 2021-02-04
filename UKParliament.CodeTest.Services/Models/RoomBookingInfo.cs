using System;

namespace UKParliament.CodeTest.Services.Models
{
    public class RoomBookingInfo
    {
        public int Id { get; set; }

        public string PersonName { get; set; }

        public string PersonDOB { get; set; }

        public string RoomName { get; set; }

        public DateTime BookingDateTimeStart { get; set; }

        public int lengthBookingMin { get; set; }

        public DateTime BookingDateTimeEnd { get; set; }

        public string BookingNote { get; set; }


    }

}