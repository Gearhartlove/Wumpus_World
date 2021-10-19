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
        private double totalScore = 0;
        
        // reward function , actions are -1 to score
        private int pWKill = 0;
        private int pGold = 100;

        /// <summary>
        /// Statsmanager constructor.
        /// </summary>
        /// <param name="i"></param>
        public StatsManager(int i) {
            iterations = i;
        }

        /// <summary>
        /// Calculates and prints the averages of the statistics for each board.
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="boardSize"></param>
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

        /// <summary>
        /// Calculates the averaages for each statistic divided by the number of boards at each size.
        /// </summary>
        /// <param name="stats2D"></param>
        /// <param name="bs"></param>
        private void CalculateAverage(List<Statistics> stats2D, int bs) {
            // Sum the totals
            for (int j = 0; j < iterations; j++) {
                totalGoldFound += stats2D[j].GetGoldFound;
                totalPitFalls += stats2D[j].GetPitFalls;
                totalWumpusFalls += stats2D[j].GetWumpusFalls;
                totalWumpusKilled += stats2D[j].GetWumpusKilled;
                totalCellsExplored += stats2D[j].GetCellsExplored;
                // calculate score
                totalScore -= stats2D[j].GetScore;
                totalScore += (totalGoldFound * pGold);
                totalScore += (totalWumpusKilled * pWKill);
            }

            // divide by number of board per board size
            totalGoldFound /= iterations;
            totalPitFalls /= iterations;
            totalWumpusFalls /= iterations;
            totalWumpusKilled /= iterations;
            totalCellsExplored /= iterations;
            totalScore /= iterations;
        }

        /// <summary>
        /// Print the averages of the stats.
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="bs"></param>
        public void PrintAverage(String agent, int bs) {
            String o = "";
            if (agent == "foa") o += "First Order Logic Agent " + bs + "x" + bs + " Averages:\n";
            if (agent == "ra") o += "Reflexive Agent " + bs + "x" + bs + " Averages:\n";
            o += "Gold Found: " + totalGoldFound + "\n";
            o += "Wumpus Killed: " + totalWumpusKilled + "\n";
            o += "Deaths to Pit: " + totalPitFalls + "\n";
            o += "Deaths to Wumpus: " + totalWumpusFalls + "\n";
            o += "Cells Explored: " + totalCellsExplored + "\n";
            o += "Score: " + totalScore;
            Console.WriteLine(o);
            Console.WriteLine();
            ClearAverages(); // Clear averages, prepare for next iteration(s)
        }

        /// <summary>
        /// Clear the averages; use case: after every board size category
        /// </summary>
        private void ClearAverages() {
            totalGoldFound = 0;
            totalWumpusFalls = 0;
            totalPitFalls = 0;
            totalWumpusFalls = 0;
            totalCellsExplored = 0;
            totalScore = 0;
        }
    }
}