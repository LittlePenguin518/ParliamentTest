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
    public class ViewAvailabilityControllerTest
    {
        
        private readonly IRoomBookingService roomBookingService;
        private readonly ViewAvailabilityController controller;
     
        public ViewAvailabilityControllerTest()
        {

            controller = new ViewAvailabilityController(roomBookingService);
        }
        [Fact]
        public void ShouldViewAvailability()
        {
            // arrange
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now.AddDays(1);

            // act
            var controller = new ViewAvailabilityController(roomBookingService);
            var result = controller.GetFreeRoom(start,end);

            // assert
            Assert.IsNotNull(result);



        }
    }
}
