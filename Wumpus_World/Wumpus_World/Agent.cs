using System;
using System.Collections.Generic;

namespace Wumpus_World {
    public class Agent {

        private Direction facing = Direction.North;
        private int currentX, currentY;
        private int score = 0;
        private List<Cell> cellsVisited;

        public void SpawnAgent(Cell spawn) {
            currentX = spawn.getX;
            currentY = spawn.getY;
        } 
        
        public Cell getCell(Board board) {
            return board[currentX, currentY];
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

        protected void walkForward() {
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

            score++;
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
        public void MoveNorth() {
            switch (facing) {
                case Direction.North:
                    walkForward();
                    break;
                case Direction.South:
                    turnRight(); // right(south) => West
                    turnRight(); // right(west) => North
                    walkForward();
                    break;
                case Direction.East:
                    turnLeft(); // left(East) => North
                    walkForward();
                    break;
                case Direction.West:
                    turnRight(); // right(West) => North
                    walkForward();
                    break;
            }
        }
        
        /// Helper method or "macro" to move agent South (turns included)
        public void MoveSouth() {
            switch (facing) {
                case Direction.North:
                    turnRight(); // right(north) => East
                    turnRight(); // right(east) => South
                    walkForward();
                    break;
                case Direction.South:
                    walkForward();
                    break;
                case Direction.East:
                    turnRight(); // right(east) => South
                    walkForward();
                    break;
                case Direction.West:
                    turnLeft();// left(West) => North
                    walkForward();
                    break;
            }
        }
        
        /// Helper method or "macro" to move agent West (turns included)
        public void MoveWest() {
            switch (facing) {
                case Direction.North:
                    turnLeft(); // left(North) => West
                    walkForward();
                    break;
                case Direction.South:
                    turnRight(); // right(South) => West
                    walkForward();
                    break;
                case Direction.East:
                    turnLeft(); // left(East) => North
                    turnLeft(); // left(North) => East
                    walkForward();
                    break;
                case Direction.West:
                    walkForward();
                    break;
            }
        }
        
        // Helper method or "macro" to move agent Eorth (turns included)
        public void MoveEast() {
            switch (facing) {
                case Direction.North:
                    turnRight(); // right(North) => East
                    walkForward();
                    break;
                case Direction.South:
                    turnLeft(); // left(South) => East
                    walkForward();
                    break;
                case Direction.East:
                    walkForward();
                    break;
                case Direction.West:
                    turnRight(); // right(West) => North
                    turnRight(); // right(North) => East
                    walkForward();
                    break;
            }
        }
    }
}