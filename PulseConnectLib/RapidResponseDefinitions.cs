namespace PulseConnectLib
{
    internal class RapidResponseDefinitions
    {
    }
    public class RapidResponseEvent
    {

    }
    public class ResponseProvider
    {
        public string ProviderName;
        public ProviderClass providerClass;
        public bool CanSendAlerts;
        public bool CanReceiveAlerts;
        public bool CanTransferShift;
        public string GroupName;
        public string Username;
        public string HashedPasswd;
        public ResponseProvider(string name, ProviderClass personClass, bool canSendAlert, bool canReceiveAlert, bool canTransferShift, string teamName, string username, string hashedPass)
        {
            ProviderName = name;
            providerClass = personClass;
            CanSendAlerts = canSendAlert;
            CanReceiveAlerts = canReceiveAlert;
            CanTransferShift = canTransferShift;
            GroupName = teamName;
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
}
