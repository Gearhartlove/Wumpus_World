using System.Data;

namespace Wumpus_World {
	
	//Evaluates for all rules in the for of (∀ x, y)
	public abstract class FOLUniversalRule {
		public abstract FOLFact eval(int x, int y);
	}
}