using System.Runtime.CompilerServices;

namespace Wumpus_World {
    
    //Defines rule functionality for ∀ x,y [PRECONDICTION] => [EVAL Return]
    public abstract class FOLRule {

        protected int width, height;
        protected FOLKnowledgeBase knowledgeBase;

        protected FOLRule(int width, int height, FOLKnowledgeBase knowledgeBase) {
            this.width = width;
            this.height = height;
            this.knowledgeBase = knowledgeBase;
        }

        protected abstract FOLFact eval(int x, int y);

        protected abstract bool precondition(FOLFact fact);

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
        protected override FOLFact eval(int x, int y) {
            var fact = new FOLFact(PredicateType.SAFE, x, y, width, height, knowledgeBase).isNegative()
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

        protected override bool precondition(FOLFact fact) {
            return fact.Type == PredicateType.PIT && !fact.hasNext() && !fact.Not;
        }

        public PitQuantifier(int width, int height, FOLKnowledgeBase knowledgeBase) : base(width, height, knowledgeBase) { }
    }
    
    /// <summary>
    /// Quantifier 2: Is there a draft in here?
    /// ∀ x,y BREEZE(x,y) => WUMPUS(x+-1, y+-1) v 
    /// </summary>
    public class BreezeQuantifier : FOLRule {
        protected override FOLFact eval(int x, int y) {
            var fact = new FOLFact(PredicateType.PIT, x + 1, y, width, height, knowledgeBase)
                .or(PredicateType.PIT, x - 1, y)
                .or(PredicateType.PIT, x, y + 1)
                .or(PredicateType.PIT, x, y - 1)
                .getHead();

            return fact;
        }

        protected override bool precondition(FOLFact fact) {
            return fact.Type == PredicateType.BREEZE && !fact.hasNext() && !fact.Not;
        }

        public BreezeQuantifier(int width, int height, FOLKnowledgeBase knowledgeBase) : base(width, height, knowledgeBase) { }
    }

    /// <summary>
    /// Quantifier 3: The wumpus lurks...
    /// ∀ x,y WUMPUS(x,y) => -SAFE(x,y) ∧ SMELL(x +=- 1, y +=- 1) 
    /// </summary>
    public class WumpusQuantifier : FOLRule {
        protected override FOLFact eval(int x, int y) {
            return new FOLFact(PredicateType.SAFE, x, y, width, height, knowledgeBase).isNegative()
                .and(PredicateType.SMELL, x, y)
                .and(PredicateType.SMELL, x + 1, y)
                .and(PredicateType.SMELL, x - 1, y)
                .and(PredicateType.SMELL, x, y + 1)
                .and(PredicateType.SMELL, x, y - 1)
                .and(PredicateType.PIT, x, y).isNegative()
                .and(PredicateType.GOLD, x, y).isNegative()
                .and(PredicateType.OBSTACLE, x, y).isNegative()
                .and(PredicateType.WUMPUS, x, y)
                .getHead();
        }

        protected override bool precondition(FOLFact fact) {
            return fact.Type == PredicateType.WUMPUS && !fact.hasNext() && !fact.Not;
        }

        public WumpusQuantifier(int width, int height, FOLKnowledgeBase knowledgeBase) : base(width, height, knowledgeBase) { }
    }
    
    /// <summary>
    /// Quantifier 4: Walls don't budge.
    /// ∀ x,y OBSTACLE(x,y) => -MOVABLE(x,y)
    /// </summary>
    public class ObstacleQuantifier : FOLRule {
        protected override FOLFact eval(int x, int y) {
            return new FOLFact(PredicateType.MOVEABLE, x, y, width, height, knowledgeBase).isNegative()
                .and(PredicateType.PIT, x, y).isNegative()
                .and(PredicateType.GOLD, x, y).isNegative()
                .and(PredicateType.OBSTACLE, x, y).isNegative()
                .getHead();
        }

        protected override bool precondition(FOLFact fact) {
            return fact.Type == PredicateType.OBSTACLE && !fact.hasNext() && !fact.Not;
        }

        public ObstacleQuantifier(int width, int height, FOLKnowledgeBase knowledgeBase) : base(width, height, knowledgeBase) { }
    }
    
    /// <summary>
    /// Quantifier 5: X marks the spot.
    /// ∀ x,y GOLD(x,y) => SAFE(x,y) ∧ GLITTER(x+=-1, y+=-1) ∧ -OTHER(x, y)
    /// </summary>
    public class GoldQuantifier : FOLRule {
        protected override FOLFact eval(int x, int y) {
            return new FOLFact(PredicateType.SAFE, x, y, width, height, knowledgeBase)
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

        protected override bool precondition(FOLFact fact) {
            return fact.Type == PredicateType.GOLD && !fact.hasNext() && !fact.Not;
        }

        public GoldQuantifier(int width, int height, FOLKnowledgeBase knowledgeBase) : base(width, height, knowledgeBase) { }
    }

    /// <summary>
    /// Quantifier 6: Where there is a smell, there is a Wumpus.
    /// ∀ x,y SMELL(x,y) => WUMPUS(x+-1, y+-1) v 
    /// </summary>
    public class StenchQuantifier : FOLRule {
        protected override FOLFact eval(int x, int y) {
            return new FOLFact(PredicateType.WUMPUS, x + 1, y, width, height, knowledgeBase)
                .or(PredicateType.WUMPUS, x - 1, y)
                .or(PredicateType.WUMPUS, x, y + 1)
                .or(PredicateType.WUMPUS, x, y - 1)
                .getHead();
        }

        protected override bool precondition(FOLFact fact) {
            return fact.Type == PredicateType.SMELL && !fact.hasNext() && !fact.Not;
        }

        public StenchQuantifier(int width, int height, FOLKnowledgeBase knowledgeBase) : base(width, height, knowledgeBase) { }
    }

    /// <summary>
    /// Quantifier 7: Keep your eyes on the prize.
    /// ∀ x,y GLITTER(x,y) => GOLD(x+-1, y+-1) v 
    /// </summary>
    public class GlitterQuantifier : FOLRule {
        protected override FOLFact eval(int x, int y) {
            return new FOLFact(PredicateType.GOLD, x + 1, y, width, height, knowledgeBase)
                .or(PredicateType.GOLD, x - 1, y)
                .or(PredicateType.GOLD, x, y + 1)
                .or(PredicateType.GOLD, x, y - 1)
                .getHead();
        }

        protected override bool precondition(FOLFact fact) {
            return fact.Type == PredicateType.GLITTER && !fact.hasNext() && !fact.Not;
        }

        public GlitterQuantifier(int width, int height, FOLKnowledgeBase knowledgeBase) : base(width, height, knowledgeBase) { }
    }

    /// <summary>
    /// Quantifier 8: Safety is where the scary things aren't.
    /// ∀ x,y GLITTER(x,y) => GOLD(x+-1, y+-1) v 
    /// </summary>
    public class SafeQuantifier : FOLRule {
        protected override FOLFact eval(int x, int y) {
            return new FOLFact(PredicateType.GOLD, x, y, width, height, knowledgeBase).isNegative()
               .and(PredicateType.WUMPUS, x, y).isNegative()
               .and(PredicateType.OBSTACLE, x, y).isNegative()
               .and(PredicateType.PIT, x, y).isNegative()
               .getHead();
        }

        protected override bool precondition(FOLFact fact) {
            return fact.Type == PredicateType.SAFE && !fact.hasNext() && !fact.Not;
        }

        public SafeQuantifier(int width, int height, FOLKnowledgeBase knowledgeBase) : base(width, height, knowledgeBase) { }
    }
    
    /// <summary>
    /// Quantifier 9: Hear no Evil, See no Evil, Say no Evil
    /// ∀ x,y EMPTY(x,y) => -PIT(x+-1, y+-1) v -WUMPUS(x+-1, y+-1) v -GOLD(x+-1, y+-1) v SAFE(x+=-1, y+=-1)
    /// </summary>
    public class EmptyQualifier : FOLRule {
        protected override FOLFact eval(int x, int y) {
            return new FOLFact(PredicateType.SAFE, x + 1, y, width, height, knowledgeBase)
               .and(PredicateType.SAFE, x - 1, y)
               .and(PredicateType.SAFE, x, y + 1)
               .and(PredicateType.SAFE, x, y - 1)
               .and(PredicateType.GOLD, x + 1, y).isNegative()
               .and(PredicateType.WUMPUS, x + 1, y).isNegative()
               .and(PredicateType.PIT, x + 1, y).isNegative()
               .and(PredicateType.GOLD, x-1, y).isNegative()
               .and(PredicateType.WUMPUS, x-1, y).isNegative()
               .and(PredicateType.PIT, x-1, y).isNegative()
               .and(PredicateType.GOLD, x, y+1).isNegative()
               .and(PredicateType.WUMPUS, x, y+1).isNegative()
               .and(PredicateType.PIT, x, y+1).isNegative()
               .and(PredicateType.GOLD, x, y-1).isNegative()
               .and(PredicateType.WUMPUS, x, y-1).isNegative()
               .and(PredicateType.PIT, x, y-1).isNegative()
               .getHead();
        }

        protected override bool precondition(FOLFact fact) {
            return fact.Type == PredicateType.EMPTY && !fact.hasNext() && !fact.Not;
        }

        public EmptyQualifier(int width, int height, FOLKnowledgeBase knowledgeBase) : base(width, height, knowledgeBase) { }
    }
}
