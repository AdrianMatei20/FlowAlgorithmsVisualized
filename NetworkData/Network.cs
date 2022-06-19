using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkData
{
    public sealed class Network
    {
        private static Network instance = null;

        private Network()
        {
        }

        public static Network Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Network();
                }
                return instance;
            }
        }

        public static string GetCapacityNetwork()
        {
            string filepath = "..\\NetworkData\\Networks\\Network_1.dot";
            string dotNetwork = File.ReadAllText(filepath);
            return dotNetwork;
        }

        public static string GetFlowNetwork()
        {
            string filepath = "..\\NetworkData\\Networks\\Network_1.dot";
            string dotNetwork = File.ReadAllText(filepath);
            return dotNetwork;
        }
    }
}
