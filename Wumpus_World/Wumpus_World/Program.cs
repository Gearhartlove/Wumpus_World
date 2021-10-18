using System;
using System.Globalization;

namespace Wumpus_World
{
    class Program
    {
        static void Main(string[] args) {
            // FOAgent foAgent = new FOAgent();
            // ReflexAgent reflexAgent = new ReflexAgent();
            // Driver driver = new Driver(foAgent, reflexAgent); 
            // driver.RunTestWumpusWorld(foAgent); // test Wumpus World on 5x5 for specific agent (debugging)
            // //driver.RunWumpusWord();


            Board board = new Board(5);
            FOLAgent agent = new FOLAgent();
            agent.SetBoard(board);
            
            agent.Navigate(board);
        }
    }
}