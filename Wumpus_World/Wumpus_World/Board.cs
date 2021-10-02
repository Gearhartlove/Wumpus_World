namespace Wumpus_World
{
    public class Board
    {
        private Cell[,] board;
        private const int DEFSIZE = 5; // default board size if no other size is given
        
        /// <summary>
        /// Creates a Wumpus board. Uses the probabilities defined above to allocate board states and modifiers.
        /// </summary>
        /// <param name="size"></param>
        public Board(int size = DEFSIZE)
        {
            // Create the board
            board = new Cell[size, size];
            // Place Wumpus, Pits, and Walls on the board
            
            // Place Gold and Player on the board. If no room for both, regenerate the map seed. 
             
        }

        private void GenObsticles()
        {
            
        }

        private void GenGold()
        {
            
        } 
        
        private void GenSpawn()
        {
            
        }
        
        //Do we want to have the running the board functionality in this class???

    }
}