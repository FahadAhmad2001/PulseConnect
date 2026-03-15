using PulseConnectLib.Definitions;

namespace PulseConnectServer.Utilities.Services
{
    public static class RequestAuthVerificationService
    {
        public static async Task<bool> VerifyStandaloneUserAsync(string  username, string cookieId, IAuthenticationManager authManager)
        {
            if (string.IsNullOrEmpty(cookieId) || string.IsNullOrEmpty(username)) return false;
            try
            {
                bool isValidLogin = await authManager.ValidateStandaloneUserCookieAsync(username, cookieId);
                return isValidLogin;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<bool> VerifyIntegratedUserAsync(string username, string cookieId, IAuthenticationManager authManager)
        {
            if (string.IsNullOrEmpty(cookieId) || string.IsNullOrEmpty(username)) return false;
            try
            {
                bool isValidLogin = await authManager.ValidateUserCookieAsync(username, cookieId);
                return isValidLogin;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
