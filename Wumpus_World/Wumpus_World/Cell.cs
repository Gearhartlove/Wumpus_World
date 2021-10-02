namespace Wumpus_World
{
    public class Cell
    {
        private Obstacle obs = Obstacle.Empty;
        public Obstacle Obs
        {
            get => obs;
            set
            {
                // If the cell is empty, then assign the cell to the obstacle
                if (obs == Obstacle.Empty) obs = value;
            }
        }
        //cell modifiers
        public bool isBreeze = false;
        public bool isGlitter = false;
        public bool isSmelly = false;
        public bool isSafe = false; //Do we need this one? 
       
        //Get Modifier(s) Method
        
    }

    public enum Obstacle
    {
       Wumpus,
       Wall,
       Pit,
       Empty
    }
}