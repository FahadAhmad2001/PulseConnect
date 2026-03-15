using System;
using System.Collections.Generic;
using System.Text;

namespace PulseConnectLib.Definitions.Events
{
    [Serializable]
    public class RapidResponseEvent : MedicalEvent
    {
        public bool RoscAchieved { get; set; }
        public RapidResponseEventType RRTEventType { get; set;  }
        public int TimeToRosc { get; set;  }
        public WardBed PatientLocation { get; set; }
        public string CreateRapidResponseEvent(string Patientid, RapidResponseEventType RapidType, DateTime RRTTime, string RRTDetails, string PatLoc, string CallerId, WardBed officialPatLoc, bool isRoscAchieved = false, int timeToRosc = 0) 
        {
            EventTime = RRTTime;
            PatientGuid = Patientid;
            RRTEventType = RapidType;
            EventData = RRTDetails;
            PatientCurrentLocationGuid = PatLoc;
            RoscAchieved = isRoscAchieved;
            TimeToRosc = timeToRosc;
            EventCallerGuid = CallerId;
            PatientLocation = officialPatLoc;
            if (RapidType == RapidResponseEventType.CardiacArrest)
            {
                EventSeverity = MedEventSeverity.Arrest;
                EventClass = MedEventType.Arrest;
            }
            else
            {
                EventSeverity = MedEventSeverity.Critical;
                EventClass = MedEventType.RapidResponseOther;
            }
            EventGuid=Guid.NewGuid().ToString();
            return EventGuid;
        }
        public void UpdateRoscStatus(bool roscAchieved, int timeToRosc)
        {
            TimeToRosc = timeToRosc;
            RoscAchieved= roscAchieved;
        } 
    }
    public enum RapidResponseEventType
    {
        Tachycardia,Bradycardia,CardiacArrest
    }
    public struct WardBed
    {
        public string WardName {  get; set; }
        public string BedNumber { get; set; }
    }
}
