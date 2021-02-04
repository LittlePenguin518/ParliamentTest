using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Services.Models;

namespace UKParliament.CodeTest.Web.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("[controller]")]
    public class RoomBookingController : ControllerBase
    {
        private readonly IRoomBookingService _roomBookingService;

        public RoomBookingController(IRoomBookingService roomBookingService)
        {
            _roomBookingService = roomBookingService;
        }

        [HttpGet("{RoomBookingId}")]
        public async Task<ActionResult<RoomBookingInfo>> GetBooking(int roomBookingId)
        {
            RoomBookingInfo booking = await _roomBookingService.GetAsync(roomBookingId);
            if (booking.BookingNote != "Error")
            {
                return Ok(booking);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost("{RoomBookingId}")]
        public async Task<IActionResult> CreateRoomBooking(RoomBookingInfo roomBooking)
        {
            if (roomBooking.lengthBookingMin <= 60)
            {
                var result = await _roomBookingService.Create(roomBooking);

                if (result.BookingNote.Contains("exist") == true || result.BookingNote.Contains("Error") == true)
                {
                    return StatusCode(409, result.BookingNote);
                }
                else
                {
                    return Ok(result);
                }


            }
            else
            {
                return StatusCode(409, "Booking room time is maximum at 1 hour");
            }
        }

        [HttpDelete("{RoomBookingId}")]
        public async Task<IActionResult> DeleteRoomBooking(int roomBookingId)
        {

            if (roomBookingId > 0)
            {
                var result = await _roomBookingService.Delete(roomBookingId);

                return Ok();
            }
            else
            {
                return BadRequest("error update roomBooking");
            }
        }


    }
}