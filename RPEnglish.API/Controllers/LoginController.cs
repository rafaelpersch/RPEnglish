using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RPEnglish.API.Tools;
using RPEnglish.API.DTO;
using RPEnglish.API.DatabaseContext;

namespace RPEnglish.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly MyDbContext myDbContext;
        public LoginController([FromServices] MyDbContext myDbContext)
        {
            this.myDbContext = myDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]LoginDTO credentials)
        {
            if (credentials == null)
            {
                return Unauthorized("Object credentials null!");
            }

            if (string.IsNullOrEmpty(credentials.Email) || string.IsNullOrEmpty(credentials.Password))
            {
                return Unauthorized("Invalid credentials!");
            }

            var userPassword = await myDbContext.UsersPassword.Include(x => x.User).FirstOrDefaultAsync(x=> x.User.Email == credentials.Email && x.Password == Cryptography.Encrypt(credentials.Email + credentials.Password));

            if (userPassword == null)
            {
                return Unauthorized("Invalid login!"); 
            }

            userPassword.Password = string.Empty;

            var token = TokenService.GenerateToken(userPassword.User);

            return Ok(new{ user = userPassword.User, token = token });
        }
    }
}