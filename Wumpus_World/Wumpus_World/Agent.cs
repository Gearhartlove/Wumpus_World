using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wumpus_World {
    public class Agent {

        private Direction facing = Direction.North;
        private int currentX, currentY;
        private int previousX, previousY;
        private int arrowX, arrowY;
        private int arrowCount;
        private int score = 0;
        private Board board;
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
        public void Navigate(Board board) {
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
            if (board[arrowX, arrowY].GetState() == State.Wumpus) return true;
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
            int prevX = currentX;
            int prevY = currentY;
            
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
                case State.Pit:
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Updated cells visited list;
        /// </summary>
        private void UpdateVisited() {
            if (!cellsVisited.ContainsKey(new Tuple<int, int>(currentX, currentY))) {
                cellsVisited.Add(new Tuple<int, int>(currentX, currentY), true);
            }
        }

        /// <summary>
        /// If the cell has been visited return true, else return false
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool QueryVisited(Cell cell) {
            if (!cellsVisited.ContainsKey(new Tuple<int, int>(cell.getX, cell.getY))) return false;
            else return true;
        }
    }
}