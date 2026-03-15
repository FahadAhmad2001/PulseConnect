using System.Runtime.CompilerServices;
using PulseConnectLib.Definitions.Events;

namespace PulseConnectServer.DTOs
{
    public class SendNewStandAloneRapidEventDTO
    {
        public string WardId { get; set; }
        public string BedId { get; set; }
        public RapidResponseEventType  RapidResponseType { get; set; }
        public string CallerName { get; set; }
        public string EventDetails { get; set; }
    }
}
