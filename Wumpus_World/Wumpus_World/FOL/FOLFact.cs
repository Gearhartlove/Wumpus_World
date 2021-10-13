namespace Wumpus_World {
	public class FOLFact {

		private int x, y;
		private PredicateType type;
		private bool not = false;
		
		private FOLFact next;
		private UnifierType unifierType;

		public FOLFact(PredicateType type, int x, int y) {
			this.x = x;
			this.y = y;
			this.type = type;
		}

		public FOLFact and(FOLFact term) {
			this.next = term;
			this.unifierType = UnifierType.AND;
			return term;
		}
		
		public FOLFact or(FOLFact term) {
			this.next = term;
			this.unifierType = UnifierType.OR;
			return term;
		}

		public bool Not {
			get => not;
			set => not = value;
		}

		public int X => x;

		public int Y => y;

		public PredicateType Type => type;

		public FOLFact Next => next;

		public UnifierType UnifierType => unifierType;
	}

	public enum PredicateType {
		PIT,
		WUMPUS,
		OBSTACLE,
		GOLD,
		SAFE,
		SMELL,
		GLITTER,
		BREEZE,
		MOVEABLE
	}
	
	public enum UnifierType {
		AND,
		OR
	}
}