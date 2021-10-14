using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wumpus_World {
    public class Agent {

        private Direction facing = Direction.North;
        private int currentX, currentY;
        private int previousX, previousY;
        private int score = 0;
        private Board board;
        private Dictionary<Tuple<int,int>, bool> cellsVisited = new Dictionary<Tuple<int, int>, bool>();

        public void SpawnAgent(Cell spawn) {
            currentX = spawn.getX;
            currentY = spawn.getY;
            
            UpdateVisited();
        } 
        
        public Cell getCell(Board board) {
            return board[currentX, currentY];
        }

        public void SetBoard(Board board) {
            this.board = board;
        }

        /// <summary>
        /// Agent Navigating through Wumpus' World. This includes moving, recording statistics, as well as additional
        /// logic, depending on the type of agent. Will be overriden by children.
        /// </summary>
        /// <param name="board"></param>
        public virtual void Navigate(Board board) {
            board.SetAgent(this); // NEEDS TO BE INCLUDED FOR BOARD TO KNOW WHERE THE AGENT IS AND PRINT CORRECTLY.
            // Put navigating logic below
        }

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
                    currentX++;
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