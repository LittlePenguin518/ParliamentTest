using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Services.Models;

namespace UKParliament.CodeTest.Web.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;
        private ControllerBase personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;

        }

        

        [HttpGet("{personId}")]
        public async Task<ActionResult<PersonInfo>> GetPerson(int personId)
        {

            var person = await _personService.GetAsync(personId);
            if (person.DateOfBirth != "Error")
            {
                return Ok(person);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("{personId}")]

        public async Task<IActionResult> CreatePerson(PersonInfo person)
        {

            var result = await _personService.Create(person);
            if (result.Name != "Error")
            {

                return Ok(result);

            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{personId}")]
        public async Task<IActionResult> UpdatePerson(PersonInfo person)
        {


            if (person.Id == 0 || string.IsNullOrEmpty(person.Name) || string.IsNullOrEmpty(person.DateOfBirth))
            {

                return BadRequest();
            }
            else
            {
                var result = await _personService.Update(person);
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

        }
        [HttpDelete("{personId}")]
        public async Task<IActionResult> Delete(int personId)
        {
            if (personId > 0)
            {
                var result = await _personService.Delete(personId);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
