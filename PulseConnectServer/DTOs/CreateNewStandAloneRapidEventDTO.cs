using PulseConnectLib.Definitions.Events;

namespace PulseConnectServer.DTOs
{
    public class CreateNewStandAloneRapidEventDTO
    {
        public string WardId { get; set; }
        public string BedId { get; set; }
        public RapidResponseEventType RapidResponseType { get; set; }
        public string EventDetails { get; set; }
    }
}
