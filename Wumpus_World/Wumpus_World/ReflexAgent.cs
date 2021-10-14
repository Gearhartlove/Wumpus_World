using System;

namespace Wumpus_World
{
    public class ReflexAgent : Agent
    {
        // Creates a Statistics class "stats" to track agent stats
        Statistics stats = new Statistics();
        // Random class for use when randomness is required
        Random random = new Random();

        // Method called to start board navigation
        public override void Navigate(Board _board)
        {
            // Sets the current board's current agent as this agent
            _board.SetAgent(this);

            // Truth values required for solving
            bool isSolved = false;
            bool isDead = false;

            // Variable to hold the value of the current cell
            Cell currentCell = getCell(_board);

            // Print the initial state of the board
            Console.WriteLine(_board.ToString());

            // Continue this while loop while the gold is not found and agent is not dead
            while (!isSolved && !isDead)
            {
                // Runs ReflexMove method which moves the agent
                ReflexMove();

                // Print the current state of the board
                Console.WriteLine(_board.ToString());

                // Gets the current cell the agent is in
                currentCell = getCell(_board);

                // Runs a check to see if agent is dead
                DeathCheck();

                // If the current cell is gold:
                // The agent clears the board
                if (currentCell.GetState() == State.Gold)
                {
                    Console.WriteLine("\nEntered gold\n");
                    Console.WriteLine("Cleared board!");
                    isSolved = true;
                    stats.IncrementValue(1);
                }
            }


            void ReflexMove()
            {
                // Variable to determine when to take risks that could lead to vicotry
                bool willTakeRisks = false;

                // Gets the current cell the agent is inside
                currentCell = getCell(_board);

                // The size of the current board
                int boardSize = _board.GetSize;
                // X and Y values of the current cell
                int cellX = currentCell.getX;
                int cellY = currentCell.getY;

                // Class containing turth values of the current cell
                // Informs agent of breeze, smell, glitter
                Modifier mods = _board.GetModifiers(currentCell);

                Console.WriteLine("Cell X " + cellX + ", Cell Y " + cellY);
                if (cellX > 4 || cellY > 4)
                {
                    isSolved = true;
                }

                // Prevents agent from moving out of bounds
                bool canMoveNorth = IsMoveValid(boardSize, cellX, cellY + 1);
                bool canMoveEast = IsMoveValid(boardSize, cellX + 1, cellY);
                bool canMoveSouth = IsMoveValid(boardSize, cellX, cellY - 1);
                bool canMoveWest = IsMoveValid(boardSize, cellX - 1, cellY);

                // Change behavior if near gold
                if (mods.isGlitter)
                    willTakeRisks = true;
                else
                    willTakeRisks = false;

                //DEBUG
                Console.WriteLine("" + canMoveNorth + canMoveEast + canMoveSouth + canMoveWest);

                if (canMoveNorth && !willTakeRisks && !QueryVisited(_board[cellX, cellY + 1]))
                {
                    Console.WriteLine("M1N:");
                    MoveNorth();
                }
                else if (canMoveEast && !willTakeRisks && !QueryVisited(_board[cellX + 1, cellY]))
                {
                    Console.WriteLine("M2E:");
                    MoveEast();
                }
                else if (canMoveSouth && !willTakeRisks && !QueryVisited(_board[cellX, cellY - 1]))
                {
                    Console.WriteLine("M3S:");
                    MoveSouth();
                }
                else if (canMoveWest && !willTakeRisks && !QueryVisited(_board[cellX - 1, cellY]))
                {
                    Console.WriteLine("M4W:");
                    MoveWest();
                }
                else if (!willTakeRisks)
                {
                    // Prevents agent from getting stuck when
                    // it's already explored nearby cells
                    int choice = random.Next(4);

                    switch (choice)
                    {
                        case 0:
                            if (canMoveNorth)
                                MoveNorth();
                            break;
                        case 1:
                            if (canMoveEast)
                                MoveEast();
                            break;
                        case 2:
                            if (canMoveSouth)
                                MoveSouth();
                            break;
                        case 3:
                            if (canMoveWest)
                                MoveWest();
                            break;
                        default:
                            break;
                    }
                }

                // Remember the cell the gold is adjacent to then
                // go to each of those adjacent cells until gold is found
                if (willTakeRisks)
                {
                    bool inGoldCell = false;
                    int direction = 0;

                    while (!inGoldCell && !isDead)
                    {
                        switch (direction)
                        {
                            case 0:
                                if (canMoveNorth)
                                {
                                    Console.WriteLine("M5NS:");
                                    MoveNorth();
                                    DeathCheck();
                                    if (currentCell.GetState() == State.Gold)
                                        inGoldCell = true;
                                    else
                                        MoveSouth();
                                }
                                break;
                            case 1:
                                if (canMoveEast)
                                {
                                    Console.WriteLine("M6EW:");
                                    MoveEast();
                                    DeathCheck();
                                    if (currentCell.GetState() == State.Gold)
                                        inGoldCell = true;
                                    else
                                        MoveWest();
                                }
                                break;
                            case 2:
                                if (canMoveSouth)
                                {
                                    Console.WriteLine("M7SN:");
                                    MoveSouth();
                                    DeathCheck();
                                    if (currentCell.GetState() == State.Gold)
                                        inGoldCell = true;
                                    else
                                        MoveNorth();
                                }
                                break;
                            case 3:
                                if (canMoveWest)
                                {
                                    Console.WriteLine("M8WE:");
                                    MoveWest();
                                    DeathCheck();
                                    if (currentCell.GetState() == State.Gold)
                                        inGoldCell = true;
                                    else
                                        MoveEast();
                                }
                                break;
                            default:
                                break;
                        }
                        direction++;
                    }
                }
            }

            // Method to check if a particular move is valid
            bool IsMoveValid(int _size, int _x, int _y)
            {
                // Retruns true if the desired move is valid
                if (_x > _size - 1 || _x < 0)
                    return false;
                if (_y > _size - 1 || _y < 0)
                    return false;
                else
                    return true;
            }

            // Method to check if the agent is in a dangerous cell and should be considered dead
            void DeathCheck()
            {
                // If the current cell is a Wumpus:
                // The agent dies
                if (currentCell.GetState() == State.Wumpus)
                {
                    Console.WriteLine("\nEntered Wumpus\n");
                    isDead = true;
                    stats.IncrementValue(3);
                    stats.IncrementValue(4);
                }

                // If the current cell is a pit:
                // The agent dies
                if (currentCell.GetState() == State.Pit)
                {
                    Console.WriteLine("\nEntered Pit\n");
                    isDead = true;
                    stats.IncrementValue(5);
                    stats.IncrementValue(4);
                }
            }
                // Print stats after death or victory
                stats.PrintStats();
        }
    }
}