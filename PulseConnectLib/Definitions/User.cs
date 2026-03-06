using System;
using System.Collections.Generic;
using System.Text;

namespace PulseConnectLib.Definitions
{
    public class User
    {
        public string UserName { get; set;  }
        public string HashedPassword { get; set; }
        public ProviderClass providerClass { get; set; }
        public bool CanStartRapidResponse { get; set; }
        public bool ReceiveRapidResponseAlert { get; set; }
        public string GroupGUID { get; set; }
        public bool CanTransferShift { get; set; }
        public string CurrentSessionID { get; set;  }
        public DateTime SessionExpiry { get; set;  }
        public User(string username, string hashedpassword, ProviderClass providerDesignation, bool startRRT, bool receiveRRT, string groupID, bool transferShift, string currentSessionID, DateTime sessionExpiry)
        {
            UserName = username;
            HashedPassword = hashedpassword;
            providerClass = providerDesignation;
            CanStartRapidResponse = startRRT;
            ReceiveRapidResponseAlert = receiveRRT;
            GroupGUID = groupID;
            CanTransferShift = transferShift;
            CurrentSessionID = currentSessionID;
            SessionExpiry = sessionExpiry;
        }


    }

    public enum ProviderClass
    {
        Nurse = 0,
        MedDoc = 1,
        CardioDoc = 2,
        ICUdoc = 3,
        DedicatedRapidResponse = 4
    }

    public struct UserSessionDetails
    {
        public string UserName { get; set; }
        public string SessionId { get; set; }
        public DateTime SessionExpiry { get; set; }
    }
}
