using Microsoft.AspNetCore.Mvc;
using PulseConnectLib.Definitions.Events;
using PulseConnectServer.DTOs;
using PulseConnectServer.Utilities;
using PulseConnectServer.Utilities.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PulseConnectServer.Controllers.StandaloneRRTControllers
{
    [Route("api/standalone/newevent")]
    [ApiController]
    public class CreateStandaloneRRTEvent : ControllerBase
    {

        private readonly IAuthenticationManager _authenticationManager;
        private readonly IStandAloneRRTEventsManager _eventsManager;
        public CreateStandaloneRRTEvent(IAuthenticationManager authenticationManager, IStandAloneRRTEventsManager standAloneRRTEventsManager)
        {
            _authenticationManager = authenticationManager;
            _eventsManager = standAloneRRTEventsManager;
        }
        // POST api/standalone/newevent
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateNewStandAloneRapidEventDTO newEventDetails)
        {
            string cookieId = Request.Cookies["clientSessionId"];
            string clientUsername = Request.Cookies["clientUsername"];
            if (!await RequestAuthVerificationService.VerifyStandaloneUserAsync(clientUsername, cookieId, _authenticationManager)) return BadRequest("Invalid request - the user and cookie session is not registered on the server");
            try
            {
                string newEventGuid = await _eventsManager.CreateNewRapidResponseAsync(clientUsername, newEventDetails.WardId, newEventDetails.BedId, newEventDetails.RapidResponseType, newEventDetails.EventDetails);
                return Ok(newEventGuid);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }

    [Route("api/standalone/listevents")]
    [ApiController]
    public class GetStandaloneRRTEvents : ControllerBase
    {

        private readonly IAuthenticationManager _authenticationManager;
        private readonly IStandAloneRRTEventsManager _eventsManager;
        public GetStandaloneRRTEvents(IAuthenticationManager authenticationManager, IStandAloneRRTEventsManager standAloneRRTEventsManager)
        {
            _authenticationManager = authenticationManager;
            _eventsManager = standAloneRRTEventsManager;
        }
        // GET api/standalone/listevents
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string cookieId = Request.Cookies["clientSessionId"];
            string clientUsername = Request.Cookies["clientUsername"];
            if (!await RequestAuthVerificationService.VerifyStandaloneUserAsync(clientUsername, cookieId, _authenticationManager)) return BadRequest("Invalid request - the user and cookie session is not registered on the server");
            try
            {
                IEnumerable<RapidResponseEvent> allEvents = await _eventsManager.GetAllRapidResponsesAsync();
                return Ok(allEvents);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }

    [Route("api/standalone/geteventbycaller")]
    [ApiController]
    public class GetStandaloneEventsByCaller : ControllerBase
    {

        private readonly IAuthenticationManager _authenticationManager;
        private readonly IStandAloneRRTEventsManager _eventsManager;
        public GetStandaloneEventsByCaller(IAuthenticationManager authenticationManager, IStandAloneRRTEventsManager standAloneRRTEventsManager)
        {
            _authenticationManager = authenticationManager;
            _eventsManager = standAloneRRTEventsManager;
        }
        // GET api/standalone/geteventbycaller/user1
        [HttpGet("{caller}")]
        public async Task<IActionResult> Get(string caller)
        {
            string cookieId = Request.Cookies["clientSessionId"];
            string clientUsername = Request.Cookies["clientUsername"];
            if (!await RequestAuthVerificationService.VerifyStandaloneUserAsync(clientUsername, cookieId, _authenticationManager)) return BadRequest("Invalid request - the user and cookie session is not registered on the server");
            try
            {
                IEnumerable<RapidResponseEvent> allEvents = await _eventsManager.GetEventsByCallerAsync(caller);
                return Ok(allEvents);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }

    [Route("api/standalone/geteventbyward")]
    [ApiController]
    public class GetStandaloneEventsByWard : ControllerBase
    {

        private readonly IAuthenticationManager _authenticationManager;
        private readonly IStandAloneRRTEventsManager _eventsManager;
        public GetStandaloneEventsByWard(IAuthenticationManager authenticationManager, IStandAloneRRTEventsManager standAloneRRTEventsManager)
        {
            _authenticationManager = authenticationManager;
            _eventsManager = standAloneRRTEventsManager;
        }
        // GET api/standalone/geteventbycaller/14
        [HttpGet("{ward}")]
        public async Task<IActionResult> Get(string ward)
        {
            string cookieId = Request.Cookies["clientSessionId"];
            string clientUsername = Request.Cookies["clientUsername"];
            if (!await RequestAuthVerificationService.VerifyStandaloneUserAsync(clientUsername, cookieId, _authenticationManager)) return BadRequest("Invalid request - the user and cookie session is not registered on the server");
            try
            {
                IEnumerable<RapidResponseEvent> allEvents = await _eventsManager.GetEventsByWardAsync(ward);
                return Ok(allEvents);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }

    [Route("api/standalone/geteventbypatient")]
    [ApiController]
    public class GetStandaloneEventsByPatient : ControllerBase
    {

        private readonly IAuthenticationManager _authenticationManager;
        private readonly IStandAloneRRTEventsManager _eventsManager;
        public GetStandaloneEventsByPatient(IAuthenticationManager authenticationManager, IStandAloneRRTEventsManager standAloneRRTEventsManager)
        {
            _authenticationManager = authenticationManager;
            _eventsManager = standAloneRRTEventsManager;
        }
        // POST api/standalone/geteventbypatient
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SearchStandaloneRapidEventsByPatientDTO searchDetails)
        {
            string cookieId = Request.Cookies["clientSessionId"];
            string clientUsername = Request.Cookies["clientUsername"];
            if (!await RequestAuthVerificationService.VerifyStandaloneUserAsync(clientUsername, cookieId, _authenticationManager)) return BadRequest("Invalid request - the user and cookie session is not registered on the server");
            try
            {
                IEnumerable<RapidResponseEvent> allEvents = await _eventsManager.GetEventsByPatientAsync(searchDetails.WardId, searchDetails.BedId, searchDetails.StartSearchTime, searchDetails.EndSearchTime);
                return Ok(allEvents);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }

    [Route("api/standalone/geteventbyguid")]
    [ApiController]
    public class GetStandaloneEventById : ControllerBase
    {

        private readonly IAuthenticationManager _authenticationManager;
        private readonly IStandAloneRRTEventsManager _eventsManager;
        public GetStandaloneEventById(IAuthenticationManager authenticationManager, IStandAloneRRTEventsManager standAloneRRTEventsManager)
        {
            _authenticationManager = authenticationManager;
            _eventsManager = standAloneRRTEventsManager;
        }
        // GET api/standalone/geteventbyguid/aaaa-bbbb-cccc-dddd
        [HttpGet("{guid}")]
        public async Task<IActionResult> Get(string guid)
        {
            string cookieId = Request.Cookies["clientSessionId"];
            string clientUsername = Request.Cookies["clientUsername"];
            if (!await RequestAuthVerificationService.VerifyStandaloneUserAsync(clientUsername, cookieId, _authenticationManager)) return BadRequest("Invalid request - the user and cookie session is not registered on the server");
            try
            {
                RapidResponseEvent correctEvent = await _eventsManager.GetEventByIDAsync(guid);
                return Ok(correctEvent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
