using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;

namespace Wumpus_World {
    public class FOLKnowledgeBase {
        //Represents the non-Conrete FOL Sentences that are used for the rules
        //These are Defined in FOLRule.cs
        private List<FOLRule> rules = new List<FOLRule>();
        
        //Represents the Simple Concrete Facts of the environment. These facts are only 1 clause long.
        //This can not and will not be simplified.
        private Dictionary<PredicateType, List<FOLFact>> simpleFacts = new Dictionary<PredicateType, List<FOLFact>>();
        
        //Represents the Complex Contrete Facts of the environment. These facts are put through the inference engine every set.
        private Queue<FOLFact> complexFacts = new Queue<FOLFact>();

        //Facts to be applied to rules
        private Queue<FOLFact> percepts = new Queue<FOLFact>();

        private int boardX, boardY;
        
        public FOLKnowledgeBase(int width, int height) {
            rules.Add(new PitQuantifier(width, height, this));
            rules.Add(new BreezeQuantifier(width, height, this));
            rules.Add(new WumpusQuantifier(width, height, this));
            rules.Add(new StenchQuantifier(width, height, this));
            rules.Add(new ObstacleQuantifier(width, height, this));
            rules.Add(new GoldQuantifier(width, height, this));
            rules.Add(new GlitterQuantifier(width, height, this));
            rules.Add(new SafeQuantifier(width, height, this));
            rules.Add(new EmptyQualifier(width, height, this));

            this.boardX = width;
            this.boardY = height;
        }

        public void infer() {
            processPercepts();
            ruleQuery();
            processComplexFacts();
        }
        
        private void processPercepts() {
            while (percepts.Count != 0) {
                FOLFact current = percepts.Dequeue();
                foreach (var rule in rules) {
                    var result = rule.unify(current);
                    if(result != null) addFacts(result.separate().ToArray());
                }
            }
        }

        public void processComplexFacts() {
            int startCount = complexFacts.Count;
            for(int i = 0; i < startCount; i++) {
                FOLFact complexFact = complexFacts.Dequeue();

                var unknowns = new List<FOLFact>();
                var knowns = new List<KnowledgeQuery>();
                
                FOLFact current = complexFact;
                do {
                    var sf = queryFact(current.Type, current.X, current.Y);
                    switch (sf) {
                        case KnowledgeQuery.UNKNOWN:
                            unknowns.Add(current.singleOut());
                            break;
                        case KnowledgeQuery.TRUE:
                        case KnowledgeQuery.FALSE:
                            knowns.Add(sf);
                            break;
                    }
                    current = current.Next;
                } while(current != null);

                if(unknowns.Count == 0) continue;
                
                if (unknowns.Count == 1) {
                    bool allFalse = knowns.All(a => a == KnowledgeQuery.FALSE);
                    if (allFalse) {
                        var f = unknowns.First();
                        if(!cantBeOverriden(f.Type, f.X, f.Y)) addFacts(unknowns.First());
                        continue;
                    }
                }
                
                complexFacts.Enqueue(complexFact);
            }
        }
        
        //Valid Check will check to see, based on facts, if a claim is valid or not.
        public bool validCheck(FOLFact fact) {
            
        }

        public void ruleQuery() {
            foreach (var rule in rules) {
                var result = rule.queryKnowledgeBase(this);
                if(result != null) addFacts(result.separate().ToArray());
            }
        }

        private void addFacts(params FOLFact[] facts) {
            foreach (var fact in facts) {
                if (fact.length() > 1) {
                    complexFacts.Enqueue(fact);
                } else {
                    addSimpleFact(fact);
                }
            }
        }

        private void addSimpleFact(FOLFact fact) {
            if(!simpleFacts.ContainsKey(fact.Type)) simpleFacts[fact.Type] = new List<FOLFact>();
            if(simpleFacts[fact.Type].Contains(fact)) return;
            if(cantBeOverriden(fact.Type, fact.X, fact.Y)) return;
            simpleFacts[fact.Type].Add(fact);
        }

        private FOLFact getSimpleFact(PredicateType type, int x, int y) {
            if (!simpleFacts.ContainsKey(type)) return null;
            foreach (var fact in simpleFacts[type]) {
                if (fact.X == x && fact.Y == y) return fact;
            }

            return null;
        }
        
        public KnowledgeQuery queryFact(PredicateType type, int x, int y) {
            FOLFact fact = getSimpleFact(type, x, y);
            if (fact == null) return KnowledgeQuery.UNKNOWN;
            if (fact.Not) return KnowledgeQuery.FALSE;
            return KnowledgeQuery.TRUE;
        }

        public FOLFact[] queryFactType(PredicateType type) {
            return simpleFacts[type].ToArray();
        }

        public void addPercept(PredicateType type, int x, int y) {
            if (cantBeOverriden(type, x, y)) return;
            FOLFact fact = new FOLFact(type, x, y, this.boardX, this.boardY, this);
            percepts.Enqueue(fact);
        }
        
        public bool cantBeOverriden(PredicateType type, int x, int y) {
            return this.queryFact(PredicateType.PIT, x, y) == KnowledgeQuery.TRUE ||
                    this.queryFact(PredicateType.GOLD, x, y) == KnowledgeQuery.TRUE ||
                    this.queryFact(PredicateType.OBSTACLE, x, y) == KnowledgeQuery.TRUE ||
                    this.queryFact(PredicateType.WUMPUS, x, y) == KnowledgeQuery.TRUE ||
                    this.queryFact(PredicateType.EMPTY, x, y) == KnowledgeQuery.TRUE;
        }

        public string simpleFactsString() {
            var res = "";
            foreach (var facts in simpleFacts.Values) {
                res += string.Join(", ", facts) + ", ";
            }

            return res;
        }

        public string complexFactsString() {
            return string.Join(", ", complexFacts);
        }

        public override string ToString() {
            return simpleFactsString() + "\n" + complexFactsString();
        }
    }

    public enum KnowledgeQuery {
        TRUE,
        FALSE,
        UNKNOWN
    }
}