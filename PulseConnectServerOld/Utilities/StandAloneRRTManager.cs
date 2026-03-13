using System;
using System.Collections.Generic;
using System.Text;
using PulseConnectLib.Definitions.Events;
using Microsoft.EntityFrameworkCore;
using PulseConnectServerOld.Utilities.DatabaseContexts;

namespace PulseConnectServerOld.Utilities
{
    public static class StandAloneRRTManager
    {
        public static List<RapidResponseEvent> AllRRTEvents { get; set;  } = new List<RapidResponseEvent>();
        public static int CurrentNewEventId { get; set; } = 0;

        public static async Task CreateDB()
        {
            using (StandAloneRRTEventsDBContext dbContext = new StandAloneRRTEventsDBContext())
            {
                await dbContext.Database.EnsureCreatedAsync();
            }
            //CurrentNewEventId = GetAllEventsList().Result.Count;
        }



        public static async Task<List<RapidResponseEvent>> GetAllEventsList()
        {
            List<RapidResponseEvent> newList = new List<RapidResponseEvent>();
            using (StandAloneRRTEventsDBContext dbContext = new StandAloneRRTEventsDBContext())
            {
                newList = await dbContext.ListOfStandaloneEvents.OrderByDescending(x => x.EventTime).ToListAsync();
            }
            AllRRTEvents = newList;
            Log.AddLog("===========LISTING ALL STANDALONE RAPID RESPONSE EVENTS===========", LogLevel.Info);
            foreach(RapidResponseEvent eachRRT in AllRRTEvents)
            {
                Log.AddLog("Rapid response alert WAS received on standalone RRT for patient GUID " + eachRRT.PatientGuid
                + " and patient location " + eachRRT.PatientCurrentLocationGuid + " and started on" + eachRRT.EventTime
                + "and called by user ID " + eachRRT.EventCallerGuid + " and event ID " + eachRRT.EventGuid + " and event severity is " + eachRRT.EventSeverity.ToString() +
                " and rapid response type is " + eachRRT.RRTEventType.ToString(), LogLevel.Info);
            }
            return newList;
        }

        public static async Task AddNewStandAloneRRTEvent(RapidResponseEvent newEvent)
        {
            Log.AddLog("Rapid response alert received on standalone RRT for patient GUID " + newEvent.PatientGuid
                + " and patient location " + newEvent.PatientCurrentLocationGuid + " and started on" + newEvent.EventTime
                + "and called by user ID " + newEvent.EventCallerGuid + " and event ID " + newEvent.EventGuid + " and event severity is " + newEvent.EventSeverity.ToString() +
                " and rapid response type is " + newEvent.RRTEventType.ToString(),LogLevel.Info);


            List<RapidResponseEvent> newList = new List<RapidResponseEvent>();
            using (StandAloneRRTEventsDBContext dbContext = new StandAloneRRTEventsDBContext())
            {
                await dbContext.ListOfStandaloneEvents.AddAsync(newEvent);
                await dbContext.SaveChangesAsync();
                newList = await dbContext.ListOfStandaloneEvents.OrderByDescending(x => x.EventTime).ToListAsync();
            }
            AllRRTEvents= newList;
        }
    }
}
