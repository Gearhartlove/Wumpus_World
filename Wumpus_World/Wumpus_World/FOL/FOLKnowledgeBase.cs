using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;

namespace Wumpus_World {
    public class FOLKnowledgeBase {
        //Represents the non-Conrete FOL Sentences that are used for the rules
        //These are Defines in FOLRule.cs
        private List<FOLRule> rules = new();
        
        //Represents the Simple Concrete Facts of the environment. These facts are only 1 clause long.
        //This can not and will not be simplified.
        private Dictionary<Tuple<PredicateType, int, int>, FOLFact> simpleFacts = new();
        
        //Represents the Complex Contrete Facts of the environment. These facts are put through the inference engine every set.
        private Queue<FOLFact> complexFacts = new();

        //Facts to be applied to rules
        private Queue<FOLFact> percepts = new();

        public FOLKnowledgeBase() {
            rules.Add(new PitQuantifier());
            
        }
        
        public void processPercepts() {
            while (percepts.Count != 0) {
                FOLFact current = percepts.Dequeue();
                foreach (var rule in rules) {
                    var result = rule.unify(current);
                    if(result != null) addFacts(result.separate().ToArray());
                }
            }
        }

        public void processComplexFacts() {
            while (complexFacts.Count != 0) {
                FOLFact complexFact = complexFacts.Dequeue();

                FOLFact current = complexFact;
            }
        }

        private void addFacts(params FOLFact[] facts) {
            foreach (var fact in facts) {
                if (fact.length() > 1) {
                    complexFacts.Enqueue(fact);
                } else {
                    simpleFacts.Add(new Tuple<PredicateType, int, int>(fact.Type, fact.X, fact.Y), fact);
                }
            }
        }

        public FOLFact queryFact(PredicateType type, int x, int y) {
            return simpleFacts[new Tuple<PredicateType, int, int>(type, x, y)];
        }

        public void addPercept(FOLFact fact) {
            percepts.Enqueue(fact);
        }

        public string simpleFactsString() {
            return string.Join(", ", simpleFacts);
        }
    }
}