using System;
using System.Collections.Generic;
using System.Linq;

namespace Wumpus_World {
    public class FOLAgent : Agent {

        List<CellLocation> placesToGo = new List<CellLocation>();
        protected FOLKnowledgeBase knowledgeBase;
        
        private Random rng = new Random();
        
        public override void Navigate(Board board) {
            base.Navigate(board);

            knowledgeBase = new FOLKnowledgeBase(board.GetSize, board.GetSize);
            routeAdjacent(board);
            
            while (AliveCheck()) {
                if(GoldCheck()) break;

                //Perceve and make decision
                var modifiers = board.GetModifiers(getCell(board));
                
                knowledgeBase.addPercept(PredicateType.SAFE, currentX, currentY);
                
                if(modifiers.isBreeze) knowledgeBase.addPercept(PredicateType.BREEZE, currentX, currentY);
                if(modifiers.isGlitter) knowledgeBase.addPercept(PredicateType.GLITTER, currentX, currentY);
                if(modifiers.isSmell) knowledgeBase.addPercept(PredicateType.SMELL, currentX, currentX);
                if(!modifiers.isBreeze && !modifiers.isGlitter && !modifiers.isSmell)
                    knowledgeBase.addPercept(PredicateType.EMPTY, currentX, currentY);

                knowledgeBase.infer();

                placesToGo.RemoveAll(i => !i.calcScore(currentX, currentY));

                if (placesToGo.Count > 1) {
                    shuffle(ref placesToGo);
                    placesToGo.Sort(delegate(CellLocation location1, CellLocation location2) {
                        if (location1.Score > location2.Score) {
                            return -1;
                        }

                        if (location1.Score == location2.Score) {
                            return 0;
                        }

                        return 1;
                    });

                }

                if (placesToGo.Count == 0) {
                    isDead = true;
                    break;
                }
                var target = placesToGo.First();
                
                placesToGo.RemoveAt(0);
                
                TravelPath(board[target.X, target.Y]);

                if (currentX == prevX && currentY == prevY) {
                    knowledgeBase.addPercept(PredicateType.OBSTACLE, target.X, target.Y);
                } else {
                    routeAdjacent(board);
                }
            }
        }

        private void routeAdjacent(Board board) {
            if(currentX + 1 >= 0 && currentX + 1 < board.GetSize && currentY >= 0 && currentY < board.GetSize && !QueryVisited(board[currentX + 1, currentY])) 
                addPlace(new CellLocation(knowledgeBase, currentX + 1, currentY));
            if(currentX - 1 >= 0 && currentX - 1 < board.GetSize && currentY >= 0 && currentY < board.GetSize && !QueryVisited(board[currentX - 1, currentY])) 
                addPlace(new CellLocation(knowledgeBase, currentX - 1, currentY)); 
            if(currentX >= 0 && currentX < board.GetSize && currentY + 1 >= 0 && currentY + 1 < board.GetSize && !QueryVisited(board[currentX, currentY + 1])) 
                addPlace(new CellLocation(knowledgeBase, currentX, currentY + 1)); 
            if(currentX >= 0 && currentX < board.GetSize && currentY - 1 >= 0 && currentY - 1 < board.GetSize && !QueryVisited(board[currentX, currentY - 1])) 
                addPlace(new CellLocation(knowledgeBase, currentX, currentY - 1));
        }

        private void addPlace(CellLocation location) {
            if(!placesToGo.Contains(location)) placesToGo.Add(location); 
        }

        private void shuffle(ref List<CellLocation> list) {  
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                var value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }
    }

    class CellLocation {
        const int POINTS_FOR_WUMPUS = -1000;
        const int POINTS_FOR_PIT = -1000;
        const int POINTS_FOR_SAFE = 200;
        const int POINTS_FOR_SUSPECT_GOLD = 300;
        const int POINTS_FOR_SUSPECT_PIT = -300;
        const int POINTS_FOR_SUSPECT_WUMPUS = -300;

        private FOLKnowledgeBase knowledgeBase;
        private int x, y, score;

        public CellLocation(FOLKnowledgeBase knowledgeBase, int x, int y) {
            this.knowledgeBase = knowledgeBase;
            this.x = x;
            this.y = y;
        }

        public bool calcScore(int currentX, int currentY) {
            score = 0;
            var safe = knowledgeBase.queryFact(PredicateType.SAFE, x, y);
            var movable = knowledgeBase.queryFact(PredicateType.MOVEABLE, x, y);
            var gold = knowledgeBase.queryFact(PredicateType.GOLD, x, y);
            var wumpus = knowledgeBase.queryFact(PredicateType.WUMPUS, x, y);
            var pit = knowledgeBase.queryFact(PredicateType.PIT, x, y);
            var distance = Math.Abs(currentX - x) + Math.Abs(currentY - y);
            var suspectGold = knowledgeBase.isSuspected(PredicateType.GOLD, x, y);
            var suspectWumpus = knowledgeBase.isSuspected(PredicateType.WUMPUS, x, y);
            var suspectPit = knowledgeBase.isSuspected(PredicateType.PIT, x, y);

            if (gold == KnowledgeQuery.TRUE) {
                score = Int32.MaxValue;
                return true;
            }

            if (movable == KnowledgeQuery.FALSE) {
                score = Int32.MinValue;
                return false;
            }

            if (pit == KnowledgeQuery.TRUE) score += POINTS_FOR_PIT;
            if (wumpus == KnowledgeQuery.TRUE) score += POINTS_FOR_WUMPUS;
            if (safe == KnowledgeQuery.TRUE) score += POINTS_FOR_SAFE;
            if (suspectGold) score += POINTS_FOR_SUSPECT_GOLD;
            if (suspectWumpus) score += POINTS_FOR_SUSPECT_WUMPUS;
            if (suspectPit) score += POINTS_FOR_SUSPECT_PIT;
            score -= distance;

            return true;
        }
        
        public override bool Equals(object obj) {
            if (obj is CellLocation) {
                CellLocation other = (CellLocation) obj;
                return this.x == other.x && this.y == other.y;
            }
            return base.Equals(obj);
        }

        public int Score => score;

        public int X => x;

        public int Y => y;

        public override string ToString() {
            return "CellTarget[" + score +"](" + x + ", " + y + ")";
        }
    }
}