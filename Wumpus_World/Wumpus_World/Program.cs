using System;
using System.Globalization;

namespace Wumpus_World
{
    class Program
    {
        static void Main(string[] args) {
             FOLAgent foAgent = new FOLAgent();
             ReflexAgent reflexAgent = new ReflexAgent();
             Driver driver = new Driver(foAgent, reflexAgent); 
             driver.RunWumpusWord();
        }
    }
}