using System.Collections.Generic;
using System.Linq;


namespace UKParliament.CodeTest.Data.Domain
{
    public class Room
    {

        public int Id { get; set; }

        public string Name { get; set; }



        public ICollection<RoomBooking> AllBookings { get; set; }
        = new List<RoomBooking>().ToList();



    }
}