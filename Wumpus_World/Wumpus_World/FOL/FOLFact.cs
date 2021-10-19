using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Wumpus_World {
	public class FOLFact {

		
		/// <summary>
		/// Fields for the facts are here.
		/// </summary>
		private int x, y, width, height;
		private PredicateType type;
		private bool not = false;

		private FOLKnowledgeBase knowledgeBase;
		
		private FOLFact head;
		private FOLFact next;
		private UnifierType unifierType = UnifierType.NONE;

		
		/// <summary>
		/// This is the constructor for the fact class. It takes in the knowledge base to validate cells created by it. 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="knowledgeBase"></param>
		/// <param name="head"></param>
		protected internal FOLFact(PredicateType type, int x, int y, int width, int height, FOLKnowledgeBase knowledgeBase, FOLFact head = null) {
			this.head = head != null ? head : this;
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
			this.type = type;
			this.knowledgeBase = knowledgeBase;
		}

		/// <summary>
		/// This method add a fact appended to the end of this one with the 'and' quantifier. If the fact cannot be made, just returns this.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public FOLFact and(PredicateType type, int x, int y) {
			return newTerm(type, x, y, UnifierType.AND);
		}
		
		/// <summary>
		/// This method add a fact appended to the end of this one with the 'or' quantifier and returns it. If the fact cannot be made, just returns this.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public FOLFact or(PredicateType type, int x, int y) {
			return newTerm(type, x, y, UnifierType.OR);
		}

		/// <summary>
		/// this is a helper function for makeing and appending new terms.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="unifierType"></param>
		/// <returns></returns>
		private FOLFact newTerm(PredicateType type, int x, int y, UnifierType unifierType) {
			if (x < 0 || y < 0 || x >= width || y >= height) return this;
			if (knowledgeBase.cantBeOverriden(type, x, y)) return this;
			FOLFact term = new FOLFact(type, x, y, width, height, knowledgeBase, head);
			this.next = term;
			this.unifierType = unifierType;
			return term;
		}

		
		/// <summary>
		/// Will return true if the term is notted (-)
		/// </summary>
		/// <returns></returns>
		public FOLFact isNegative() {
			this.not = true;
			return this;
		}

		/// <summary>
		/// Returns true if the fact has a term after it.
		/// </summary>
		/// <returns></returns>
		public bool hasNext() {
			return next != null;
		}

		/// <summary>
		/// This method returns the first term in the chain
		/// </summary>
		/// <returns></returns>
		public FOLFact getHead() {
			return head;
		}

		/// <summary>
		/// This detaches the next node from the chain and returns it
		/// </summary>
		/// <returns></returns>
		public FOLFact detatchNext() {
			var n = next;
			next = null;
			unifierType = UnifierType.NONE;
			return n;
		}

		/// <summary>
		/// This method seporates an entire term into like term, ie 'ands' and 'or' chains separated and returned in a list.
		/// </summary>
		/// <param name="head"></param>
		/// <param name="current"></param>
		/// <param name="facts"></param>
		/// <returns></returns>
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

		/// <summary>
		/// This creates a string representation of the fact.
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			var not = this.not ? "-" : "";
			var str = not + type + "(" + x + ", " + y + ")";
			if (hasNext()) {
				string uni = unifierType == UnifierType.AND ? " A " : " v ";
				return str + uni + next.ToString();
			} else return str;
		}

		/// <summary>
		/// Returns the number of terms starting from this one.
		/// </summary>
		/// <param name="current"></param>
		/// <returns></returns>
		public int length(int current = 1) {
			if (hasNext()) return next.length(current: current + 1);
			return current;
		}

		/// <summary>
		/// Property for Not
		/// </summary>
		public bool Not {
			get => not;
			set => not = value;
		}

		/// <summary>
		/// This returns a copy of this term with no children.
		/// </summary>
		/// <returns></returns>
		public FOLFact singleOut() {
			return new FOLFact(type, x, y, width, height, knowledgeBase);
		}

		/// <summary>
		/// Equality check for term.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj) {
			if (obj is FOLFact) {
				FOLFact other = (FOLFact) obj;
				if (this.hasNext() && other.hasNext()) {
					return this.Type == other.type && this.x == other.x && this.y == other.y && this.Not == other.Not && this.next.Equals(other.next);
				}

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