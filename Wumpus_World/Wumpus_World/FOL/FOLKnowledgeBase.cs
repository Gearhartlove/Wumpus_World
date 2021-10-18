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

        /// <summary>
        /// This method represents the inference step
        /// </summary>
        public void infer() {
            processPercepts();
            reduceComplexFacts();
        }
        
        /// <summary>
        /// This method goes through every percept that was added last frame and adds them to the knowledge base
        /// by unification.
        /// </summary>
        private void processPercepts() {
            while (percepts.Count != 0) {
                FOLFact current = percepts.Dequeue();
                foreach (var rule in rules) {
                    var result = rule.unify(current);
                    if(result != null) addFacts(result.separate().ToArray());
                }
            }
        }

        /// <summary>
        /// This is the reduction stage of our algorithm. This method tried to break down 'P() V P()' facts based on
        /// other facts.
        /// </summary>
        public void reduceComplexFacts() {
            int startCount = complexFacts.Count;
            for(int i = 0; i < startCount; i++) {
                FOLFact complexFact = complexFacts.Dequeue();

                var unknowns = new List<FOLFact>();
                var knowns = new List<KnowledgeQuery>();
                
                FOLFact current = complexFact;
                do {
                    if(!validStateCheck(current)) {
                        knowns.Add(KnowledgeQuery.FALSE);
                    }
                    else {
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
                    }
                    
                    current = current.Next;
                } while(current != null);

                if(unknowns.Count == 0) continue;
                
                if (unknowns.Count == 1) {
                    bool allFalse = knowns.All(a => a == KnowledgeQuery.FALSE);
                    if (allFalse) {
                        var f = unknowns.First();
                        if (!cantBeOverriden(f.Type, f.X, f.Y)) {
                            Console.WriteLine("Fact Inferred! " + unknowns.First());
                            addFacts(unknowns.First());
                        }
                        continue;
                    }
                }
                
                complexFacts.Enqueue(complexFact);
            }
        }

        /// <summary>
        /// This is a method that returns true if the cell has a predicate of that type in a complex fact.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool isSuspected(PredicateType type, int x, int y) {
            foreach (var complexFact in complexFacts) {
                var current = complexFact;
                do {
                    if (current.Type == type && current.X == x && current.Y == y) return true;
                    current = current.Next;
                } while (current.hasNext());
            }

            return false;
        }
        
        //Valid Check will check to see, based on facts, if a claim is valid or not.
        /// <summary>
        /// This method will check if the fact is valid based on what is in our knowledge base. This check is only
        /// applied to facts that haven't yet been added to the knowledge base. This is used to reduce our complex
        /// facts that we receive from some of the rules.
        /// </summary>
        /// <param name="fact"></param>
        /// <returns></returns>
        private bool validStateCheck(FOLFact fact) {
            switch (fact.Type) {
                case PredicateType.PIT:
                    return !hasFactSurrounding(fact.X, fact.Y, PredicateType.EMPTY)
                        || !hasFactAt(fact.X, fact.Y, PredicateType.WUMPUS, PredicateType.OBSTACLE, PredicateType.GOLD, PredicateType.EMPTY, PredicateType.SAFE);
                case PredicateType.WUMPUS:
                    return !hasFactSurrounding(fact.X, fact.Y, PredicateType.EMPTY)
                        || !hasFactAt(fact.X, fact.Y, PredicateType.PIT, PredicateType.OBSTACLE, PredicateType.EMPTY, PredicateType.SAFE, PredicateType.GOLD);
                case PredicateType.GOLD:
                    return !hasFactSurrounding(fact.X, fact.Y, PredicateType.EMPTY)
                        || !hasFactAt(fact.X, fact.Y, PredicateType.WUMPUS, PredicateType.PIT, PredicateType.OBSTACLE, PredicateType.EMPTY, PredicateType.SAFE);
            }
            
            return false;
        }

        /// <summary>
        /// Checks if a certain fact or facts are in at least on adjacent cell. Used in the validate method.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        private bool hasFactSurrounding(int x, int y, params PredicateType[] types) {
            foreach (var type in types) {
                foreach (var fact in getSimpleFactsByType(type)) {
                    if(fact.Not) continue;
                    
                    var b = (fact.X + 1 == x && fact.Y == y)
                         || (fact.X - 1 == x && fact.Y == y)
                         || (fact.X == x && fact.Y + 1 == y)
                         || (fact.X == x && fact.Y - 1 == y);

                    if (b) return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a certain fact or facts are in the current cell. Used in the validate method.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        private bool hasFactAt(int x, int y, params PredicateType[] types) {
            foreach (var type in types) {
                foreach (var fact in getSimpleFactsByType(type)) {
                    if(fact.Not) continue;
                    if (fact.X == x && fact.Y == y) return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Simple method that sorts the simple fact from the complex ones.
        /// </summary>
        /// <param name="facts"></param>
        private void addFacts(params FOLFact[] facts) {
            foreach (var fact in facts) {
                if (fact.length() > 1) {
                    if(!complexFacts.Contains(fact)) complexFacts.Enqueue(fact);
                } else {
                    addSimpleFact(fact);
                }
            }
        }

        /// <summary>
        /// Helper Method for filtering facts that should be added to the simple fact list.
        /// </summary>
        /// <param name="fact"></param>
        private void addSimpleFact(FOLFact fact) {
            if(!simpleFacts.ContainsKey(fact.Type)) simpleFacts[fact.Type] = new List<FOLFact>();
            if(simpleFacts[fact.Type].Contains(fact)) return;
            if(cantBeOverriden(fact.Type, fact.X, fact.Y)) return;
            simpleFacts[fact.Type].Add(fact);
        }

        /// <summary>
        /// Helper method to return a fact of the given type at the given location.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public FOLFact getSimpleFact(PredicateType type, int x, int y) {
            foreach (var fact in getSimpleFactsByType(type)) {
                if (fact.X == x && fact.Y == y) return fact;
            }

            return null;
        }

        /// <summary>
        /// Helper method to return all facts of the given type at the given location.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<FOLFact> getSimpleFactsByType(PredicateType type) {
            if(!simpleFacts.ContainsKey(type)) simpleFacts[type] = new List<FOLFact>();
            return simpleFacts[type];
        }

        /// <summary>
        /// This method queries the state of the fact at the given point. I returns either TRUE, FALSE, or UNKNOWN.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public KnowledgeQuery queryFact(PredicateType type, int x, int y) {
            FOLFact fact = getSimpleFact(type, x, y);
            if (fact == null) return KnowledgeQuery.UNKNOWN;
            if (fact.Not) return KnowledgeQuery.FALSE;
            return KnowledgeQuery.TRUE;
        }

        /// <summary>
        /// Adds a fact to the percepts list for processing.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void addPercept(PredicateType type, int x, int y) {
            if (cantBeOverriden(type, x, y)) return;
            FOLFact fact = new FOLFact(type, x, y, this.boardX, this.boardY, this);
            percepts.Enqueue(fact);
        }
        
        
        /// <summary>
        /// Checks if the given cell type can be overriden.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool cantBeOverriden(PredicateType type, int x, int y) {
            return this.queryFact(PredicateType.PIT, x, y) == KnowledgeQuery.TRUE ||
                    this.queryFact(PredicateType.GOLD, x, y) == KnowledgeQuery.TRUE ||
                    this.queryFact(PredicateType.OBSTACLE, x, y) == KnowledgeQuery.TRUE ||
                    this.queryFact(PredicateType.WUMPUS, x, y) == KnowledgeQuery.TRUE ||
                    this.queryFact(PredicateType.EMPTY, x, y) == KnowledgeQuery.TRUE;
        }

        
        /// <summary>
        /// Returns the String representation of the Simple Facts.
        /// </summary>
        /// <returns></returns>
        public string simpleFactsString() {
            var res = "";
            foreach (var facts in simpleFacts.Values) {
                if(facts.Count > 0) res += string.Join(", ", facts) + ", ";
            }

            return res;
        }

        /// <summary>
        /// Returns the String representation of the Complex Facts.
        /// </summary>
        /// <returns></returns>
        public string complexFactsString() {
            return string.Join(", ", complexFacts);
        }

        
        /// <summary>
        /// Returns the String representation of the Knowledge Base.
        /// </summary>
        /// <returns></returns>
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