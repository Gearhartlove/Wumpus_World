namespace Wumpus_World {
    public class Modifier {
        public Modifier(Cell c, Board board) {
            int x = c.getX;
            int y = c.getY;
            
            //look at self
            Mod(board[x, y]);
            
            //look North
            if (y + 1 < board.GetSize) {
                Mod(board[x, y + 1]);
            } 
            
            //look South
            if (y - 1 > -1 ) {
                Mod(board[x, y - 1]); 
            }

            //look West
            if (x - 1 > -1) {
               Mod(board[x-1,y]); 
            }

            //look East
            if (x + 1 < board.GetSize) {
                Mod(board[x + 1, y]);
            }
        }

        public bool isBreeze = false;
        public bool isSmell = false;
        public bool isGlitter = false;

        /// <summary>
        /// Mods the current cell; depends on the cells surrounding the modified cell 
        /// </summary>
        /// <param name="c"></param>
        private void Mod(Cell c) {
            switch (c.GetState()) {
                case State.Wumpus:
                    isSmell = true;
                    break;
                case State.Pit:
                    isBreeze = true;
                    break;
                case State.Gold:
                    isGlitter = true;
                    break;
            }
        }
    }
}