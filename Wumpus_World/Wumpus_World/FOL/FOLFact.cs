using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Wumpus_World {
	public class FOLFact {

		private int x, y, width, height;
		private PredicateType type;
		private bool not = false;

		private FOLKnowledgeBase knowledgeBase;
		
		private FOLFact head;
		private FOLFact next;
		private UnifierType unifierType = UnifierType.NONE;

		
		
		protected internal FOLFact(PredicateType type, int x, int y, int width, int height, FOLKnowledgeBase knowledgeBase, FOLFact head = null) {
			this.head = head != null ? head : this;
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
			this.type = type;
			this.knowledgeBase = knowledgeBase;
		}

		public FOLFact and(PredicateType type, int x, int y) {
			return newTerm(type, x, y, UnifierType.AND);
		}
		
		public FOLFact or(PredicateType type, int x, int y) {
			return newTerm(type, x, y, UnifierType.OR);
		}

		private FOLFact newTerm(PredicateType type, int x, int y, UnifierType unifierType) {
			if (x < 0 || y < 0 || x >= width || y >= height) return this;
			if (knowledgeBase.cantBeOverriden(type, x, y)) return this;
			FOLFact term = new FOLFact(type, x, y, width, height, knowledgeBase, head);
			this.next = term;
			this.unifierType = unifierType;
			return term;
		}

		public FOLFact isNegative() {
			this.not = true;
			return this;
		}

		public bool hasNext() {
			return next != null;
		}

		public FOLFact getHead() {
			return head;
		}

		public FOLFact detatchNext() {
			var n = next;
			next = null;
			unifierType = UnifierType.NONE;
			return n;
		}

		public List<FOLFact> separate(FOLFact head = null, FOLFact current = null,  List<FOLFact> facts = null) {
			facts = facts == null ? new List<FOLFact>() : facts;
			head = head == null ? this : head;
			current = current == null ? this : current;
			if (current.hasNext()) {
				var n = current.next;
				if (current.unifierType == UnifierType.AND) {
					current.detatchNext();
					facts.Add(head);

					return separate(facts: facts, head: n, current: n);
				}
				
				return separate(facts: facts, head: head, current:n);
			}

			facts.Add(head);
			return facts;
		}

		public override string ToString() {
			var not = this.not ? "-" : "";
			var str = not + type.ToString() + "(" + x + ", " + y + ")";
			if (hasNext()) {
				string uni = unifierType == UnifierType.AND ? " A " : " v ";
				return str + uni + next.ToString();
			} else return str;
		}

		public int length(int current = 1) {
			if (hasNext()) return next.length(current: current + 1);
			return current;
		}

		public bool Not {
			get => not;
			set => not = value;
		}

		public FOLFact singleOut() {
			return new FOLFact(type, x, y, width, height, knowledgeBase);
		}

		public override bool Equals(object obj) {
			if (obj is FOLFact) {
				FOLFact other = (FOLFact) obj;
				return this.Type == other.type && this.x == other.x && this.y == other.y;
			}
			
			return false;
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
		MOVEABLE,
		EMPTY
	}
	
	public enum UnifierType {
		AND,
		OR,
		NONE
	}
}