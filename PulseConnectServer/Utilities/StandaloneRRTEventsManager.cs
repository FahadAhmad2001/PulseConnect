using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PulseConnectLib.Definitions.Entities;
using PulseConnectLib.Definitions.Events;
using PulseConnectServer.DTOs;
using PulseConnectServer.Utilities.DatabaseContexts;
using PulseConnectServer.Utilities.Notifiers;
using PulseConnectServer.Utilities.Services;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;


namespace PulseConnectServer.Utilities
{
    public class StandaloneRRTEventsManager : IStandAloneRRTEventsManager
    {
        
        private readonly StandAloneRRTEventsDBContext dbContext;
        private readonly StandaloneRapidEventsTrackingService eventTracker;
        private readonly IHubContext<StandaloneRRTEventsNotifier> notifierService;
        public StandaloneRRTEventsManager(StandAloneRRTEventsDBContext database, StandaloneRapidEventsTrackingService tracker, IHubContext<StandaloneRRTEventsNotifier> hubContext)
        {
            dbContext = database;
            eventTracker = tracker;
            notifierService = hubContext;
        }

        public async Task<string> CreateNewRapidResponseAsync(string callerID, string WardID, string BedID, RapidResponseEventType eventType, string eventDetails)
        {
            WardBed wardBed = new WardBed();
            wardBed.WardName = WardID;
            wardBed.BedNumber = BedID;
            RapidResponseEvent newEvent = new RapidResponseEvent();
            newEvent.CreateRapidResponseEvent("patient-W" + WardID + "-B" + BedID, eventType, DateTime.Now, eventDetails, "Ward " + WardID + " Bed " + BedID, callerID, wardBed);
            await dbContext.AddAsync(newEvent);
            await dbContext.SaveChangesAsync();
            SendNewStandAloneRapidEventDTO newEventDetails = new SendNewStandAloneRapidEventDTO();
            newEventDetails.EventDetails = eventDetails;
            newEventDetails.CallerName = callerID;
            newEventDetails.RapidResponseType = eventType;
            newEventDetails.WardId = WardID;
            newEventDetails.BedId = BedID;
            Log.AddLog("Sending alert for new rapid response on standalone server to all listening users", LogLevel.Info);
            await notifierService.Clients.All.SendAsync("NewRapidResponse", newEventDetails);
            await eventTracker.AddNewRapidResponseEvent(newEvent);
            return newEvent.EventGuid;
        }
        
        public async Task<IEnumerable<RapidResponseEvent>> GetEventsByCallerAsync(string callerId)
        {
            IEnumerable<RapidResponseEvent> listOfEvents;
            List<RapidResponseEvent> allEventsList = await dbContext.ListOfStandaloneEvents.ToListAsync();
            listOfEvents = allEventsList.Where(i => i.EventCallerGuid == callerId).OrderByDescending(d => d.EventTime);
            return listOfEvents;
        }

        public async Task<IEnumerable<RapidResponseEvent>> GetEventsByPatientAsync(string wardId, string bedId, DateTime startDate, DateTime endDate)
        {
            IEnumerable<RapidResponseEvent> listOfEvents;
            List<RapidResponseEvent> allEventsList = await dbContext.ListOfStandaloneEvents.ToListAsync();
            listOfEvents = allEventsList.Where(i => i.PatientLocation.WardName==wardId && i.PatientLocation.BedNumber==bedId && i.EventTime > startDate && i.EventTime < endDate).OrderByDescending(d => d.EventTime);
            return listOfEvents;
        }

        public async Task<IEnumerable<RapidResponseEvent>> GetEventsByWardAsync(string wardId)
        {
            IEnumerable<RapidResponseEvent> listOfEvents;
            List<RapidResponseEvent> allEventsList = await dbContext.ListOfStandaloneEvents.ToListAsync();
            listOfEvents = allEventsList.Where(i => i.PatientLocation.WardName==wardId).OrderByDescending(d => d.EventTime);
            return listOfEvents;
        }

        public async Task<RapidResponseEvent> GetEventByIDAsync(string id)
        {
            RapidResponseEvent correctEvent;
            List<RapidResponseEvent> allEventsList = await dbContext.ListOfStandaloneEvents.ToListAsync();
            correctEvent = allEventsList.First(i => i.EventGuid==id);
            return correctEvent;
        }

        public async Task<IEnumerable<RapidResponseEvent>> GetAllRapidResponsesAsync()
        {
            IEnumerable<RapidResponseEvent> listOfEvents;
            List<RapidResponseEvent> allEventsList = await dbContext.ListOfStandaloneEvents.ToListAsync();
            listOfEvents = allEventsList.OrderByDescending(d => d.EventTime);
            return listOfEvents;
        }

        public async Task<List<RapidResponseEvent>> GetCurrentlyActiveRapidEvents()
        {
            return await eventTracker.GetCurrentlyActiveEvents();
        }

    }
}
