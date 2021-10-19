using System;
using System.Collections.Generic;

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
            Statistics aveStats = new Statistics();
            if (agent == "foa") {
                aveStats = CalculateAverage(raStats);
            }
            if (agent == "ra") {
                aveStats = CalculateAverage(foaStats);
            }
            PrintAverage(agent, aveStats, boardSize * 5);
        }
        
        // calculate average
        private Statistics CalculateAverage(List<List<Statistics>> stats2D) {
            Statistics aveStats = new Statistics();
            // Sum the totals
            for (int i = 0; i < stats2D.Count; i++) {
                for (int j = 0; j < stats2D[i].Count; j++) {
                    totalGoldFound += stats2D[i][j].GetGoldFound;
                    totalPitFalls += stats2D[i][j].GetPitFalls;
                    totalWumpusFalls += stats2D[i][j].GetWumpusFalls;
                    totalWumpusKilled += stats2D[i][j].GetWumpusKilled;
                    totalCellsExplored += stats2D[i][j].GetCellsExplored;
                }
            }

            totalGoldFound /= iterations;
            totalPitFalls /= iterations;
            totalWumpusFalls/= iterations;
            totalWumpusKilled/= iterations;
            totalCellsExplored/= iterations;
            //totalScore /= iterations;
            
            // Divide by 10
            return aveStats;
        }
        
        // print stats
        public void PrintAverage(String agent, Statistics ave, int bs) {
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