using System;

namespace Wumpus_World
{
    public class ReflexAgent : Agent
    {
        // Creates a Statistics class "stats" to track agent stats
        Statistics stats = new Statistics();
        // Random class for use when randomness is required
        Random random = new Random();

        // Truth values required for solving
        bool isSolved = false;
        bool isDead = false;

        // Variable to reference the board
        Board board;

        // Variable to hold the current cell the agent is in
        Cell currentCell;

        // X and Y values of the current cell
        int cellX;
        int cellY;

        /// <summary>
        /// Starts board navigation with the ReflexAgent
        /// </summary>
        /// <param name="_board"></param>
        public override void Navigate(Board _board)
        {
            // Sets the current board's current agent as this agent
            _board.SetAgent(this);

            // Sets the board variable to be the input _board
            board = _board;

            // Updates the urrentCell variable
            UpdateCurrentCell();

            // Assigning X and Y values of the current cell
            cellX = currentCell.getX;
            cellY = currentCell.getY;

            // Print the initial state of the board
            Console.WriteLine(_board.ToString());

            // Life of the agent: stops it from going on forever
            int life = 30;

            // Continue this while loop while the gold is not found and agent is not dead
            while (!isSolved && !isDead && life > 0)
            {
                // Runs a reflex move method which moves the agent
                ReflexMove();

                // Print the current state of the board
                Console.WriteLine(_board.ToString());

                // Gets the current cell the agent is in
                UpdateCurrentCell();

                // Runs a check to see if agent is dead
                DeathCheck();

                // If the current cell is gold:
                // The agent clears the board
                if (currentCell.GetState() == State.Gold)
                {
                    Console.WriteLine("\nEntered gold\n");
                    Console.WriteLine("Cleared board!");
                    isSolved = true;
                    stats.IncrementStat('G');
                }

                life++;
            }
            // Print stats after death or victory
            stats.PrintStats();
        }

        /// <summary>
        /// Moves the agent to an adjacent cell based on many turth values
        /// </summary>
        void ReflexMove()
        {
            // Variable to determine when to take risks that could lead to vicotry
            bool willTakeRisks = false;

            UpdateCurrentCell();

            Console.WriteLine("RA starting ReflexMove() in Cell X " + cellX + ", and Cell Y " + cellY);

            // Contains turth values of the current cell
            // Informs agent of breeze, smell, glitter
            Modifier mods = board.GetModifiers(currentCell);

            // Change behavior if near gold
            if (mods.isGlitter)
                willTakeRisks = true;
            else
                willTakeRisks = false;

            if (IsMoveValid(cellX, cellY + 1)
                && !willTakeRisks && !QueryVisited(board[cellX, cellY + 1]))
            {
                //Console.WriteLine("M1N:");
                MoveNorth();
                UpdateCurrentCell();
            }
            else if (IsMoveValid(cellX + 1, cellY)
                && !willTakeRisks && !QueryVisited(board[cellX + 1, cellY]))
            {
                //Console.WriteLine("M2E:");
                MoveEast();
                UpdateCurrentCell();
            }
            else if (IsMoveValid(cellX, cellY - 1)
                && !willTakeRisks && !QueryVisited(board[cellX, cellY - 1]))
            {
                //Console.WriteLine("M3S:");
                MoveSouth();
                UpdateCurrentCell();
            }
            else if (IsMoveValid(cellX - 1, cellY)
                && !willTakeRisks && !QueryVisited(board[cellX - 1, cellY]))
            {
                //Console.WriteLine("M4W:");
                MoveWest();
                UpdateCurrentCell();
            }

            // Prevents agent from getting stuck when
            // it's already explored nearby cells
            else if (!willTakeRisks)
            {
                int choice = random.Next(4);

                switch (choice)
                {
                    case 0:
                        if (IsMoveValid(cellX, cellY + 1))
                        {
                            MoveNorth();
                            UpdateCurrentCell();
                        }
                        break;
                    case 1:
                        if (IsMoveValid(cellX + 1, cellY))
                        {
                            MoveEast();
                            UpdateCurrentCell();
                        }
                        break;
                    case 2:
                        if (IsMoveValid(cellX, cellY - 1))
                        {
                            MoveSouth();
                            UpdateCurrentCell();
                        }
                        break;
                    case 3:
                        if (IsMoveValid(cellX - 1, cellY))
                        {
                            MoveWest();
                            UpdateCurrentCell();
                        }
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

                //TODO: only move if cell is unexplored to save actions
                while (!inGoldCell && !isDead)
                {
                    switch (direction)
                    {
                        case 0:
                            if (IsMoveValid(cellX, cellY + 1))
                            {
                                //Console.WriteLine("M5NS:");
                                MoveNorth();
                                UpdateCurrentCell();
                                DeathCheck();
                                if (currentCell.GetState() == State.Gold)
                                    inGoldCell = true;
                                else
                                {
                                    MoveSouth();
                                    UpdateCurrentCell();
                                }
                            }
                            break;
                        case 1:
                            if (IsMoveValid(cellX + 1, cellY))
                            {
                                //Console.WriteLine("M6EW:");
                                MoveEast();
                                UpdateCurrentCell();
                                DeathCheck();
                                if (currentCell.GetState() == State.Gold)
                                    inGoldCell = true;
                                else
                                {
                                    MoveWest();
                                    UpdateCurrentCell();
                                }
                            }
                            break;
                        case 2:
                            if (IsMoveValid(cellX, cellY - 1))
                            {
                                //Console.WriteLine("M7SN:");
                                MoveSouth();
                                UpdateCurrentCell();
                                DeathCheck();
                                if (currentCell.GetState() == State.Gold)
                                    inGoldCell = true;
                                else
                                {
                                    MoveNorth();
                                    UpdateCurrentCell();
                                }
                            }
                            break;
                        case 3:
                            if (IsMoveValid(cellX - 1, cellY))
                            {
                                //Console.WriteLine("M8WE:");
                                MoveWest();
                                UpdateCurrentCell();
                                DeathCheck();
                                if (currentCell.GetState() == State.Gold)
                                    inGoldCell = true;
                                else
                                {
                                    MoveEast();
                                    UpdateCurrentCell();
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    direction++;
                }
            }
            UpdateCurrentCell();
            Console.WriteLine("RA ending ReflexMove() in Cell X " + cellX + ", and Cell Y " + cellY);
        }

        // Method to update the current cell the agent is in
        /// <summary>
        /// Updates information of the cells X and Y values as well
        /// as the current cell the agent is in
        /// </summary>
        void UpdateCurrentCell()
        {
            currentCell = getCell(board);
            cellX = currentCell.getX;
            cellY = currentCell.getY;
        }

        /// <summary>
        /// Checks if a particular move is valid or not based on the
        /// desired cell and board size
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        /// <returns></returns>
        bool IsMoveValid(int _x, int _y)
        {
            // Retruns true if the desired move is valid
            if (_x > board.GetSize - 1 || _x < 0)
                return false;
            if (_y > board.GetSize - 1 || _y < 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Checks to see if the player is inside a dengerous cell
        /// and if so the player is considered dead
        /// </summary>
        void DeathCheck()
        {
            // If the current cell is a Wumpus:
            // The agent dies
            if (currentCell.GetState() == State.Wumpus)
            {
                Console.WriteLine("\nEntered Wumpus\n");
                isDead = true;
                stats.IncrementStat('D');
                stats.IncrementStat('W');
            }

            // If the current cell is a pit:
            // The agent dies
            if (currentCell.GetState() == State.Pit)
            {
                Console.WriteLine("\nEntered Pit\n");
                isDead = true;
                stats.IncrementStat('D');
                stats.IncrementStat('P');
            }
        }
    }
}