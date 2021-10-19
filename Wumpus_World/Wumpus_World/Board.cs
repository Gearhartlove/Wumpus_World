using System;
using System.Collections.Generic;
using System.Linq;

namespace Wumpus_World {
    public class Board {
        private Cell[,] board;
        //indexer to access board cells easier
        public Cell this[int x, int y] {
            get {
                //Won't let the agent travel outside of the board
                if (x >= size || y >= size || x < 0 || y < 0) {
                    Console.WriteLine("You cannot travel to cell [" + x + "," + y +"]. " +
                                      "It does not exist. Nothing Happens");
                    return agent.getCell(this); //agent does not move, nothing happens
                }
            
                return board[x,y];
            }
        }
        //used to place Gold and Spawn cells efficiently without overriding another State 
        private List<Cell> EmptyCells; 
        // spawn cell
        private Cell spawn;
        // default board size if no other size is given
        private int size = 0;
        public int GetSize => size;
        // Board probabilities
        private const double pWumpus = 0.1;
        private const double pObstacle = 0.1;
        private const double pPit = 0.1;
        private const double pEmpty = 0.7;
        // Random generator
        private Random rand;
        // Agent must be manually assigned using SetAgent() Method
        private Agent agent;
        // Number of Wumpuses on the board
        private int wumpusCount = 0;
        public int GetWumpusCount => wumpusCount;
        
        /// <summary>
        /// Creates a Wumpus board. Uses the probabilities defined above to allocate board states and modifiers.
        /// </summary>
        /// <param name="size"></param>
        public Board(int size) {
            this.size = size;
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
                        wumpusCount++; // add 1 to total number of wumpuses on the board
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
           spawn = EmptyCells[n]; // set the spawn for the board
           //agent.SetPos(board[EmptyCells[n].getX, EmptyCells[n].getY]);
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
            if (c == agent.getCell(this)) return "\u2588";
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

        /// <summary>
        /// Sets the agent who will navigate the board.
        /// </summary>
        /// <param name="a"></param>
        public void SetAgent(Agent a) {
            agent = a;
            agent.SpawnAgent(spawn); // X and Y are private, which is why the board class calls the SpawnAgent method
                                     // apart of the Agent class. Design can improve.
        }

        /// <summary>
        /// Returns a list of the cells neighboring a cell.
        /// </summary>
        public List<Cell> CellNeighbors(Cell c) {
            List<Cell> returnCells = new List<Cell>();
            int x = c.getX;
            int y = c.getY;
            
            if (y + 1 < this.GetSize) {
                returnCells.Add(board[x, y + 1]);
            } 
            
            //look South
            if (y - 1 > -1 ) {
                returnCells.Add(board[x, y - 1]);
            }

            //look West
            if (x - 1 > -1) {
                returnCells.Add(board[x-1, y]);
            }

            //look East
            if (x + 1 < this.GetSize) {
                returnCells.Add(board[x+1,y]);
            }

            return returnCells;
        }
    }
}