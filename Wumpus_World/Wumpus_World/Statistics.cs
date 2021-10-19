using System;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;
using System.Xml.Schema;

namespace Wumpus_World {
    public class Statistics
    {
        // Important wumpus world Statistics + Getters + Incrementers
        private int goldFound = 0;
        public int GetGoldFound => goldFound;
        public void incGoldFound() => goldFound++;

        private int wumpusKilled = 0;
        public int GetWumpusKilled => wumpusKilled;
        public void incWumpusKilled() => wumpusKilled++;
        
        private int pitFalls = 0;
        public int GetPitFalls => pitFalls;

        public void incPitFalls() => pitFalls++;

        private int wumpusFalls = 0;
        public int GetWumpusFalls => wumpusFalls;
        public void incWumpusFalls() => wumpusFalls++;
        
        private int cellsExplored = 0;
        public int GetCellsExplored => cellsExplored;
        public void incCellsExplored() => cellsExplored++;

        private int score = 0;
        public int GetScore => score;
        public void incScore() => score++;

        public override String ToString() {
            String o = "";
            o += "Gold Found: " + GetGoldFound + "\n";
            o += "Wumpus Killed: " + GetWumpusKilled + "\n";
            o += "Deaths to Pit: " + GetPitFalls + "\n";
            o += "Deaths to Wumpus: " + GetWumpusFalls + "\n";
            o += "Cells Explored: " + GetCellsExplored + "\n";
            //o += "Agent Score " + getScore + "\n";
            return o;
        }

        // Clear the Statistics each agent navigation cycle
        public void ClearStats() {
            goldFound = 0;
            wumpusKilled = 0;
            pitFalls = 0;
            wumpusFalls = 0;
            cellsExplored = 0;
        }

        // Used to copy the statistic and add to a list
        public Statistics Copy() {
            return new Statistics() {
                cellsExplored = GetCellsExplored, goldFound = GetGoldFound,
                pitFalls = GetPitFalls, wumpusFalls = GetWumpusFalls,
                wumpusKilled = GetWumpusKilled,
                score = GetScore
            };
        }
    }
}