using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Services.Models;

namespace UKParliament.CodeTest.Web.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("[controller]")]
    public class RoomController : ControllerBase
    {

        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("{RoomId}")]
        public async Task<ActionResult<RoomInfo>> GetRoom(int RoomId)
        {
            var room = await _roomService.GetAsync(RoomId);
            if (room.Name != "Error")
            {
                return Ok(room);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("{RoomId}")]
        public async Task<IActionResult> CreateRoom(RoomInfo room)
        {
            var result = await _roomService.Create(room);
            if (result.Name.Contains("Error"))
            {

                return BadRequest();

            }
            if (result.Name.Contains("exist") == true)
            {
                return StatusCode(409, result.Name);
            }
            else
            {
                return Ok(result);
            }

        }

        [HttpPut("{RoomId}")]
        public async Task<IActionResult> UpdateRoom(RoomInfo room)
        {

            if (room.Id == 0 || string.IsNullOrEmpty(room.Name))
            {

                return BadRequest();
            }
            else
            {
                var result = await _roomService.Update(room);


                if (result.Name.Contains("exist") || result.Name.Contains("Error"))
                {
                    return StatusCode(409, result.Name);
                }

                else
                {
                    return Ok(result);
                }
            }
        }

        [HttpDelete("{RoomId}")]
        public async Task<IActionResult> DeleteRoom(int RoomId, bool shiftAllExistingBooking, string RoomName)
        {
            if (RoomId > 0)
            {
                var result = await _roomService.Delete(RoomId, shiftAllExistingBooking, RoomName);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

    }
}