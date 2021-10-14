using System;

namespace Wumpus_World
{
    public class Statistics
    {
        // Stats to be tracked
        int timesActionsUsed = 0;
        int timesGoldFound = 0;
        int timesWumpusKilled = 0;
        int timesWumpusKilledPlayer = 0;
        int timesPlayerDied = 0;
        int timesFellInPit = 0;
        int cellsExplored = 0;

        // Initialize stats
        public Statistics()
        {
            timesActionsUsed = 0;
            timesGoldFound = 0;
            timesWumpusKilled = 0;
            timesWumpusKilledPlayer = 0;
            timesPlayerDied = 0;
            timesFellInPit = 0;
            cellsExplored = 0;
        }

        // Method for resetting stat values
        public void Reset()
        {
            timesActionsUsed = 0;
            timesGoldFound = 0;
            timesWumpusKilled = 0;
            timesWumpusKilledPlayer = 0;
            timesPlayerDied = 0;
            timesFellInPit = 0;
            cellsExplored = 0;
        }

        ///
        ///
        /// DO A DICTIONARY FOR THIS
        ///
        ///

        // Increments a particular stat,
        // does nothing if no stat is selected
        public void IncrementValue(int _value)
        {
            switch (_value)
            {
                case 0:
                    timesActionsUsed++;
                    break;
                case 1:
                    timesGoldFound++;
                    break;
                case 2:
                    timesWumpusKilled++;
                    break;
                case 3:
                    timesWumpusKilledPlayer++;
                    break;
                case 4:
                    timesPlayerDied++;
                    break;
                case 5:
                    timesFellInPit++;
                    break;
                case 6:
                    cellsExplored++;
                    break;
                default:
                    break;
            }
        }

        // Gets and returns the value of a particular stat,
        // returns -1 if no stat is selected
        public int GetValue(int _value)
        {
            switch (_value)
            {
                case 0:
                    return timesActionsUsed;
                case 1:
                    return timesGoldFound;
                case 2:
                    return timesWumpusKilled;
                case 3:
                    return timesWumpusKilledPlayer;
                case 4:
                    return timesPlayerDied;
                case 5:
                    return timesFellInPit;
                case 6:
                    return cellsExplored;
                default:
                    return -1;
            }
        }

        // Prints stats
        public void PrintStats()
        {
            Console.WriteLine("Actions taken: " + timesActionsUsed + "\nGolds found: "
                + timesGoldFound + "\nWumpus' killed: " + timesWumpusKilled + "\nDeaths to Wumpus: "
                + timesWumpusKilledPlayer + "\nTotal deaths: " + timesPlayerDied + "\nDeaths to pit: "
                + timesFellInPit + "\nCells explored: " + cellsExplored);
        }
    }
}