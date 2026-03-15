namespace PulseConnectServer.DTOs
{
    public class SearchStandaloneRapidEventsByPatientDTO
    {
        public string WardId { get; set;  }
        public string BedId { get; set; }
        public DateTime StartSearchTime { get; set; }
        public DateTime EndSearchTime { get; set; }
    }
}
