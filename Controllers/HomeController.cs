using ASPNETLIVE.Services.ThaiDate;
using Microsoft.AspNetCore.Mvc;

namespace ASPNETLIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IThaiDate _thaiDate;

        public HomeController(IThaiDate thaiDate) // inject
        {
            _thaiDate = thaiDate;
        }

        [HttpGet]
        public IActionResult Get ()
        {
            var myThaiDate = _thaiDate.ShowThaiDate();
            return Ok(new { message = $"Hello My API {myThaiDate}" });
        }
    }
}
