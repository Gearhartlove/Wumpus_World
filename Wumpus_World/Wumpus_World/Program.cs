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

            FOLKnowledgeBase knowledgeBase = new FOLKnowledgeBase(5, 5);
            
            FOLFact fact1 = new FOLFact(PredicateType.BREEZE, 1, 1, 5, 5, knowledgeBase);
            FOLFact fact2 = new FOLFact(PredicateType.BREEZE, 1, 1, 5, 5, knowledgeBase);
            
            //Console.WriteLine(fact1.Equals(fact2));
            

            Board board = new Board(5);
            FOLAgent agent = new FOLAgent();
            agent.SetBoard(board);
            
            agent.Navigate(board);
        }
    }
}