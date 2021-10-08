namespace Wumpus_World
{
    public class Cell
    {
        private State state;
        // initialized to -1, change when board is generated
        private int x;
        private int y;

        public int getX => x;
        public int getY => y;
        
        public Cell(int x, int y) {
            //instantiate Obs
             state = State.Empty;
             this.x = x;
             this.y = y;
        }

        public State S {
            get => state;
            set {
                if (value == State.Gold || value == State.Spawn) {
                    state = value;
                }
                // If the cell is empty, then assign the cell to the obstacle
                if (state == State.Empty) state = value;
            }
        }
        
        public State GetState() => S;
    }

    public enum State {
        Spawn,
        Gold,
        Wumpus,
        Obstacle,
        Pit,
        Empty
    }
}