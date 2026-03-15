using Microsoft.AspNetCore.SignalR;
using PulseConnectLib.Definitions.Events;
using PulseConnectServer.DTOs;

namespace PulseConnectServer.Utilities.Notifiers
{
    public class StandaloneRRTEventsNotifier : Hub
    {
        public async Task SendNewEventAlert(WardBed patientLocation, string eventDetails, RapidResponseEventType eventType, string callerName)
        {
            
        }
    }
}
