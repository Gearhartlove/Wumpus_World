using System;
using System.Collections.Generic;

namespace Wumpus_World
{
    public class Statistics
    {
        // Stats to be tracked
        Dictionary<char, int> agentStats = new Dictionary<char, int>()
        {
            {'A', 0},
            {'G', 0},
            {'K', 0},
            {'W', 0},
            {'D', 0},
            {'P', 0},
            {'E', 0}
        };

        /// <summary>
        /// Increases the desired statistic by one
        /// A for actions, G for gold, K for kills, W for Wumpus deaths,
        /// D for total deaths, P for pit deaths, E for cells explored
        /// </summary>
        /// <param name="_key"></param>
        public void IncrementStat(char _key)
        {
            agentStats[_key]++;
        }

        /// <summary>
        /// Gets and returns the desired statistic as an int
        /// </summary>
        /// <param name="_key"></param>
        /// <returns></returns>
        public int GetStat(char _key)
        {
            return agentStats[_key];
        }

        /// <summary>
        /// Resets all values within the stat dictionary
        /// </summary>
        public void Reset()
        {
            agentStats['A'] = 0;
            agentStats['G'] = 0;
            agentStats['K'] = 0;
            agentStats['W'] = 0;
            agentStats['D'] = 0;
            agentStats['P'] = 0;
            agentStats['E'] = 0;
        }

        /// <summary>
        /// Prints all statistics to the console
        /// </summary>
        public void PrintStats()
        {
            Console.WriteLine("Actions taken: " + agentStats['A'] + "\nGolds found: "
                + agentStats['G'] + "\nWumpus' killed: " + agentStats['K'] + "\nDeaths to Wumpus: "
                + agentStats['W'] + "\nTotal deaths: " + agentStats['D'] + "\nDeaths to pit: "
                + agentStats['P'] + "\nCells explored: " + agentStats['E']);
        }
    }
}