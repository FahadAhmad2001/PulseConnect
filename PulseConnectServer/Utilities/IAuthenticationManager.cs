using PulseConnectLib.Definitions.Entities;

namespace PulseConnectServer.Utilities
{
    public interface IAuthenticationManager
    {
        Task<IEnumerable<User>> GetIntegratedUsersAsync();
        Task<IEnumerable<User>> GetStandaloneUsersAsync();
        Task<IEnumerable<User>> GetUsersFromMemory();
        Task<IEnumerable<User>> GetStandAloneUsersFromMemory();
        Task<UserSessionDetails> AddNewUserAync(string usrname, string passwd, ProviderClass usrType, bool StartRRT, bool ReceiveRRT, string grpID, bool CanTransShift);
        Task<UserSessionDetails> AddNewStandaloneUserAync(string usrname, string passwd, ProviderClass usrType, bool StartRRT, bool ReceiveRRT, string grpID, bool CanTransShift);
        Task<UserSessionDetails> ValidateUserPassAsync(string username, string password);
        Task<UserSessionDetails> ValidateStandaloneUserPassAsync(string username, string password);
        Task<bool> ValidateUserCookieAsync(string username, string cookieID);
        Task<bool> ValidateStandaloneUserCookieAsync(string username, string cookieID);
        Task<bool> UpdateStandaloneUserSessionDetailsAsync(string username, string sessionID, DateTime newExpiry);
        Task<bool> UpdateUserSessionDetailsAsync(string username, string sessionID, DateTime newExpiry);
    }
}
