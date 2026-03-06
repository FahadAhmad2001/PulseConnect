using Konscious.Security.Cryptography;
using PulseConnectLib.Definitions;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;


namespace PulseConnectServer.Utilities
{
    public static class AuthenticationManager
    {
        public static bool IsModifyingUsersList = false;
        private const int SaltSize = 16; // 128 bits
        private const int HashSize = 32; // 256 bits
        private const int DegreeOfParallelism = 8; // Number of threads to use
        private const int Iterations = 4; // Number of iterations
        private const int MemorySize = 65536; // 1 GB
        public static List<User> AllCurrentUsersList { get; set; }
        public static void LoadUserDetails(List<User> AllUsersList)
        {
            AllCurrentUsersList = AllUsersList;
        }

        public static UserSessionDetails AddNewUser(string usrname, string passwd, ProviderClass usrDesig, bool StartRRT, bool ReceiveRRT, string grpID, bool CanTransShift)
        {
        StartAddUser:
            if (IsModifyingUsersList)
            {
                Thread.Sleep(100);
                goto StartAddUser;
            }
            IsModifyingUsersList = true;
            bool DoesUserExist = false;
            foreach (User eachusr in AllCurrentUsersList)
            {
                if (eachusr.UserName == usrname)
                {
                    DoesUserExist = true;
                }
            }
            if (DoesUserExist)
            {
                IsModifyingUsersList = false;
                throw new UserAlreadyExistsException(usrname);
            }
            //for production releases dont write passwords to log
            Log.AddLog("Adding new user with username: (" + usrname + ") password: (" + passwd + ") designation: (" + usrDesig.ToString() + ") Start rapid responses set to " + StartRRT.ToString() + " and receive RR alerts set to " + ReceiveRRT.ToString() + " and group ID: (" + grpID.ToString() + ") and ability to transfer shift as " + CanTransShift.ToString(), LogLevel.Info);
            string HashedPass = HashPassword(passwd);
            Log.AddLog("Hashed password is " + HashedPass, LogLevel.Debug);
            DateTime NewSessionExp = DateTime.Now.AddMonths(2);
            Guid newSessionId = Guid.NewGuid();
            Log.AddLog("New session ID is " + newSessionId.ToString() + " and will expire on " + NewSessionExp.ToString(), LogLevel.Debug);
            User newuser = new User(usrname,HashedPass,usrDesig,StartRRT, ReceiveRRT, grpID, CanTransShift,newSessionId.ToString(),NewSessionExp);
            AllCurrentUsersList.Add(newuser);
            IsModifyingUsersList = false;
            UserSessionDetails newUserDeets = new UserSessionDetails();
            newUserDeets.SessionId = newSessionId.ToString();
            newUserDeets.UserName = usrname;
            newUserDeets.SessionExpiry = NewSessionExp;
            return newUserDeets;
        }

       // public static UserSessionDetails ValidateUserPass(string username, string password)
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
