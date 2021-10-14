namespace Wumpus_World
{
    public class Cell
    {
        // Arguments
        private State state;
        private int x;
        private int y;

        // Get args
        public int getX => x;
        public int getY => y;
        
        public Cell(int x, int y) {
            //instantiate State 
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

    /// <summary>
    /// Possible states a cell could be.
    /// </summary>
    public enum State {
        Spawn,
        Gold,
        Wumpus,
        Obstacle,
        Pit,
        Empty
    }
}