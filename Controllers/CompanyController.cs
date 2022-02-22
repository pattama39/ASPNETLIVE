using Microsoft.AspNetCore.Mvc;

namespace ASPNETLIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        // https://localhost:7000/api/company
        [Route("")]
        [HttpGet]
        public IActionResult Hello()
        {
            return Ok(new { message = "Hello .NET" });
        }

        // https://localhost:7000/api/company/show/1
        [Route("show/{id}")]
        [HttpGet]
        public IActionResult Show(int id)
        {
            return Ok(new { message = $"Hello {id}" });
        }

        // https://localhost:7000/api/company/search?name=cct&age=10
        [Route("search")]
        [HttpGet]
        public IActionResult Search(string name, int? age = 10)
        {
            return Ok(new { message = $"Hello {name} age: {age}" });
        }
    }
}
