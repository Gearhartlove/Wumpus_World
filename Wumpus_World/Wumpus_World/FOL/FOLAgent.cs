namespace Wumpus_World {
    public class FOLAgent : Agent {

        private FOLKnowledgeBase KnowledgeBase;

        public override void Navigate(Board board) {
            base.Navigate(board);
            KnowledgeBase = new FOLKnowledgeBase(board.GetSize, board.GetSize);
            
            board.GetModifiers(getCell(board));
            
            
        }
    }
}