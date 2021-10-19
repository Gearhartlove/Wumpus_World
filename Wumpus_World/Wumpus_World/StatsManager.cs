using System;
using System.Collections.Generic;

namespace Wumpus_World {
    public class StatsManager {
        public List<List<Statistics>> raStats = new List<List<Statistics>>();
        public List<List<Statistics>> foaStats = new List<List<Statistics>>();
        
        // print average
        public void Average(String agent) {
            if (agent == "foa") {
                Console.WriteLine("First Order Logic Agent Data");
                
            }
            if (agent == "ra") {
                Console.WriteLine("Reflex Agent Data");
                
            }
        }
        // calculate average
    }
}