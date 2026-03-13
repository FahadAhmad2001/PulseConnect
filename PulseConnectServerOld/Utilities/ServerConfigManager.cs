using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ProtoBuf;
using PulseConnectLib.Definitions.Entities;

namespace PulseConnectServerOld.Utilities
{
    public static class ServerConfigManager
    {
        
        public static List<User> CurrentUsersList = new List<User>();
        
        public static void WriteDataToStorage()
        {
            
            UsersData alluserdata = new UsersData();
            alluserdata.UsersList = AuthenticationManager.AllCurrentUsersList;
            Log.AddLog("Writing user data to file..", LogLevel.Debug);
            if (File.Exists("userdata.db"))
            {
                Log.AddLog("Backing up old user data first...", LogLevel.Debug);
                if (File.Exists("userdata.db.bak"))
                {
                    File.Delete("userdata.db.bak");
                }
                File.Copy("userdata.db", "usesrdata.db.bak");
                File.Delete("userdata.db");
            }
            using (var file = File.Create("userdata.db"))
            {
                Log.AddLog("Serializing user list to userdata.db", LogLevel.Debug);
                Serializer.Serialize(file, alluserdata);
            }

        }
        
        
    }
    

    [Serializable]
    public struct UsersData
    {
        [ProtoMember(1)]
        public List<User> UsersList { get; set; }
        // public List<Groups> GroupsList; add list of a groups class down the line
    }
}
