using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading;

namespace LaneScanner
{
    class Program
    {
        static string eventHubName = ConfigurationManager.AppSettings["EventHubName"];
        static string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
        static string registrationState = ConfigurationManager.AppSettings["RegistrationState"];

        static void Main(string[] args)
        {
            int laneNo = Convert.ToInt16(args[0]);

            ScanVehicles(laneNo);
        }

        static void ScanVehicles(int laneNo)
        {
            var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);
            while (true)
            {
                try
                {
                    var vehicleNo = $"{registrationState} 0{laneNo} { RandomGenerator.GetLetter()} { RandomGenerator.Get4DigitNumber()}";
                    var tollCharges = RandomGenerator.GetRandomCharge();
                    var message = $"{{  \"LaneNo\" : {laneNo}, \"VehicleNo\" : \"{vehicleNo}\", \"Payment\" : {{ \"Charges\" : {tollCharges}, \"Status\" : \"{(tollCharges != 0)}\" }}, \"Timestamp\" : \"{DateTime.Now}\" }}";                    
                    Console.WriteLine($"{DateTime.Now} > Scanned Vehicle: {vehicleNo}");
                    eventHubClient.Send(new EventData(Encoding.UTF8.GetBytes(message)));
                }
                catch (Exception exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                    Console.ResetColor();
                }

                Thread.Sleep(RandomGenerator.GetRandomInterval());
            }
        }

        static class RandomGenerator
        {
            static Random _random = new Random();
            public static char GetLetter()
            {
                // This method returns a random lowercase letter.
                // ... Between 'a' and 'z' inclusize.
                int num = _random.Next(0, 26); // Zero to 25
                char let = (char)('A' + num);
                return let;
            }

            public static int Get4DigitNumber()
            {
                int _min = 0000;
                int _max = 9999;
                Random _rdm = new Random();
                return _rdm.Next(_min, _max);
            }

            public static int GetRandomInterval()
            {
                List<int> intervals = new List<int>();                
                intervals.Add(100);
                intervals.Add(200);
                intervals.Add(400);
                intervals.Add(5000);
                intervals.Add(8000);
                intervals.Add(10000);
                Random _rdm = new Random();
                int r = _rdm.Next(intervals.Count);
                return intervals[r];
            }

            public static int GetRandomCharge()
            {
                List<int> charges = new List<int>();
                charges.Add(50);
                charges.Add(100);
                charges.Add(10);
                charges.Add(450);
                charges.Add(0);
                Random _rdm = new Random();
                int r = _rdm.Next(charges.Count);
                return charges[r];
            }
        }
    }
}
