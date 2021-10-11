namespace Wumpus_World {
    public class Agent {

        private Direction facing = Direction.North;
        private int currentX, currentY;
        private int score = 0;

        public Agent(int currentX, int currentY) {
            this.currentX = currentX;
            this.currentY = currentY;
        }

        public Cell getCell(Board board) {
            return board[currentX, currentY];
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
                    currentX--;
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
    }
}