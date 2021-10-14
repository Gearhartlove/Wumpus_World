using System.Collections.Generic;

namespace Wumpus_World {
    public class FOLKnowledgeBase {
        //Represents the non-Conrete FOL Sentences that are used for the rules
        private List<FOLRule> rules = new List<FOLRule>();
        
        //Represents the Concrete Facts of the environment
        private List<FOLFact> facts = new List<FOLFact>();

        //Facts to be infered
        private Queue<FOLFact> percepts = new Queue<FOLFact>();

        public void inferenceStep() {
            for (int i = 0; i < percepts.Count; i++) {
                FOLFact current = percepts.Dequeue();
                
                
            }
        }

        private bool processPercepts() {
            return false;
        }
    }
}