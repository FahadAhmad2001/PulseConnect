using PulseConnectLib.Definitions.Events;
using System.Runtime.CompilerServices;

namespace PulseConnectServer.Utilities
{
    public interface IStandAloneRRTEventsManager
    {
        Task<IEnumerable<RapidResponseEvent>> GetAllRapidResponsesAsync();
        Task<RapidResponseEvent> GetEventByIDAsync(string id);
        Task<IEnumerable<RapidResponseEvent>> GetEventsByWardAsync(string wardId);
        Task<IEnumerable<RapidResponseEvent>> GetEventsByPatientAsync(string wardId, string bedId, DateTime startDate, DateTime endDate);
        Task<string> CreateNewRapidResponseAsync(string callerID, string WardID, string BedID, RapidResponseEventType eventType, string eventDetails);
        Task<IEnumerable<RapidResponseEvent>> GetEventsByCallerAsync(string callerId);
    }
}
