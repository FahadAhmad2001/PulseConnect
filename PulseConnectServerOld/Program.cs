using PulseConnectLib.Definitions.Events;
using System.Threading;
using MySqlConnector;
using PulseConnectServerOld.Utilities;

namespace PulseConnectServerOld
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Thread testThread = new Thread(InitializeServer);
            testThread.Start();
            Console.ReadLine();
        }

        static async void InitializeServer()
        {
            Log.StartLogging();
            await AuthenticationManager.LoadUserDetails();
            await StandAloneRRTManager.CreateDB();
            MainServer server = new MainServer();
            server.StartServer(10500);
        }

        static async void PerformDatabaseQueries()
        {
            Log.StartLogging();
            await StandAloneRRTManager.CreateDB();
            await StandAloneRRTManager.GetAllEventsList();
            RapidResponseEvent event1 = GenerateEvent("patient1",RapidResponseEventType.CardiacArrest,DateTime.Now,"Helb","Ward 0","nurse1");
            await StandAloneRRTManager.AddNewStandAloneRRTEvent(event1);
            await StandAloneRRTManager.GetAllEventsList();
            Thread.Sleep(3000);
            RapidResponseEvent event2 = GenerateEvent("patient2", RapidResponseEventType.Tachycardia, DateTime.Now, "Helb", "Ward 123", "juniordr2");
            await StandAloneRRTManager.AddNewStandAloneRRTEvent(event2);
            await StandAloneRRTManager.GetAllEventsList();
            Thread.Sleep(3000);
            RapidResponseEvent event3 = GenerateEvent("patient3", RapidResponseEventType.CardiacArrest, DateTime.Now, "Helb", "Ward 4566", "reg1");
            await StandAloneRRTManager.AddNewStandAloneRRTEvent(event3);
            await StandAloneRRTManager.GetAllEventsList();
            Thread.Sleep(3000);
        }

        static RapidResponseEvent GenerateEvent(string patname, RapidResponseEventType eventClass, DateTime eventCallTime, string eventMsg, string eventLoc, string caller, bool isRosc=false, int ttr = 0)
        {
            RapidResponseEvent newEvent = new RapidResponseEvent();
            WardBed location;
            location.WardName = "aaa";
            location.BedNumber = "12";
            newEvent.CreateRapidResponseEvent(patname,eventClass,eventCallTime,eventMsg,eventLoc,caller,location);
        /*    newEvent.EventCallerGuid = caller;
            newEvent.EventGuid=Guid.NewGuid().ToString();
            newEvent.PatientCurrentLocationGuid = eventLoc;
            newEvent.PatientGuid = patname;
            newEvent.EventData = eventMsg;
            newEvent.EventClass = MedEventType.Arrest;
            newEvent.EventSeverity = MedEventSeverity.Critical;
            newEvent.EventTime= eventCallTime;
            newEvent.RoscAchieved = isRosc;
            newEvent.RRTEventType= eventClass;
            newEvent.TimeToRosc= ttr;
            newEvent.Id = newEvent.EventGuid.GetHashCode(); */
            return newEvent;
        }

        
    }
}
