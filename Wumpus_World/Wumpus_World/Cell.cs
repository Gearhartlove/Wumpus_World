namespace Wumpus_World
{
    public class Cell
    {
        private Obstacle obs;
        
        public Cell() {
            //instantiate Obs
             obs = Obstacle.Initial;
        }

        public Obstacle Obs {
            get => obs;
            set {
                if (value == Obstacle.Gold || value == Obstacle.Spawn) {
                    obs = value;
                }
                // If the cell is empty, then assign the cell to the obstacle
                if (obs == Obstacle.Initial) obs = value;
            }
        }

        public Obstacle GetObsticle() => Obs;
        
        //cell modifiers
        public bool isBreeze = false;
        public bool isGlitter = false;
        public bool isSmelly = false;
        public bool isSafe = false; //Do we need this one? 
       
        //Get Modifier(s) Method //Should this return a tuple? 
        // TODO: is the best way to do this? 
        public bool GetBreeze => isBreeze;
        public bool GetGlitter => isGlitter;
        public bool GetSmelly => isSmelly;
        public bool GetSafe => isSafe;
    }

    public enum Obstacle {
        Initial, //think about this design if it's a problem later
        Spawn,
        Gold,
        Wumpus,
        Wall,
        Pit,
        Empty
    }
}