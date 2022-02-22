using ASPNETLIVE.Areas.Identity.Data;
using ASPNETLIVE.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASPNETLIVE.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager; // user
        private readonly SignInManager<User> _signInManager; // login
        private readonly IWebHostEnvironment _hosting; // path
        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment hosting)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hosting = hosting;
        }

        // POST: api/user/register
        [HttpPost]
        [RequestSizeLimit(10*1024*1024)] // 10MB
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            var user = new User
            {
                FullName = registerViewModel.FullName,
                Email = registerViewModel.Email,
                UserName = registerViewModel.Email
            };

            //upload file base64
            if (registerViewModel.Photo != "nopic.png")
            {
                var base64array = Convert.FromBase64String(registerViewModel.Photo!);
                var newFileName = Guid.NewGuid() + ".png";

                var ImagePath = Path.Combine($"{_hosting.WebRootPath}/upload/{newFileName}");
                await System.IO.File.WriteAllBytesAsync(ImagePath, base64array);

                user.Photo = newFileName;
            }

            var result = await _userManager.CreateAsync(user, registerViewModel.Password); // สร้าง user
            if (result.Succeeded)
            {
                return Created("", new { message = "ลงทะเบียนสำเร็จ" });
            }
            return BadRequest(new { message = "เกิดข้อผิดพลาด มีผู้ใช้งานในระบบแล้ว" });
        }

        // Log in
        // POST: api/user/login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            if (user == null) return NotFound(new { message = "ไม่พบผู้ใช้นี้ในระบบ" });

            var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, false, false);
            if (result.Succeeded)
            {
                // สร้าง token
                return await CreateToken(loginViewModel.Email);
            }
            return Unauthorized(new { message = "อีเมลหรือรหัสผ่านไม่ถูกต้อง" });
        }

        // Create token
        private async Task<IActionResult> CreateToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            // 1. ค้นหาว่า user คนนี้ role อะไร ถ้าต้องการใช้ role-base authentication (สิทธิ)
            var roles = await _userManager.GetRolesAsync(user);

            if (user != null)
            {
                //  generate token that is valid for 7 days
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretKey = Encoding.UTF8.GetBytes("odn051PvFMtRTBZsqmWkGJl8CHbKceQz"); //jwt secret key

                var subject = new List<Claim>
                 {
                    new Claim("user_id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                 };

                // 2.เพิ่ม role เข้าไปใน payload ถ้าต้องการใช้ role-base authentication
                subject.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(subject),
                    Expires = DateTime.UtcNow.AddDays(7), //วันหมดอายุของ token
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var resultToken = new
                {
                    access_token = tokenHandler.WriteToken(token),
                    expiration = token.ValidTo
                };

                return Ok(resultToken);
            }
            return Unauthorized();
        }

        // GET: api/user/profile
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            //get token from header
            var token = await HttpContext.GetTokenAsync("access_token");

            //decode token and get user_id
            var payload = new JwtSecurityToken(token);
            var userId = payload.Claims.First(p => p.Type == "user_id").Value; // user's id

            //เอา userId ไปค้นหาในตาราง แล้ว return ข้อมูลออกไป
            var userProfile = await _userManager.FindByIdAsync(userId);

            return Ok(new
            {
                Id = userProfile.Id,
                Fullname = userProfile.FullName,
                Email = userProfile.Email,
                PhotoUrl = $"{Request.Scheme}://{Request.Host}/upload/{userProfile.Photo}"
            });

        }

    }
}
