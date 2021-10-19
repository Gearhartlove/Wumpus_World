using System;
using System.Collections.Generic;
using System.Linq;

namespace Wumpus_World {
    public class Agent {

        private Direction facing = Direction.North;
        protected int currentX, currentY;
        protected int prevX, prevY;
        private int arrowX, arrowY;
        private int arrowCount;
        protected int GetArrowCount => arrowCount;
        protected Statistics statistics = new Statistics();
        public List<Statistics> statsList = new List<Statistics>();
        private int score = 0;
        private Board board;
        protected bool isDead;
        private Dictionary<Tuple<int,int>, bool> cellsVisited = new Dictionary<Tuple<int, int>, bool>();

        /// <summary>
        /// Spawn the agent on the board.
        /// </summary>
        /// <param name="spawn"></param>
        public void SpawnAgent(Cell spawn) {
            currentX = spawn.getX;
            currentY = spawn.getY;
            
            UpdateVisited();
        } 
        
        /// <summary>
        /// Get specific cell from the board.
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public Cell getCell(Board board) {
            return board[currentX, currentY];
        }

        /// <summary>
        /// Assign the board to the agent.
        /// </summary>
        /// <param name="board"></param>
        public void SetBoard(Board board) {
            this.board = board;
            arrowCount = board.GetWumpusCount;
        }

        /// <summary>
        /// Agent Navigating through Wumpus' World. This includes moving, recording statistics, as well as additional
        /// logic, depending on the type of agent. Will be overriden by children.
        /// </summary>
        /// <param name="board"></param>
        public virtual void Navigate(Board board) {
            statsList.Add(statistics);
            statistics = new Statistics();
            //stats.PrintStats(board);
            //stats = new Statistics();
            //AppendStatsList(stats);
            board.SetAgent(this); // NEEDS TO BE INCLUDED FOR BOARD TO KNOW WHERE THE AGENT IS AND PRINT CORRECTLY.
            // Put navigating logic below
        }

        /// <summary>
        /// Agent shoots in the direction they are facing. If it hits a Wumpus, award points (100);
        /// </summary>
        public void Shoot() {
            if (arrowCount > 0) {
                switch (facing) {
                    case Direction.North:
                        while (arrowY < board.GetSize) {
                            arrowY++;
                            if (ObstacleShot()) break;
                            if (WumpusShot()) {
                                // reward function
                                break;
                            }
                        }
                        break;
                    
                    case Direction.South:
                        while (arrowY >= 0) {
                            arrowY--;
                            if (ObstacleShot()) break;
                            if (WumpusShot()) {
                                // reward function
                                break;
                            }
                        }
                        break;
                    
                    case Direction.East:
                        while (arrowX >= 0) {
                            arrowY--;
                            if (ObstacleShot()) break;
                            if (WumpusShot()) {
                                // reward function 
                                break;
                            }
                        }
                        break;
                    
                    case Direction.West:
                        while (arrowX <= board.GetSize) {
                            arrowX++;
                            if (ObstacleShot()) break;
                            if (WumpusShot()) {
                                // reward function
                                break;
                            }
                        }
                        break;
                }

                score++;
                arrowCount--;
                // reset arrow position to agent
                arrowX = currentX;
                arrowY = currentY;
            }
        }

        /// <summary>
        /// Check if wumpus was shot.
        /// </summary>
        /// <returns></returns>
        private bool WumpusShot() {
            if (board[arrowX, arrowY].GetState() == State.Wumpus) {
                statistics.IncrementStat('K');
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if obstacle was shot.
        /// </summary>
        /// <returns></returns>
        private bool ObstacleShot() {
           if (board[arrowX, arrowY].GetState() == State.Obstacle) return true;
           return false;
        }

        /// <summary>
        /// Walk the agent forward, depending on the direction the agent is facing.
        /// </summary>
        /// <returns></returns>
        protected State walkForward() {
            prevX = currentX;
            prevY = currentY;
            
            switch (facing) {
                case Direction.North:
                    currentY++;
                    break;
                case Direction.East:
                    currentX++;
                    break;
                case Direction.South:
                    currentY--;
                    break;
                case Direction.West:
                    currentX--;
                    break;
            }
            UpdateVisited(); // update visited cells
            statistics.IncrementStat('A');
            score++;
            // checks if wall, does not let the agent move to wall cell
            if (ObstacleCheck(board[currentX, currentY].GetState(), prevX, prevY)) {
                return State.Obstacle;
            }
            return board[currentX, currentY].GetState();
        }
            
        /// <summary>
        /// Turn the agent right.
        /// </summary>
        protected void turnRight() {
            switch (facing) {
                case Direction.North:
                    facing = Direction.East;
                    break;
                case Direction.East:
                    facing = Direction.South;
                    break;
                case Direction.South:
                    facing = Direction.West;
                    break;
                case Direction.West:
                    facing = Direction.North;
                    break;
            }
            statistics.IncrementStat('A');
            score++;
        }
        
        /// <summary>
        /// Turn the agent left.
        /// </summary>
        protected void turnLeft() {
            switch (facing) {
                case Direction.North:
                    facing = Direction.West;
                    break;
                case Direction.East:
                    facing = Direction.North;
                    break;
                case Direction.South:
                    facing = Direction.East;
                    break;
                case Direction.West:
                    facing = Direction.South;
                    break;
            }
            statistics.IncrementStat('A');
            score++;
        }
        
        /// Helper method or "macro" to move agent North (turns included)
        public State MoveNorth() {
            switch (facing) {
                case Direction.North:
                    return walkForward();
                case Direction.South:
                    turnRight(); // right(south) => West
                    turnRight(); // right(west) => North
                    return walkForward();
                case Direction.East:
                    turnLeft(); // left(East) => North
                    return walkForward();
                case Direction.West:
                    turnRight(); // right(West) => North
                    return walkForward();
            }
            return State.Empty;
        }
        
        /// Helper method or "macro" to move agent South (turns included)
        public State MoveSouth() {
            switch (facing) {
                case Direction.North:
                    turnRight(); // right(north) => East
                    turnRight(); // right(east) => South
                    return walkForward();
                case Direction.South:
                    return walkForward();
                case Direction.East:
                    turnRight(); // right(east) => South
                    return walkForward();
                case Direction.West:
                    turnLeft();// left(West) => North
                    return walkForward();
            }
            return State.Empty;
        }
        
        /// Helper method or "macro" to move agent West (turns included)
        public State MoveWest() {
            switch (facing) {
                case Direction.North:
                    turnLeft(); // left(North) => West
                    return walkForward();
                case Direction.South:
                    turnRight(); // right(South) => West
                    return walkForward();
                case Direction.East:
                    turnLeft(); // left(East) => North
                    turnLeft(); // left(North) => East
                    return walkForward();
                case Direction.West:
                    return walkForward();
            }
            return State.Empty; // will never happen
        }
        
        // Helper method or "macro" to move agent Eorth (turns included)
        public State MoveEast() {
            switch (facing) {
                case Direction.North:
                    turnRight(); // right(North) => East
                    return walkForward();
                case Direction.South:
                    turnLeft(); // left(South) => East
                    return walkForward();
                case Direction.East:
                    return walkForward();
                case Direction.West:
                    turnRight(); // right(West) => North
                    turnRight(); // right(North) => East
                    return walkForward();
            }
            return State.Empty; // will never happen
        }

        /// <summary>
        /// If I walked into an obstacle, to the previous cell.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="prevX"></param>
        /// <param name="prevY"></param>
        public bool ObstacleCheck(State state, int prevX, int prevY) {
            if (state == State.Obstacle) {
                currentX = prevX;
                currentY = prevY;
                return true; // yes their was a wall, bounce off
            }
            return false; // no wall in cell 
        }
        
        /// <summary>
        /// Check if the agent is alive. Return true if alive, return false if dead.
        /// </summary>
        /// <returns></returns>
        public bool AliveCheck() {
            switch (board[currentX, currentY].GetState()) {
                case State.Wumpus:
                    statistics.IncrementStat('W');
                    statistics.IncrementStat('D');
                    return false;
                case State.Pit:
                    statistics.IncrementStat('P');
                    statistics.IncrementStat('D');
                    return false;
            }
            return !isDead;
        }

        public bool GoldCheck() {
            if (board[currentX, currentY].GetState() == State.Gold) {
                statistics.IncrementStat('G');
                return true;
            }
            return false;
        }
        /// <summary>
        /// Updated cells visited list.
        /// </summary>
        private void UpdateVisited() {
            if (!cellsVisited.ContainsKey(new Tuple<int, int>(currentX, currentY))) {
                cellsVisited.Add(new Tuple<int, int>(currentX, currentY), true);
                statistics.IncrementStat('E');
            }
        }

        /// <summary>
        /// If the cell has been visited return true, else return false
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool QueryVisited(Cell cell) {
            if (!cellsVisited.ContainsKey(new Tuple<int, int>(cell.getX, cell.getY))) return false;
            return true;
        }

        /// <summary>
        /// Greedy algorithm to find the shortest path to the desired cell.
        /// Trying to travel to cells which are closer to the agent's current cell
        /// from the goalCell, using only cells which the agent has traveled.
        /// Greed on the slopes between current cell and agent's current cell.
        /// </summary>
        /// <param name="goalCell"></param>
        /// <returns></returns>
        public void TravelPath(Cell goalCell) {
            bool flag = false;
            foreach (Cell c in board.CellNeighbors(goalCell)) {
                if (cellsVisited.ContainsKey(new Tuple<int, int>(c.getX, c.getY)) && c.GetState() != State.Pit 
                    && c.GetState() != State.Obstacle && c.GetState() != State.Wumpus) {
                    flag = true;
                }
            }

            if (flag) {
                if (goalCell.getX == currentX && goalCell.getY == currentY) {
                    return;
                }

                Stack<Cell> path = new Stack<Cell>();
                path = CalculatePath(goalCell);
                path.Pop();
                // travel the path
                while (path.Count > 0) {
                    Cell gotoCell = path.Pop();
                    int gotoX = gotoCell.getX;
                    int gotoY = gotoCell.getY;

                    // only change y direction
                    // TODO: HAVE NOT tested if this works yet
                    if (gotoX > currentX) MoveEast();
                    else if (gotoX < currentX) MoveWest();
                    else if (gotoY > currentY) MoveNorth();
                    else if (gotoY < currentY) MoveSouth();
                }
            }

            // no path to desired cell, kill the agent
            else {
                isDead = true;
            }
        }

        private Stack<Cell> CalculatePath(Cell goalCell) {
            // cells to not path too, dead ends
            List<Cell> avoidCells = new List<Cell>();
            // path for agent to travel to 
            Stack<Cell> returnCells = new Stack<Cell>();
            // starting point == returned cell ; back => forward
            returnCells.Push(goalCell);
            do {
                Dictionary<Cell, double> distance = new Dictionary<Cell, double>();
                // step 1: look at the current cells surroundings , ignore the cells in the avoid Cells, and 
                // calculate distance. 
                foreach (Cell c in board.CellNeighbors(returnCells.Peek())) {
                    // if c is not avoided and visited, proceed
                    if (!avoidCells.Contains(c) && QueryVisited(c) && !returnCells.Contains(c)
                        && c.GetState() != State.Obstacle) {
                        distance.Add(c, CalcDistance(goalCell, c));
                    }
                }

                // if there is a possible path, calculate which cell to travel to next
                if (distance.Count > 0) {
                    // get smallest distance
                    double min = distance.Min(x => x.Value);
                    Cell leaf = distance.FirstOrDefault(x => x.Value.Equals(min)).Key;
                    returnCells.Push(leaf);
                }

                // if not cells were recognized as travelable, remove the most recently considred cell, add it to the
                // avoid cells list
                else if (returnCells.Count <= 1) {
                    isDead = true;
                    return returnCells;
                }
                else {
                    avoidCells.Add(returnCells.Pop());
                }
            } while (returnCells.Peek() != board[currentX,currentY]); // EXIT condition == stack starts with current pos
            return returnCells;
        }

        // calculate the distance from player cell to desired cell
        private double CalcDistance(Cell goalCell, Cell compCell)  => 
            Math.Sqrt((Math.Pow(goalCell.getX - compCell.getX, 2) + Math.Pow(goalCell.getY - compCell.getY, 2)));

        /// <summary>
        /// Append stats to the stats list.
        /// </summary>
        /// <param name="stat"></param>
        protected void AppendStatsList(Statistics stat) {
            statsList.Add(stat);
        }

        /// <summary>
        /// Print the average of the stats
        /// </summary>
        public void PrintAverage() {
            Statistics averageStats = new Statistics();
            // loop through stats list and calculate the average
            foreach (Statistics s in statsList) {
                s.PrintStats(board);
            }
            // foreach (Statistics s in statsList)
            // {
            //     averageStats.AddToStat('A', s.agentStats['A']);
            //     averageStats.AddToStat('G', s.agentStats['G']);
            //     averageStats.AddToStat('K', s.agentStats['K']);
            //     averageStats.AddToStat('W', s.agentStats['W']);
            //     averageStats.AddToStat('D', s.agentStats['D']);
            //     averageStats.AddToStat('P', s.agentStats['P']);
            //     averageStats.AddToStat('E', s.agentStats['E']);
            // }
            
            // divide stats by 10
            // foreach (var key in averageStats.agentStats.Keys.ToList()) {
            //     averageStats.agentStats[key] = averageStats.agentStats[key] / 10.0; //10 boards per run
            // }
            // print out the stats
            //averageStats.PrintStats(board);
            ClearAverage(); // clear stats from agent
        }

        /// <summary>
        /// Clears all stats from the stats
        /// </summary>
        private void ClearAverage() {
            statsList.Clear();
        } 
    }
}
