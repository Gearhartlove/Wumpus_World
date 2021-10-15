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

            FOLKnowledgeBase knowledgeBase = new FOLKnowledgeBase();
            knowledgeBase.addPercept(new FOLFact(PredicateType.PIT, 1, 1));
            knowledgeBase.processPercepts();
            Console.WriteLine(knowledgeBase.simpleFactsString());
        }
    }
}