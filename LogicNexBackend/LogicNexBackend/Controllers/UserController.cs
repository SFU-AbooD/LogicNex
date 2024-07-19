using LogicNexBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using LogicNexBackend.utils;
using LogicNexBackend.Repositories;
using Microsoft.AspNetCore.Identity.UI.Services;
namespace LogicNexBackend.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[Controller]")]
    public class UserController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly UserRepository _Usermanager;
        private readonly IEmailSender _EmailSender;
        public UserController(IConfiguration configuration, UserRepository Usermanager,
            IEmailSender EmailSender) {
            _configuration = configuration;
            _Usermanager = Usermanager;
            _EmailSender = EmailSender;
        }
        [Route("confirm")]
        [HttpGet]
        public async Task<IActionResult> ConfirmAccount([FromQuery] string link) {
            bool result = await _Usermanager.confirmUserEmail(link);
            if (result == true)
            {
                return Ok("user has been activated");
            }
            else {
                return BadRequest("Link Expired");
            }

        }
        [Route("getUser")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> getUser()
        {
            
            Claim? email_claim = User.Claims.FirstOrDefault(x=>x.Type == ClaimTypes.Email);
            if (email_claim == null)
                return NotFound();
            else { 
                string email = email_claim.Value;
                MongoDbuser? usr = await _Usermanager.Get_User(email);
                if (usr == null)
                {
                    return NotFound();
                }
                else {
                    return Json(new { 
                        username = usr.UserName,
                        email = email_claim.Value,

                    });
                }
            }
        }
        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> logout() {
            Console.WriteLine("is it clear");
            HttpContext.Response.DeleteAuthCookie();
            return Ok("Deleted");
        }
        [HttpPost]
        [Route("refreshToken")]
        public async Task<IActionResult> Refresh_token() {
            string? refresh_token = Request.Cookies["__Host-Forbidden-refreshToken"];
            if (refresh_token == null)
                return Redirect("/login");
            ClaimsPrincipal? claimsPrincipal = Utils.Validate_token(refresh_token, _configuration);
            if (claimsPrincipal != null)
            {
                string? token_key_extracted = claimsPrincipal.Claims
                    .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.UniqueName)!.Value;
                string? Email = claimsPrincipal.Claims
                 .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)!.Value;
                refreshTokenModel? model = await _Usermanager.getRefreshToken(Email, token_key_extracted);
                if (model != null)
                {
                    string refresh_token_generate = Utils.GenerateRefresh_token(_configuration, Email, token_key_extracted);
                    string token_key = Guid.NewGuid().ToString();
                    string login_token = Utils.Write_token(_configuration, Email);
                    bool store_token = await _Usermanager.addRefreshToken(Email, refresh_token, token_key);
                    if (store_token == true)
                    {
                        Response.RegisterAuthCookie(login_token, refresh_token);
                        return Ok("Ok");
                    }
                    else
                    {
                        return BadRequest("Error");
                    }
                }
                else {
                    return Redirect("/login");
                }
            }
            else {
                return Redirect("/login");
            }
        }
        [Route("SignUp")]
        [HttpPost]
        public async Task<IActionResult> signUp(UserRegister New_User)
        {
            if (New_User.Username.Length <= 1)
            {
                return BadRequest();
            }
            string password_hash = Utils.md5_hash(New_User.Password);
            MongoDbuser user = new MongoDbuser()
            {
                Email = New_User.Email,
                UserName = New_User.Username,
                Password_hash = password_hash,
                role = "User",
                is_active = true,
                Email_Confimerd = false,
                title = "newbie",
                Refresh_tokens = []
            };
            Status result = await _Usermanager.RegisterUser(user, _EmailSender);
            switch (result)
            {
                case Status.OK:
                    return Ok(new CreateUserResponse { status_code = 200, message = "Inserted" });
                case Status.NOT_UNIQUE:
                    return BadRequest(new CreateUserResponse { status_code = 401, message = "User Already Registerd" });
                case Status.INSERT_ISSUE:
                    return BadRequest(new CreateUserResponse { status_code = 402, message = "Please Try again" });
                default:
                    return BadRequest(new CreateUserResponse { status_code = 403, message = "Server Does not know the issue" });
            }
        }
        [HttpGet]
        [Route("username")]
        public async Task<IActionResult> getUsername([FromQuery]string email)
        {
            MongoDbuser? user = await _Usermanager.getUsername(email);
            if (user == null)
                return NotFound();
            else return Json(new { username = user.UserName});
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLogin? User_formation)
        {
            if (User_formation == null)
            {
                return BadRequest("Empty User information");
            }
            User_formation.password = Utils.md5_hash(User_formation.password);
            MongoDbuser? result = await _Usermanager.User_Login(User_formation);
            if (result != null)
            {
                string token_key = Guid.NewGuid().ToString();
                string refresh_token = Utils.GenerateRefresh_token(_configuration,User_formation.email, token_key);
                string login_token = Utils.Write_token(_configuration, User_formation.email);
                bool store_token = await _Usermanager.addRefreshToken(User_formation.email, refresh_token, token_key);
                if (store_token == true)
                {
                    Response.RegisterAuthCookie(login_token, refresh_token);
                    return Ok("Ok");
                }
                else {
                    return BadRequest("Error");
                }
            }
            else{
                return BadRequest("Wrong Credentials");
            }
        }
    }
}
