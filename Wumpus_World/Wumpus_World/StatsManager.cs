using System;
using System.Collections.Generic;
using System.Linq;

namespace Wumpus_World {
    public class StatsManager {
        public List<List<Statistics>> raStats = new List<List<Statistics>>();
        public List<List<Statistics>> foaStats = new List<List<Statistics>>();
        private double iterations;
        private double totalGoldFound = 0;
        private double totalWumpusKilled = 0;
        private double totalPitFalls = 0;
        private double totalWumpusFalls = 0;
        private double totalCellsExplored = 0;

        public StatsManager(int i) {
            iterations = i;
        }

        public void Averages(String agent, int boardSize) {
            if (agent == "foa") {
                List<Statistics> s1 = foaStats[boardSize - 1]; 
                CalculateAverage(s1, boardSize - 1);
            }

            if (agent == "ra") {
                List<Statistics> s1 = raStats[boardSize - 1]; 
                CalculateAverage(s1, boardSize - 1);
            }

            PrintAverage(agent, boardSize * 5);
        }

        // calculate average
        private void CalculateAverage(List<Statistics> stats2D, int bs) {
            // Sum the totals
            for (int j = 0; j < iterations; j++) {
                totalGoldFound += stats2D[j].GetGoldFound;
                totalPitFalls += stats2D[j].GetPitFalls;
                totalWumpusFalls += stats2D[j].GetWumpusFalls;
                totalWumpusKilled += stats2D[j].GetWumpusKilled;
                totalCellsExplored += stats2D[j].GetCellsExplored;
            }

            // divide by number of board per board size
            totalGoldFound /= iterations;
            totalPitFalls /= iterations;
            totalWumpusFalls /= iterations;
            totalWumpusKilled /= iterations;
            totalCellsExplored /= iterations;
            //totalScore /= iterations;
        }

    // print stats
        public void PrintAverage(String agent, int bs) {
            String o = "";
            if (agent == "foa") o += "First Order Logic Agent " + bs + "x" + bs + " Averages:\n";
            if (agent == "ra") o += "Reflexive Agent " + bs + "x" + bs + " Averages:\n";
            o += "Gold Found: " + totalGoldFound + "\n";
            o += "Wumpus Killed: " + totalWumpusKilled + "\n";
            o += "Deaths to Pit: " + totalPitFalls + "\n";
            o += "Deaths to Wumpus: " + totalWumpusFalls + "\n";
            o += "Cells Explored: " + totalCellsExplored + "\n";
            //o += "Agent Score " + getScore + "\n";
            // TODO Need to have the score in here
            Console.WriteLine(o);
            ClearAverages(); // Clear averages, prepare for next iteration(s)
        }

        private void ClearAverages() {
            totalGoldFound = 0;
            totalWumpusFalls = 0;
            totalPitFalls = 0;
            totalWumpusFalls = 0;
            totalCellsExplored = 0;
        }
    }
}