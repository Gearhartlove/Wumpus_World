using System;
using System.Collections.Generic;
using System.Linq;

namespace Wumpus_World {
    public class FOLAgent : Agent {

        private FOLKnowledgeBase knowledgeBase;

        public override void Navigate(Board board) {
            base.Navigate(board);

            List<CellLocation> placesCanGo = new List<CellLocation>();
            
            
            knowledgeBase = new FOLKnowledgeBase(board.GetSize, board.GetSize);
            placesCanGo.Add(new CellLocation(knowledgeBase, currentX, currentY));
            
            while (AliveCheck()) {
                if(GoldCheck()) break;

                foreach (var cellLocation in placesCanGo) {
                    cellLocation.calcScore(currentX, currentY);
                }
                
                if(placesCanGo.Count > 1) 
                placesCanGo.Sort(delegate(CellLocation location1, CellLocation location2) {
                    if (location1.Score > location2.Score) {
                        return 1;
                    }

                    if (location1.Score == location2.Score) {
                        return 0;
                    }

                    return -1;
                });

                var target = placesCanGo.First();
                placesCanGo.RemoveAt(0);

                var modifiers = board.GetModifiers(getCell(board));
                
                if(modifiers.isBreeze) knowledgeBase.addPercept(PredicateType.BREEZE, currentX, currentY);
                if(modifiers.isGlitter) knowledgeBase.addPercept(PredicateType.GLITTER, currentX, currentY);
                if(modifiers.isSmell) knowledgeBase.addPercept(PredicateType.SMELL, currentX, currentX);
                if(!modifiers.isBreeze && !modifiers.isGlitter && !modifiers.isSmell)
                    knowledgeBase.addPercept(PredicateType.EMPTY, currentX, currentY);
            }
        }
    }

    class CellLocation {
        const int POINTS_FOR_WUMPUS = -200;
        const int POINTS_FOR_PIT = -200;
        const int POINTS_FOR_SAFE = 200;

        private FOLKnowledgeBase knowledgeBase;
        private int x, y, score;

        public CellLocation(FOLKnowledgeBase knowledgeBase, int x, int y) {
            this.knowledgeBase = knowledgeBase;
            this.x = x;
            this.y = y;
        }

        public void calcScore(int currentX, int currentY) {
            score = 0;
            var safe = knowledgeBase.queryFact(PredicateType.SAFE, x, y);
            var movable = knowledgeBase.queryFact(PredicateType.MOVEABLE, x, y);
            var gold = knowledgeBase.queryFact(PredicateType.GOLD, x, y);
            var wumpus = knowledgeBase.queryFact(PredicateType.WUMPUS, x, y);
            var pit = knowledgeBase.queryFact(PredicateType.PIT, x, y);
            var distance = Math.Abs(currentX - x) + Math.Abs(currentY - y);

            if (gold == KnowledgeQuery.TRUE) {
                score = Int32.MaxValue;
                return;
            }

            if (movable == KnowledgeQuery.TRUE) {
                score = Int32.MinValue;
                return;
            }

            if (pit == KnowledgeQuery.TRUE) score += POINTS_FOR_PIT;
            if (wumpus == KnowledgeQuery.TRUE) score += POINTS_FOR_WUMPUS;
            if (safe == KnowledgeQuery.TRUE) score += POINTS_FOR_SAFE;
            score -= distance;
        }

        public int Score => score;
    }
}