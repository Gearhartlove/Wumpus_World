using System;

namespace Wumpus_World
{
    class Program
    {
        static void Main(string[] args) {
            
            FOAgent foAgent = new FOAgent(1, 1);
            Board board = new Board(3, foAgent);
            Console.WriteLine(board);

            Modifier mod = board.GetModifiers(board[0, 0]);
        }
    }
}