namespace Wumpus_World {
    public class FOLAgent : Agent {

        private FOLKnowledgeBase KnowledgeBase;

        public override void Navigate(Board board) {
            base.Navigate(board);

            FOLKnowledgeBase knowledgeBase = new FOLKnowledgeBase(board.GetSize, board.GetSize);

            while (AliveCheck()) {
                if(GoldCheck()) break;
                
                var modifiers = board.GetModifiers(getCell(board));
                
                if(modifiers.isBreeze) knowledgeBase.addPercept(PredicateType.BREEZE, currentX, currentY);
                if(modifiers.isGlitter) knowledgeBase.addPercept(PredicateType.GLITTER, currentX, currentY);
                if(modifiers.isSmell) knowledgeBase.addPercept(PredicateType.SMELL, currentX, currentX);
            }
            
           
        }
    }
}