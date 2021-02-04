using System;


namespace UKParliament.CodeTest.Data.Domain
{
    public class RoomBooking
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int RoomId { get; set; }

        public DateTime BookingDateTimeStart { get; set; }

        public int lengthBookingMin { get; set; }

        public DateTime BookingDateTimeEnd { get; set; }

        public string BookingNote { get; set; }

    }
}