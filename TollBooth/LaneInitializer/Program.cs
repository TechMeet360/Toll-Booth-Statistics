using System;
using System.Configuration;
using System.Diagnostics;

namespace LaneInitializer
{
    class Program
    {
        static int laneInstances = Convert.ToInt16(ConfigurationManager.AppSettings["LaneInstances"]);
        static void Main(string[] args)
        {
            Process _processLaneInitializer;

            try
            {
                int lane = 1;
                while (lane <= laneInstances)
                {
                    _processLaneInitializer = new Process();
                    _processLaneInitializer.StartInfo.UseShellExecute = true;
                    _processLaneInitializer.StartInfo.FileName = ConfigurationManager.AppSettings["ApplicationPath"];
                    _processLaneInitializer.StartInfo.Arguments = "-n";
                    _processLaneInitializer.StartInfo.Arguments = lane.ToString();
                    _processLaneInitializer.Start();

                    lane++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
