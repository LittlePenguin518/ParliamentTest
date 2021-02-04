using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Services.Models;
using UKParliament.CodeTest.Web.Controllers;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace UKParliament.CodeTest.Test
{
    [TestClass]
    public class RoomBookingControllerTest
    {
        private readonly RoomBookingController controller;
        private readonly IRoomBookingService roomBookingService;

        public RoomBookingControllerTest()
        {

            controller = new RoomBookingController(roomBookingService);
        }

        [Fact]
        public void ShouldGetBooking()
        {
            // arrange
            int BookingId = 1;

            //act
            
            var result = controller.GetBooking(BookingId);

            // assert
            Assert.IsNotNull(result);



        }

        [Fact]
        public void ShouldAddBooking()
        {
            // arrange
            RoomBookingInfo RequestAddBooking = new RoomBookingInfo
            {
               
                Id = 2,
                PersonName = "DummyName",
                PersonDOB= Convert.ToString(DateTime.Now.AddYears(-20)),
                RoomName = "DummyRoom1",
                BookingDateTimeStart = DateTime.Now,
               
                lengthBookingMin = 50,
                BookingDateTimeEnd = DateTime.Now.AddMinutes(50),
                BookingNote = "test"

            };

            //act
            
            var result = controller.CreateRoomBooking(RequestAddBooking);

            // assert
            Assert.IsNotNull(result);


        }

        
        [Fact]
        public void ShouldDeleteBooking()
        {
            // arrange
            int RoomBookingId = 1;

            //act
          
            var result = controller.DeleteRoomBooking(RoomBookingId);

            // assert
            Assert.IsNotNull(result);



        }
    }
}
