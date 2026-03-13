using PulseConnectLib.Definitions.Entities;

namespace PulseConnectServer.DTOs
{
    public class CreateStandAloneUserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string OrgID { get; set; }
        public string GrpId { get; set; }
        public bool CanStartRRTs { get; set; }
        public bool ReceiveRRTs { get; set; }
        public bool TransferShift { get; set; }
        public ProviderClass ProviderType { get; set; }
    }
}
