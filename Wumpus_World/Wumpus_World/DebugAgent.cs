using System;
using System.Runtime.InteropServices;

namespace Wumpus_World {
    public class DebugAgent : Agent{
        public override void Navigate(Board board) {
            board.SetAgent(this);

            var start = getCell(board);
            
            Console.WriteLine(board);
            MoveEast();
            Console.WriteLine(board);
            MoveNorth();
            Console.WriteLine(board);
            MoveWest();
            Console.WriteLine(board);
            MoveSouth();
            Console.WriteLine(board);
            MoveSouth();
            Console.WriteLine(board);
            MoveSouth();
            Console.WriteLine(board);
            Console.WriteLine("TRAVELING BACK NOW");
            TravelPath(start);
            Console.WriteLine("Starting: x:" + start.getX + " y:" + start.getY);
            Console.WriteLine("Ending: x:" + getCell(board).getX + " y:" + getCell(board).getY);
        }
    }
}