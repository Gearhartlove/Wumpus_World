using System;
using System.Collections.Generic;

namespace Wumpus_World {
    public class Board {
        private Cell[,] board;
        //indexer to access board cells easier
        public Cell this[int x, int y] => board[x, y];
        //used to place Gold and Spawn cells efficiently without overriding another State 
        private List<Cell> EmptyCells; 
        // default board size if no other size is given
        private int size = 0;
        public int GetSize => size;
        // Board probabilities
        private const double pWumpus = 0.1;
        private const double pObstacle = 0.1;
        private const double pPit = 0.2;
        private const double pEmpty = 0.6;
        // Random generator
        private Random rand;
        private Agent agent;
        
        /// <summary>
        /// Creates a Wumpus board. Uses the probabilities defined above to allocate board states and modifiers.
        /// </summary>
        /// <param name="size"></param>
        public Board(int size, Agent agent) {
            this.size = size;
            this.agent = agent;
            EmptyCells = new List<Cell>();
            rand = new Random();
            board = new Cell[size, size];
            //instantiate cells in array
            for (var i = 0; i < size; i++) {
                for (var j = 0; j < size; j++) {
                    board[i, j] = new Cell(i,j);
                }
            }
            // Exit Program if probability not equal to 1
            PCheck(); 
            // Create the board
            // Place Wumpus, Pits, and Walls on the board
            GenStates();
            // Place Gold and Player Spawn on the board. If no room for both, regenerate the map seed. 
            GenGold();
            GenSpawn();
        }

        /// <summary>
        /// Check to make sure spawning probability adds up to 1; (pie chart out of 100%) 
        /// </summary>
        private void PCheck() {
            if (pWumpus + pObstacle + pPit + pEmpty != 1) {
                Console.WriteLine("Board probability not equal to 1, EXIT the Program.");
                System.Environment.Exit(1); 
            }
        }

        /// <summary>
        /// Generate States for the board (wumpus, pit, empty, obstacle)
        /// </summary>
        private void GenStates() {
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    // Pie Chart design, assigns obstacle to cell based on constant probabilities above
                    double r = rand.NextDouble();
                    if (r < pWumpus) {
                        board[i, j].S = State.Wumpus;
                    }

                    if (r > pWumpus && r < pWumpus + pObstacle) {
                        board[i, j].S = State.Obstacle;
                    }

                    if (r > pWumpus + pObstacle && r < pWumpus + pObstacle + pPit) {
                        board[i, j].S = State.Pit;
                    }

                    if (r > pWumpus + pObstacle + pPit && r < pWumpus + pObstacle + pPit + pEmpty) {
                        board[i, j].S = State.Empty;
                        EmptyCells.Add(board[i,j]);
                    }
                }
            }
        }

        /// <summary>
        /// Gen Gold from board's empty cells. Remove empty cell assigned from empty cell list.
        /// </summary>
        private void GenGold() {
            int n = rand.Next(EmptyCells.Count);
            EmptyCells[n].S = State.Gold;
            EmptyCells.RemoveAt(n);
        } 
        
        /// <summary>
        /// Generate spawn from board's empty cells. 
        /// </summary>
        private void GenSpawn() {
           int n = rand.Next(EmptyCells.Count);
           EmptyCells[n].S = State.Spawn;
           agent.SetPos(board[EmptyCells[n].getX, EmptyCells[n].getY]);
        }

        /// <summary>
        /// Update the board's cells by checking for current state. If a wumpus, pit, or gold, update the up to
        /// four possible surrounding cells. 
        /// </summary>
        public Modifier GetModifiers(Cell c) {
            return new Modifier(c, this);
        }
        
        // Create a GUI for the board state, output it to a file
        /// <summary>
        /// Create a GUI for the board, returns o, which is the board. Agent is represented by the square.
        /// W -> Wumpus
        /// P -> Pit
        /// O -> Obstacle
        /// E -> Empty
        /// S -> Spawn
        /// G -> Gold
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            string o = "";
            int i = 0;
            
            for (int y = 0; y < size; y++) {
                for (int x = 0; x < size; x++) {
                    o += WriteCell(board[x, y]); 
                    o += "  "; 
                }
                o += "\n";
            }
            return o;
        }
        
        /// <summary>
        /// Helper Method to ToString() above. Switches on the state of the cells.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private String WriteCell(Cell c) {
            if (c == agent.GetPos()) return "\u2588";
            else {
                switch (c.GetState()) {
                    case State.Empty:
                        return "E";
                    case State.Gold:
                        return "G";
                    case State.Pit:
                        return "P";
                    case State.Obstacle:
                        return "O";
                    case State.Wumpus:
                        return "W";
                    case State.Spawn:
                        return "S";
                }

            } 
            return "Impossible State";
        }
    }
}