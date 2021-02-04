using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Services.Models;

namespace UKParliament.CodeTest.Web.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("[controller]")]
    public class ViewAvailabilityController : ControllerBase
    {
        private readonly IRoomBookingService _roomBookingService;

        public ViewAvailabilityController(IRoomBookingService roomBookingService)
        {
            _roomBookingService = roomBookingService;
        }

        [HttpGet("{BookingDateTimeStart}&{dateTimeEnd}")]
        public async Task<ActionResult<RoomBookingAvailabilityInfo>> GetFreeRoom(DateTime dateTimeStart, DateTime dateTimeEnd)
        {
            var roombooking = await _roomBookingService.GetBookingAvailability(dateTimeStart, dateTimeEnd);
            return roombooking;
        }

    }
}