using System;

namespace Wumpus_World
{
    class Program
    {
        static void Main(string[] args) {
            FOAgent foAgent = new FOAgent();
            ReflexAgent reflexAgent = new ReflexAgent();
            DebugAgent debugAgent = new DebugAgent();
            Driver driver = new Driver(foAgent, reflexAgent); 
            
            driver.RunTestWumpusWorld(debugAgent); 
            //driver.RunTestWumpusWorld(foAgent); // test Wumpus World on 5x5 for specific agent (debugging)
            //driver.RunWumpusWord();
        }
    }
}