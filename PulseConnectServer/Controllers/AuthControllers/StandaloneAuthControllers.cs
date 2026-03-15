using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PulseConnectLib.Definitions;
using PulseConnectLib.Definitions.Entities;
using PulseConnectServer.DTOs;
using PulseConnectServer.Utilities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PulseConnectServer.Controllers.AuthControllers
{
    [Route("api/standalone/onboarding")]
    [ApiController]
    public class StandaloneCreateUserController : ControllerBase
    {
        private readonly IAuthenticationManager _authenticationManager;
        public StandaloneCreateUserController(IAuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager;
        }
        // GET: api/<StandaloneAuthController>
        
        // POST api/<StandaloneAuthController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateStandAloneUserDTO newUser)
        {
            if (newUser == null) return BadRequest("User is null");
            try
            {
                UserSessionDetails userdeets = await _authenticationManager.AddNewStandaloneUserAync(newUser.Username, newUser.Password, newUser.ProviderType, newUser.CanStartRRTs, newUser.ReceiveRRTs, newUser.OrgID, false);
                return Ok(userdeets);
            }
            catch(UserAlreadyExistsException ex)
            {
                return BadRequest("This username is already in use");
            }

        }

    }

    [Route("api/standalone/listusers")]
    [ApiController]
    public class StandaloneListUsersController : ControllerBase
    {
        private readonly IAuthenticationManager _authenticationManager;
        public StandaloneListUsersController(IAuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager;
        }
        // GET: api/<StandaloneAuthController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<User> usersList = await _authenticationManager.GetStandaloneUsersAsync();
            return Ok(usersList);
        }

        
    }

    [Route("api/standalone/loginuserpass")]
    [ApiController]
    public class StandaloneUserLoginWithPassword : ControllerBase
    {
        private readonly IAuthenticationManager _authenticationManager;
        public StandaloneUserLoginWithPassword(IAuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager;
        }
        // GET: api/standalone/loginuserpass
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginUserPassDTO userDetails)
        {
            if (userDetails == null) return BadRequest("Wrong API format");
            try
            {
                UserSessionDetails sessionDetails = await _authenticationManager.ValidateStandaloneUserPassAsync(userDetails.username, userDetails.password);
                CookieOptions loginCookie = new CookieOptions();
                loginCookie.Secure= false;
                loginCookie.HttpOnly = false;
                loginCookie.IsEssential=true;
                loginCookie.Expires = DateTimeOffset.UtcNow.AddMonths(2);
                loginCookie.SameSite = SameSiteMode.Lax;
                Response.Cookies.Append("clientSessionId", sessionDetails.CookieSessionID, loginCookie);
                Response.Cookies.Append("clientUsername", sessionDetails.UserName, loginCookie);
                return Ok(sessionDetails);
            }
            catch(UserDoesNotExistException ex)
            {
                return NotFound("The specified user does not exist");
            }
            catch(UserWrongPasswordException ex)
            {
                return Unauthorized("The password is incorrect");
            }
        }
    }

    [Route("api/standalone/logincookie")]
    [ApiController]
    public class StandaloneUserLoginWithCookie : ControllerBase
    {
        private readonly IAuthenticationManager _authenticationManager;
        public StandaloneUserLoginWithCookie(IAuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager;
        }
        // GET: api/standalone/loginuserpass`
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            string cookieId = Request.Cookies["clientSessionId"];
            string clientUsername = Request.Cookies["clientUsername"];
            if (string.IsNullOrEmpty(cookieId) || string.IsNullOrEmpty(clientUsername)) return BadRequest("Wrong API format. cookie id is " + cookieId);
            try
            {
                bool isValidLogin = await _authenticationManager.ValidateStandaloneUserCookieAsync(clientUsername, cookieId);
                if (isValidLogin)
                {
                    return Ok("Valid login details");
                }
                else
                {
                    return BadRequest("Wrong client session ID cookie :(");
                }
            }
            catch (UserExpiredCookieException ex)
            {
                return BadRequest("The cookie is expired");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
