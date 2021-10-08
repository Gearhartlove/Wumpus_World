namespace Wumpus_World {
    public class Agent {

        private Cell currentPos; 

        public Cell GetPos() {
              return currentPos;
        }

        public void SetPos(Cell pos) {
            currentPos = pos;
        }
    }
}