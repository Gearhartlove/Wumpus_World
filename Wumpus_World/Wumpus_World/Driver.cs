using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wumpus_World {
    public class Driver {
        // References of agents used throughout the program
        //private FOLAgent foa;
        //private ReflexAgent ra;
        private StatsManager sm;
        private const int iterations = 10;
        
        /// <summary>
        /// Constructor for Driver class. Main brings in the two agents as parameters.
        /// </summary>
        /// <param name="foa"></param>
        /// <param name="ra"></param>
        public Driver(FOLAgent foa, ReflexAgent ra) {
            //this.foa = foa;
            //this.ra = ra;
            sm = new StatsManager(iterations);
        }

        /// <summary>
        /// For Debugging purposes, runs specific agent on 5x5 board.
        /// </summary>
        public void RunTestWumpusWorld(Agent agent) {
            Board board = new Board(7);
            agent.SetBoard(board);
            agent.Navigate(board);
        }
        
        /// <summary>
        /// Run the wumpus world program: generate 10 boards for each size 5x5 to 25x25, stepping by 5 each board.
        /// </summary>
        public void RunWumpusWord() {
            for (int boardSize = 0; boardSize < 5; boardSize++) {
                int size = (boardSize+1) * 5; 
                sm.foaStats.Add(new List<Statistics>());
                sm.raStats.Add(new List<Statistics>());
                
                // Collecting statistics
                for (int boardNum = 0; boardNum < iterations; boardNum++) {
                    Board board = new Board(size);
                    ReflexAgent ra = new ReflexAgent();
                    FOLAgent foa = new FOLAgent();
                    foa.SetBoard(board);
                    ra.SetBoard(board);
                    foa.Navigate(board);
                    ra.Navigate(board);
                    sm.raStats[boardSize].Add( ra.GetStats.Copy());
                    sm.foaStats[boardSize].Add(foa.GetStats);
                }

                sm.Averages("ra", boardSize + 1);
                sm.Averages("foa", boardSize + 1);
            }
        }
    }
}