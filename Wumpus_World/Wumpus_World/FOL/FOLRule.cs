namespace Wumpus_World {
    
    //Defines rule functionality for ∀ x,y [PRECONDICTION] => [EVAL Return]
    public interface FOLRule {
        FOLFact eval(int x, int y);

        bool precondition(FOLFact fact);

        public FOLFact unify(FOLFact fact) {
            if (precondition(fact)) return eval(fact.X, fact.Y);
            return null;
        }
    }

    /// <summary>
    /// Quantifier 1: Keep your eye down and your finger in the air.
    /// ∀ x,y PIT(x,y) => -SAFE(x,y) ∧ BREEZE(x +=- 1, y +=- 1) 
    /// </summary>
    public class PitQuantifier : FOLRule {
        public FOLFact eval(int x, int y) {
            var fact = new FOLFact(PredicateType.SAFE, x, y).isNegative()
                .and(PredicateType.BREEZE, x, y)
                .and(PredicateType.BREEZE, x + 1, y)
                .and(PredicateType.BREEZE, x - 1, y)
                .and(PredicateType.BREEZE, x, y + 1)
                .and(PredicateType.BREEZE, x, y - 1)
                .and(PredicateType.WUMPUS, x, y).isNegative()
                .and(PredicateType.GOLD, x, y).isNegative()
                .and(PredicateType.OBSTACLE, x, y).isNegative()
                .getHead();

            return fact;
        }

        public bool precondition(FOLFact fact) {
            return fact.Type == PredicateType.PIT && !fact.hasNext();
        }
    }

    /// <summary>
    /// Quantifier 2: The wumpus lurks...
    /// ∀ x,y WUMPUS(x,y) => -SAFE(x,y) ∧ SMELL(x +=- 1, y +=- 1) 
    /// </summary>
    public class WumpusQuantifier : FOLRule {
        public FOLFact eval(int x, int y) {
            return new FOLFact(PredicateType.SAFE, x, y).isNegative()
                .and(PredicateType.SMELL, x, y)
                .and(PredicateType.SMELL, x + 1, y)
                .and(PredicateType.SMELL, x - 1, y)
                .and(PredicateType.SMELL, x, y + 1)
                .and(PredicateType.SMELL, x, y - 1)
                .and(PredicateType.PIT, x, y).isNegative()
                .and(PredicateType.GOLD, x, y).isNegative()
                .and(PredicateType.OBSTACLE, x, y).isNegative()
                .getHead();
        }

        public bool precondition(FOLFact fact) {
            return fact.Type == PredicateType.WUMPUS && !fact.hasNext();
        }
    }
    
    /// <summary>
    /// Quantifier 3 : Walls don't budge.
    /// ∀ x,y OBSTACLE(x,y) => -MOVABLE(x,y)
    /// </summary>
    public class ObstacleQuantifier : FOLRule {
        public FOLFact eval(int x, int y) {
            return new FOLFact(PredicateType.MOVEABLE, x, y).isNegative()
                .and(PredicateType.PIT, x, y).isNegative()
                .and(PredicateType.GOLD, x, y).isNegative()
                .and(PredicateType.OBSTACLE, x, y).isNegative()
                .getHead();
        }

        public bool precondition(FOLFact fact) {
            return fact.Type == PredicateType.OBSTACLE && !fact.hasNext();
        }
    }
    
    /// <summary>
    /// Quantifier 4: X marks the spot.
    /// ∀ x,y GOLD(x,y) => SAFE(x,y) ∧ GLITTER(x+=-1, y+=-1) ∧ -OTHER(x, y)
    /// </summary>
    public class GoldQuantifier : FOLRule {
        public FOLFact eval(int x, int y) {
            return new FOLFact(PredicateType.SAFE, x, y)
                .and(PredicateType.GLITTER, x, y)
                .and(PredicateType.GLITTER, x + 1, y)
                .and(PredicateType.GLITTER, x - 1, y)
                .and(PredicateType.GLITTER, x, y + 1)
                .and(PredicateType.GLITTER, x, y - 1)
                .and(PredicateType.PIT, x, y).isNegative()
                .and(PredicateType.WUMPUS, x, y).isNegative()
                .and(PredicateType.OBSTACLE, x, y).isNegative()
                .getHead();
        }

        public bool precondition(FOLFact fact) {
            return fact.Type == PredicateType.GOLD && !fact.hasNext();
        }
    }

    /// <summary>
    /// Quantifier 5: Where there is a smell, there is a Wumpus.
    /// ∀ x,y SMELL(x,y) => WUMPUS(x+-1, y+-1) v 
    /// </summary>
    public class StenchQuantifier : FOLRule {
        public FOLFact eval(int x, int y) {
            return new FOLFact(PredicateType.WUMPUS, x + 1, y)
                .or(PredicateType.WUMPUS, x - 1, y)
                .or(PredicateType.WUMPUS, x, y + 1)
                .or(PredicateType.WUMPUS, x, y - 1)
                .getHead();
        }

        public bool precondition(FOLFact fact) {
            return fact.Type == PredicateType.SMELL && !fact.hasNext();
        }
    }

    /// <summary>
    /// Quantifier 6: Keep your eyes on the prize.
    /// ∀ x,y GLITTER(x,y) => GOLD(x+-1, y+-1) v 
    /// </summary>
    public class GlitterQuantifier : FOLRule {
        public FOLFact eval(int x, int y) {
            return new FOLFact(PredicateType.GOLD, x + 1, y)
                .or(PredicateType.GOLD, x - 1, y)
                .or(PredicateType.GOLD, x, y + 1)
                .or(PredicateType.GOLD, x, y - 1)
                .getHead();
        }

        public bool precondition(FOLFact fact) {
            return fact.Type == PredicateType.GLITTER && fact.hasNext();
        }
    }
}