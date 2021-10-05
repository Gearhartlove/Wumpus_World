using System;
using System.Reflection.PortableExecutable;

namespace Wumpus_World {
    public class Board {
        private Cell[,] board;
        private int size = 0; // default board size if no other size is given
        // Board probabilities
        private const double pWumpus = 0.2;
        private const double pWall = 0.2;
        private const double pPit = 0.2;
        private const double pEmpty = 0.4;
        // Random generator
        private Random rand;
        
        
        /// <summary>
        /// Creates a Wumpus board. Uses the probabilities defined above to allocate board states and modifiers.
        /// </summary>
        /// <param name="size"></param>
        public Board(int size) {
            this.size = size;
            rand = new Random();
            board = new Cell[size, size];
            //instantiate cells in array
            for (var i = 0; i < size; i++) {
                for (var j = 0; j < size; j++) {
                    board[i, j] = new Cell();
                }
            }
            // Exit Program if probability not equal to 1
            PCheck(); 
            // Create the board
            // Place Wumpus, Pits, and Walls on the board
            GenObsticles();
            // Place Gold and Player Spawn on the board. If no room for both, regenerate the map seed. 
            GenGold();
            GenSpawn();
            //AssignModiifiers();
        }

        private void PCheck() {
            if (pWumpus + pWall + pPit + pEmpty != 1)
            {
                Console.WriteLine("Board probability not equal to 1, EXIT the Program.");
                System.Environment.Exit(1); 
            }
        }

        private void GenObsticles() {
            for (int i = 0; i < size; i++) 
            {
                for (int j = 0; j < size; j++)
                {
                    // Pie Chart design, assigns obstacle to cell based on constant probabilities above
                    double r = rand.NextDouble();
                    if (r < pWumpus) {
                        board[i, j].Obs = Obstacle.Wumpus;
                    }

                    if (r > pWumpus && r < pWumpus + pWall) {
                        board[i, j].Obs = Obstacle.Wall;
                    }

                    if (r > pWumpus + pWall && r < pWumpus + pWall + pPit) {
                        board[i, j].Obs = Obstacle.Pit;
                    }

                    if (r > pWumpus + pWall + pPit && r < pWumpus + pWall + pPit + pEmpty) {
                        board[i, j].Obs = Obstacle.Empty;
                    }
                }
            }
        }

        private void GenGold() {
        // Place gold on an empty square, recurse till it does that
        } 
        
        private void GenSpawn() {
           // Place spawn on an empty square, recurse till it does that 
        }
        
        //TODO ToString
        
        //Do we want to have the running the board functionality in this class???
    }
}