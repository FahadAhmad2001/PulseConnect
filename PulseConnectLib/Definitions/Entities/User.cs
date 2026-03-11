using System;
using System.Collections.Generic;
using System.Text;

namespace PulseConnectLib.Definitions.Entities
{
    
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set;  }
        public string HashedPassword { get; set; }
        public ProviderClass UserType { get; set; }
        public DateTime SessionExpiry { get; set;  }
        public string CurrentSessionID { get; set; }
        public DateTime TimeUserCreated { get; set; }
        public virtual void CreateNewUser(string username, string hashedpassword, ProviderClass userType, string currentSessionID, DateTime sessionExpiry, DateTime creationDate)
        {
            UserName = username;
            HashedPassword = hashedpassword;
            UserType = userType;
            CurrentSessionID = currentSessionID;
            SessionExpiry = sessionExpiry;
            TimeUserCreated = creationDate;
        }


    }

    public enum ProviderClass
    {
        Doctor,Nursing,Physiotherapist,Dietician,LabTechnician,RadioTechnician,Patient
    }

    public struct UserSessionDetails
    {
        public string UserName { get; set; }
        public string SessionId { get; set; }
        public DateTime SessionExpiry { get; set; }
        public string CookieSessionID { get; set; }
    }
}
