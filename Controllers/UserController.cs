using Microsoft.AspNetCore.Mvc;
using RandomDataGenerator.Code;

namespace RandomDataGenerator.Controllers
{
    [ApiController]
    [Route("api/persons")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetPersons(int seed, double errorProb, Region locale = Region.EN, int page = 1, int size = 10)
        {
            if (!Enum.IsDefined(locale))
                return BadRequest(new { Message = "The locale is not defined." });

            var dataGen = FakeDataGenerator.Create(seed, locale);
            return Ok(dataGen.Generate(page, size, errorProb));
        }
    }
}
