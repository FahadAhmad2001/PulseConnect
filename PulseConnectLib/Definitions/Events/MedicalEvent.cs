using System;
using System.Collections.Generic;
using System.Text;

namespace PulseConnectLib.Definitions.Events
{
    [Serializable]
    public class MedicalEvent
    {
        public int Id { get; set; }
        public DateTime EventTime { get; set; }
        public string EventData { get; set; }
        public string EventGuid { get; set; }
        public string PatientGuid { get; set; }
        public string PatientCurrentLocationGuid { get; set; }
        public MedEventSeverity EventSeverity { get; set; }
        public MedEventType EventClass { get; set; }
        public string EventCallerGuid { get; set; }
    }

    public enum MedEventSeverity
    {
        RegularFollow,Urgent,Critical,RapidResponseOther,Arrest
    }
    public enum MedEventType
    {
        Admission,Discharge,Transfer,LabReport,UrgentLabReport,ConsultationRequest,UrgentConsultationRequest,ConsultationResponse,ImagingRequest,UrgentImagingRequest,ImagingResponse,DeviceAlert,RapidResponseOther,Arrest
    }
}
