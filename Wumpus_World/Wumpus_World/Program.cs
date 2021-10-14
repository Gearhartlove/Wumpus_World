using System;

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

            FOLFact fact = new FOLFact(PredicateType.PIT, 1, 2);
                
            fact.and(PredicateType.PIT, 3, 4)
                .and(PredicateType.PIT, 34, 5)
                .or(PredicateType.PIT, 5, 6);

            var list = fact.separate();

            PitQuantifier quantifier = new PitQuantifier();

            Console.WriteLine(String.Join(", ", ((FOLRule)quantifier).unify(new FOLFact(PredicateType.PIT, 1, 1)).separate()));
            
            Console.WriteLine(fact);
            Console.WriteLine(string.Join(", ", list));
        }
    }
}