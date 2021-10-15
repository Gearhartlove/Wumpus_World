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
            knowledgeBase.addPercept(PredicateType.EMPTY, 1, 1);
            knowledgeBase.infer();
            
            Console.WriteLine(knowledgeBase);
        }
    }
}