using Microsoft.VisualStudio.TestTools.UnitTesting;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Services.Models;
using UKParliament.CodeTest.Web.Controllers;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace UKParliament.CodeTest.Test
{
    [TestClass]
    public class RoomControllerTest
    {
        private readonly RoomController controller;
        private readonly IRoomService roomService;
        public RoomControllerTest()
        {

            controller = new RoomController(roomService);
        }

        [Fact]
        public void ShouldGetRoom()
        {
            // arrange
            int viewRoomId = 1;

            //act
            
            var result = controller.GetRoom(viewRoomId);

            // assert
            Assert.IsNotNull(result);

        }

        [Fact]
        public void ShouldAddRoom()
        {
            // arrange
            RoomInfo RequestAddRoom = new RoomInfo
            {
                Id = 1,
                Name = "TestDummyRoom1",
                

            };

            //act
            
            var result = controller.CreateRoom(RequestAddRoom);
            // assert
            Assert.IsNotNull(result);


        }

        [Fact]
        public void ShouldUpdateRoom()
        {
        
            // arrange
            RoomInfo RequestUpdateRoom = new RoomInfo
            {
                Id = 1,
                Name = "TestDummyRoomUpdated1",
  
            };

            //act
   
            var result = controller.UpdateRoom(RequestUpdateRoom);

            // assert
            Assert.IsNotNull(result);
           


        }

        [Fact]
        public void ShouldDeleteRoom()
        {
            // arrange
            int RoomId = 1;
            bool shiftbooking = true;
            string RoomNameToShift = "Rose";


            //act
           
            var result = controller.DeleteRoom(RoomId, shiftbooking,RoomNameToShift);

            // assert
            Assert.IsNotNull(result);



        }
    }
}
