using Konscious.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using PulseConnectLib.Definitions;
using PulseConnectLib.Definitions.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Data.SqlTypes;
using PulseConnectServerOld.Utilities.DatabaseContexts;


namespace PulseConnectServerOld.Utilities
{
    public static class AuthenticationManager
    {
        public static bool IsModifyingUsersList = false;
        public static bool IsModifyingStandaloneusersList = false;
        private const int SaltSize = 16; // 128 bits
        private const int HashSize = 32; // 256 bits
        private const int DegreeOfParallelism = 8; // Number of threads to use
        private const int Iterations = 4; // Number of iterations
        private const int MemorySize = 65536; // 1 GB
        public static List<User> AllCurrentUsersList { get; set; }
        public static List<User> AllCurrentStandaloneUsersList { get; set; }
        private static Dictionary<string,string> UserSessions { get; set; }
        private static Dictionary <string,string> StandaloneUserSessions { get; set; }
        public static async Task LoadUserDetails()
        {
            using (StandAloneRRTMembersDBContext dbContext = new StandAloneRRTMembersDBContext())
            {
                await dbContext.Database.EnsureCreatedAsync();
                AllCurrentStandaloneUsersList =  await dbContext.ListOfStandaloneUsers.ToListAsync();
            }
            using (IntegratedServerUsersDBContext dbContext = new IntegratedServerUsersDBContext())
            {
                await dbContext.Database.EnsureCreatedAsync();
                AllCurrentUsersList = await dbContext.AllServerUsers.ToListAsync();
            }
            Log.AddLog("==========LISTING INTEGRATED USERS==========",LogLevel.Debug);
            foreach(User eachuser in AllCurrentUsersList)
            {
                //for production dont write passwords to log
                Log.AddLog("Adding existing user from file with username: (" + eachuser.UserName + 
                    ") hashed password: (" + eachuser.HashedPassword + ") designation: (" + eachuser.UserType.ToString() +  
                    " and creation date as " + eachuser.TimeUserCreated.ToString() + 
                    " and current session ID " + eachuser.CurrentSessionID + " and current session expiry " + 
                    eachuser.SessionExpiry, LogLevel.Info);
            }
            Log.AddLog("==========LISTING STANDALONE RRT USERS==========", LogLevel.Debug);
            foreach (User eachuser in AllCurrentStandaloneUsersList)
            {
                //for production dont write passwords to log
                Log.AddLog("Adding existing standalone user from file with username: (" + eachuser.UserName + 
                    ") hashed password: (" + eachuser.HashedPassword + ") designation: (" + eachuser.UserType.ToString() + 
                    " and creation date as " + eachuser.TimeUserCreated.ToString() + " and current session ID " + 
                    eachuser.CurrentSessionID + " and current session expiry " + eachuser.SessionExpiry, LogLevel.Info);
            }
            UserSessions = new Dictionary<string, string>();
            StandaloneUserSessions = new Dictionary<string, string>();
        }

        public static async Task<List<User>> GetAllUsersListAsync()
        {
            List<User> CompleteUsersList = new List<User>();
            using (IntegratedServerUsersDBContext dbContext = new IntegratedServerUsersDBContext())
            {
                CompleteUsersList = await dbContext.AllServerUsers.ToListAsync();
            }
            AllCurrentUsersList = CompleteUsersList;
            return CompleteUsersList;
        }

        public static async Task<List<User>> GetAllStandaloneUsersListAsync()
        {
            List<User> CompleteUsersList = new List<User>();
            using(StandAloneRRTMembersDBContext dbContext = new StandAloneRRTMembersDBContext())
            {
                CompleteUsersList = await dbContext.ListOfStandaloneUsers.ToListAsync();
            }
            AllCurrentStandaloneUsersList= CompleteUsersList;
            return CompleteUsersList;
        }

        public static async Task<UserSessionDetails> AddNewUserAsync(string usrname, string passwd, ProviderClass usrType, bool StartRRT, bool ReceiveRRT, string grpID, bool CanTransShift)
        {
        StartAddUser:
            if (IsModifyingUsersList)
            {
                await Task.Delay(100);
                goto StartAddUser;
            }
            IsModifyingUsersList = true;
            await GetAllUsersListAsync();
            foreach (User usr in AllCurrentUsersList)
            {
                if (usrname == usr.UserName)
                {
                    IsModifyingUsersList = false;
                    throw new UserAlreadyExistsException(usrname);
                }
            }
            if (usrType == ProviderClass.Doctor)
            {
                DoctorUser newUser = new DoctorUser();
                Log.AddLog("Adding new user with username: (" + usrname + ") password: (" + passwd + 
                    ") designation: (" + usrType.ToString() + ") Start rapid responses set to " + StartRRT.ToString() + 
                    " and receive RR alerts set to " + ReceiveRRT.ToString() + " and group ID: (" + grpID.ToString() + 
                    ") and ability to transfer shift as " + CanTransShift.ToString(), LogLevel.Info);
                //for production releases dont write passwords to log
                string HashedPass = HashPassword(passwd);
                Log.AddLog("Hashed password is " + HashedPass, LogLevel.Debug);
                DateTime NewSessionExp = DateTime.Now.AddMonths(2);
                Guid newSessionId = Guid.NewGuid();
                Guid newCookieId = Guid.NewGuid();
                Log.AddLog("New session ID is " + newSessionId.ToString() + " and will expire on " + NewSessionExp.ToString(), LogLevel.Debug);
                List<string> guidLists = new List<string>();
                guidLists.Add(grpID);
                newUser.CreateNewDoctorUser(usrname, HashedPass, usrType, newSessionId.ToString(), NewSessionExp, DateTime.Now, StartRRT, ReceiveRRT, guidLists, CanTransShift);
                using(IntegratedServerUsersDBContext dbContext = new IntegratedServerUsersDBContext())
                {
                    await dbContext.AllServerUsers.AddAsync(newUser);
                    await dbContext.SaveChangesAsync();
                    AllCurrentUsersList=await dbContext.AllServerUsers.ToListAsync();
                }
                IsModifyingUsersList = false;
                UserSessionDetails newUserDeets = new UserSessionDetails();
                newUserDeets.SessionId = newSessionId.ToString();
                newUserDeets.UserName = usrname;
                newUserDeets.SessionExpiry = NewSessionExp;
                newUserDeets.CookieSessionID = newCookieId.ToString();
                UserSessions.Add(usrname, newCookieId.ToString());
                return newUserDeets;
            }
            //add code for other user classes later
            else
            {
                throw new Exception();
            }
            
        }

        public static async Task<UserSessionDetails> AddNewStandAloneUserAsync(string usrname, string passwd, ProviderClass usrType, bool StartRRT, bool ReceiveRRT, string grpID, bool CanTransShift)
        {
        StartAddSTAloneUser:
            if (IsModifyingStandaloneusersList)
            {
                await Task.Delay(100);
                goto StartAddSTAloneUser;
            }
            IsModifyingStandaloneusersList = true;
            await GetAllStandaloneUsersListAsync();
            foreach (User usr in AllCurrentStandaloneUsersList)
            {
                if (usrname == usr.UserName)
                {
                    IsModifyingStandaloneusersList = false;
                    throw new UserAlreadyExistsException(usrname);
                }
            }
            if (usrType == ProviderClass.Doctor)
            {
                DoctorUser newUser = new DoctorUser();
                Log.AddLog("Adding new STANDALONE RRT user with username: (" + usrname + ") password: (" + passwd +
                    ") designation: (" + usrType.ToString() + ") Start rapid responses set to " + StartRRT.ToString() +
                    " and receive RR alerts set to " + ReceiveRRT.ToString() + " and group ID: (" + grpID.ToString() +
                    ") and ability to transfer shift as " + CanTransShift.ToString(), LogLevel.Info);
                //for production releases dont write passwords to log
                string HashedPass = HashPassword(passwd);
                Log.AddLog("Hashed password is " + HashedPass, LogLevel.Debug);
                DateTime NewSessionExp = DateTime.Now.AddMonths(2);
                Guid newSessionId = Guid.NewGuid();
                Guid newCookieId = Guid.NewGuid();
                Log.AddLog("New session ID is " + newSessionId.ToString() + " and will expire on " + NewSessionExp.ToString(), LogLevel.Debug);
                List<string> guidLists = new List<string>();
                guidLists.Add(grpID);
                newUser.CreateNewDoctorUser(usrname, HashedPass, usrType, newSessionId.ToString(), NewSessionExp, DateTime.Now, StartRRT, ReceiveRRT, guidLists, CanTransShift);
                using (StandAloneRRTMembersDBContext dbContext = new StandAloneRRTMembersDBContext())
                {
                    await dbContext.ListOfStandaloneUsers.AddAsync(newUser);
                    await dbContext.SaveChangesAsync();
                    AllCurrentUsersList = await dbContext.ListOfStandaloneUsers.ToListAsync();
                }
                IsModifyingStandaloneusersList = false;
                UserSessionDetails newUserDeets = new UserSessionDetails();
                newUserDeets.SessionId = newSessionId.ToString();
                newUserDeets.UserName = usrname;
                newUserDeets.SessionExpiry = NewSessionExp;
                newUserDeets.CookieSessionID = newCookieId.ToString();
                StandaloneUserSessions.Add(usrname, newCookieId.ToString());
                return newUserDeets;
            }
            //add code for other user classes later
            else
            {
                IsModifyingStandaloneusersList = false;
                throw new Exception();
            }

        }

        public static async Task<UserSessionDetails> ValidateUserPassAsync(string username, string password)
        {
            await GetAllUsersListAsync();
            string HashedPass = HashPassword(password);
            bool foundCorrectUser = false;
            User correctUser = new User();
            int i = 0;
            foreach(User eachuser in AllCurrentUsersList)
            {
                if (eachuser.UserName == username)
                {
                    correctUser= eachuser;
                    foundCorrectUser = true;
                    byte[] combinedBytes = Convert.FromBase64String(eachuser.HashedPassword);
                    byte[] salt = new byte[SaltSize];
                    byte[] hash = new byte[HashSize];
                    Array.Copy(combinedBytes, 0, salt, 0, SaltSize);
                    Array.Copy(combinedBytes, SaltSize, hash, 0, HashSize);
                    byte[] newHash = GenerateHash(password, salt);
                    if (!CryptographicOperations.FixedTimeEquals(hash, newHash))
                    {
                        throw new UserWrongPasswordException();
                    }
                }
                i += 1;
            }
            if (!foundCorrectUser)
            {
                throw new UserDoesNotExistException();
            }
            string cookieID = "";
            if (UserSessions.ContainsKey(username))
            {
                cookieID = UserSessions[username];
            }
            else
            {
                cookieID = Guid.NewGuid().ToString();
                UserSessions.Add(username, cookieID);
            }
            DateTime newExpiry = DateTime.Now.AddMonths(2);
            if (UpdateUserSessionDetailsAsync(username, correctUser.CurrentSessionID, newExpiry).Result)
            {
                UserSessionDetails replyDetails = new UserSessionDetails();
                replyDetails.SessionExpiry = newExpiry;
                replyDetails.SessionId = correctUser.CurrentSessionID;
                replyDetails.CookieSessionID = cookieID;
                replyDetails.UserName = username;
                return replyDetails;
            }
            else
            {
                //error code handling
                throw new Exception();
            }
            
            
        }

        public static async Task<UserSessionDetails> ValidateStandaloneUserPassAsync(string username, string password)
        {
            await GetAllStandaloneUsersListAsync();
            string HashedPass = HashPassword(password);
            bool foundCorrectUser = false;
            User correctUser = new User();
            int i = 0;
            foreach (User eachuser in AllCurrentStandaloneUsersList)
            {
                if (eachuser.UserName == username)
                {
                    correctUser = eachuser;
                    foundCorrectUser = true;
                    byte[] combinedBytes = Convert.FromBase64String(eachuser.HashedPassword);
                    byte[] salt = new byte[SaltSize];
                    byte[] hash = new byte[HashSize];
                    Array.Copy(combinedBytes, 0, salt, 0, SaltSize);
                    Array.Copy(combinedBytes, SaltSize, hash, 0, HashSize);
                    byte[] newHash = GenerateHash(password, salt);
                    if (!CryptographicOperations.FixedTimeEquals(hash, newHash))
                    {
                        throw new UserWrongPasswordException();
                    }
                }
                i += 1;
            }
            if (!foundCorrectUser)
            {
                throw new UserDoesNotExistException();
            }
            string cookieID = "";
            if (StandaloneUserSessions.ContainsKey(username))
            {
                cookieID = StandaloneUserSessions[username];
            }
            else
            {
                cookieID = Guid.NewGuid().ToString();
                StandaloneUserSessions.Add(username, cookieID);
            }
            DateTime newExpiry = DateTime.Now.AddMonths(2);
            if (UpdateStandaloneUserSessionDetailsAsync(username, correctUser.CurrentSessionID, newExpiry).Result)
            {
                UserSessionDetails replyDetails = new UserSessionDetails();
                replyDetails.SessionExpiry = newExpiry;
                replyDetails.SessionId = correctUser.CurrentSessionID;
                replyDetails.CookieSessionID = cookieID;
                replyDetails.UserName = username;
                return replyDetails;
            }
            else
            {
                //error code handling
                throw new Exception();
            }
        }

        public static async Task<bool> ValidateUserCookieAsync(string username, string cookieID)
        {
            string correctCookieId = "aaa";
            if (UserSessions.ContainsKey(username))
            {
                correctCookieId=UserSessions[username];

                User correctUser = new User();
                correctUser = AllCurrentUsersList.Where(x=>x.UserName == username).FirstOrDefault();
                if (correctUser!=null)
                {
                    if ((correctCookieId == cookieID) && (correctUser.SessionExpiry > DateTime.Now))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
                
            }
            else
            {
                throw new UserExpiredCookieException();
            }
        }

        public static async Task<bool> ValidateStandaloneUserCookieAsync(string username, string cookieID)
        {
            string correctCookieId = "aaa";
            if (StandaloneUserSessions.ContainsKey(username))
            {
                correctCookieId = UserSessions[username];

                User correctUser = new User();
                correctUser = AllCurrentStandaloneUsersList.Where(x => x.UserName == username).FirstOrDefault();
                if (correctUser != null)
                {
                    if ((correctCookieId == cookieID) && (correctUser.SessionExpiry > DateTime.Now))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;

            }
            else
            {
                throw new UserExpiredCookieException();
            }
        }

        public static async Task<bool> UpdateStandaloneUserSessionDetailsAsync(string username, string sessionID, DateTime newExpiry)
        {
            using (StandAloneRRTMembersDBContext dbContext = new StandAloneRRTMembersDBContext())
            {
                User correctUser = await dbContext.ListOfStandaloneUsers.FirstOrDefaultAsync(x => x.UserName == username);
                if (correctUser != null)
                {
                    correctUser.CurrentSessionID = sessionID;
                    correctUser.SessionExpiry = newExpiry;
                    await dbContext.SaveChangesAsync();
                    await GetAllUsersListAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static async Task<bool> UpdateUserSessionDetailsAsync(string username, string sessionID, DateTime newExpiry)
        {
            using (IntegratedServerUsersDBContext dbContext = new IntegratedServerUsersDBContext())
            {
                User correctUser = await dbContext.AllServerUsers.FirstOrDefaultAsync(x => x.UserName == username);
                if (correctUser!=null)
                {
                    correctUser.CurrentSessionID = sessionID;
                    correctUser.SessionExpiry=newExpiry;
                    await dbContext.SaveChangesAsync();
                    await GetAllUsersListAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

       // public static UserSessionDetails ValidateUserByCookieID(string username, string cookieID)
       // {

       // }

        private static string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Create hash
            byte[] hash = GenerateHash(password, salt);

            // Combine salt and hash
            var combinedBytes = new byte[salt.Length + hash.Length];
            Array.Copy(salt, 0, combinedBytes, 0, salt.Length);
            Array.Copy(hash, 0, combinedBytes, salt.Length, hash.Length);

            // Convert to base64 for storage
            return Convert.ToBase64String(combinedBytes);
        }

        private static byte[] GenerateHash(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = DegreeOfParallelism,
                Iterations = Iterations,
                MemorySize = MemorySize
            };

            return argon2.GetBytes(HashSize);
        }

    }
}
