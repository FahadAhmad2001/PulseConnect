using PulseConnectLib.Definitions.Events;
using System.Collections.Concurrent;

namespace PulseConnectServer.Utilities.Services
{
    public class StandaloneRapidEventsTrackingService : BackgroundService
    {
        private readonly List<RapidResponseEvent> AllCurrentEvents;
        private readonly ReaderWriterLockSlim _lock;
        private readonly IServiceScopeFactory _scopeFactory;

        public StandaloneRapidEventsTrackingService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            AllCurrentEvents = new List<RapidResponseEvent>();
            _lock = new ReaderWriterLockSlim();
        }

        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ReviewRapidResponses();
                Log.AddLog("Reviewing currently active standalone responses...", LogLevel.Debug);
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); // runs every 30 seconds
            }
        }

        public async Task AddNewRapidResponseEvent(RapidResponseEvent rapidEvent)
        {
            _lock.EnterWriteLock();
            try
            {
                AllCurrentEvents.Add(rapidEvent);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public async Task ReviewRapidResponses()
        {
            _lock.EnterWriteLock();
            try
            {
                foreach(RapidResponseEvent eachEvent in AllCurrentEvents)
                {
                    TimeSpan span = DateTime.Now - eachEvent.EventTime;
                    if(span.TotalMinutes > 20) //set timer here for when rapid responses are deemed "inactive"
                    {
                        Log.AddLog("Removing standalone rapid response GUID " + eachEvent.EventGuid + " and location " + eachEvent.PatientGuid + " and rapid response type " 
                            + eachEvent.RRTEventType.ToString() + " from active list as 20 mins have passed since initiation", LogLevel.Debug);
                        AllCurrentEvents.Remove(eachEvent);
                    }
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public async Task<List<RapidResponseEvent>> GetCurrentlyActiveEvents()
        {
            _lock.EnterReadLock();
            try
            {
                return AllCurrentEvents.ToList();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }


    }
}
