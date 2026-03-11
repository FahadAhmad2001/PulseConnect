using System;
using System.Collections.Generic;
using System.Text;

namespace PulseConnectLib.Definitions.Entities
{
    public class DoctorUser : User
    {
        public bool CanStartRapidResponse { get; set; }
        public bool ReceiveRapidResponseAlert { get; set; }
        public List<string> GroupGUIDs { get; set; }
        public bool CanTransferShift { get; set; }
        public bool IsCurrentlyOnShift { get; set; }
        public void CreateNewDoctorUser(string username, string hashedpassword, ProviderClass userType, string currentSessionID, DateTime sessionExpiry, DateTime creatingDate, bool canStartRapidResponses, bool ReceiveRRTAlerts, List<string> ListOfGroups, bool CanTransfer)
        {
            base.CreateNewUser(username, hashedpassword, userType, currentSessionID, sessionExpiry, creatingDate);
            CanStartRapidResponse = canStartRapidResponses;
            ReceiveRapidResponseAlert = ReceiveRRTAlerts;
            GroupGUIDs = ListOfGroups;
            CanTransferShift = CanTransfer;
            IsCurrentlyOnShift = false;
        }
        public void StartShift()
        {
            IsCurrentlyOnShift = true;
        }
        public void StopShift()
        {
            IsCurrentlyOnShift = false;
        }
    }
}
