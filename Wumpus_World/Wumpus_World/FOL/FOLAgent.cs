using System;
using System.Collections.Generic;
using System.Linq;

namespace Wumpus_World {
    public class FOLAgent : Agent {

        private FOLKnowledgeBase knowledgeBase;

        public override void Navigate(Board board) {
            board.SetAgent(this);
            Console.WriteLine(board);

            knowledgeBase = new FOLKnowledgeBase(board.GetSize, board.GetSize);
            
            List<CellLocation> placesToGo = new List<CellLocation>();
            placesToGo.Add(new CellLocation(knowledgeBase, currentX, currentY));
            
            while (AliveCheck()) {
                if(GoldCheck()) break;

                //Perceve and make decision
                var modifiers = board.GetModifiers(getCell(board));
                
                if(modifiers.isBreeze) knowledgeBase.addPercept(PredicateType.BREEZE, currentX, currentY);
                if(modifiers.isGlitter) knowledgeBase.addPercept(PredicateType.GLITTER, currentX, currentY);
                if(modifiers.isSmell) knowledgeBase.addPercept(PredicateType.SMELL, currentX, currentX);
                if(!modifiers.isBreeze && !modifiers.isGlitter && !modifiers.isSmell)
                    knowledgeBase.addPercept(PredicateType.EMPTY, currentX, currentY);

                knowledgeBase.infer();
                Console.WriteLine(knowledgeBase.simpleFactsString());
                Console.WriteLine(knowledgeBase.complexFactsString());

                placesToGo.RemoveAll(i => !i.calcScore(currentX, currentY));
                
                if(placesToGo.Count > 1) 
                placesToGo.Sort(delegate(CellLocation location1, CellLocation location2) {
                    if (location1.Score > location2.Score) {
                        return -1;
                    }

                    if (location1.Score == location2.Score) {
                        return 0;
                    }

                    return 1;
                });
                
                Console.WriteLine("Oder of cells: " + string.Join(", ", placesToGo));

                var target = placesToGo.First();
                placesToGo.RemoveAt(0);
                
                Console.WriteLine(target);
                
                TravelPath(board[target.X, target.Y]);
                
                if(currentX + 1 >= 0 && currentX + 1 < board.GetSize && currentY >= 0 && currentY < board.GetSize && !QueryVisited(board[currentX + 1, currentY])) 
                    placesToGo.Add(new CellLocation(knowledgeBase, currentX + 1, currentY));
                if(currentX - 1 >= 0 && currentX - 1 < board.GetSize && currentY >= 0 && currentY < board.GetSize && !QueryVisited(board[currentX - 1, currentY])) 
                    placesToGo.Add(new CellLocation(knowledgeBase, currentX - 1, currentY)); 
                if(currentX >= 0 && currentX < board.GetSize && currentY + 1 >= 0 && currentY + 1 < board.GetSize && !QueryVisited(board[currentX, currentY + 1])) 
                    placesToGo.Add(new CellLocation(knowledgeBase, currentX, currentY + 1)); 
                if(currentX >= 0 && currentX < board.GetSize && currentY - 1 >= 0 && currentY - 1 < board.GetSize && !QueryVisited(board[currentX, currentY - 1])) 
                    placesToGo.Add(new CellLocation(knowledgeBase, currentX, currentY - 1)); 
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

        public bool calcScore(int currentX, int currentY) {
            score = 0;
            var safe = knowledgeBase.queryFact(PredicateType.SAFE, x, y);
            var movable = knowledgeBase.queryFact(PredicateType.MOVEABLE, x, y);
            var gold = knowledgeBase.queryFact(PredicateType.GOLD, x, y);
            var wumpus = knowledgeBase.queryFact(PredicateType.WUMPUS, x, y);
            var pit = knowledgeBase.queryFact(PredicateType.PIT, x, y);
            var distance = Math.Abs(currentX - x) + Math.Abs(currentY - y);

            if (gold == KnowledgeQuery.TRUE) {
                score = Int32.MaxValue;
                return true;
            }

            if (movable == KnowledgeQuery.TRUE) {
                score = Int32.MinValue;
                return false;
            }

            if (pit == KnowledgeQuery.TRUE) score += POINTS_FOR_PIT;
            if (wumpus == KnowledgeQuery.TRUE) score += POINTS_FOR_WUMPUS;
            if (safe == KnowledgeQuery.TRUE) score += POINTS_FOR_SAFE;
            score -= distance;

            return true;
        }

        public int Score => score;

        public int X => x;

        public int Y => y;

        public override string ToString() {
            return "CellTarget[" + score +"](" + x + ", " + y + ")";
        }
    }
}