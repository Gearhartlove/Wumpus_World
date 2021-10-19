using System;
using System.Collections.Generic;

namespace Wumpus_World
{
    public class ReflexAgent : Agent
    {
        // cells traveled through
        public List<Cell> traveledCells = new List<Cell>();
        // Random class for use when randomness is required
        Random random = new Random();

        // Truth values required for solving
        bool isSolved = false;
        bool isDead = false;
        bool agentJustSpawned = true;

        // Variable which determines when the agent desparately needs to find the gold
        bool isDesparate = false;
        // Variable to determine how desparate the agent is
        int desparateLevel = 0;

        // Variable to hold the number of arrows the agent has
        int numArrows;

        // Variable to reference the board
        Board board;

        // Variable to hold the current cell the agent is in
        Cell currentCell;

        // Variable to hold the direction the agent came from
        Direction lastCellDirection;

        // X and Y values of the current cell
        int cellX;
        int cellY;

        /// <summary>
        /// Starts board navigation with the ReflexAgent
        /// </summary>
        /// <param name="_board"></param>
        public override void Navigate(Board _board)
        {
            base.Navigate(_board); // set agent to board, reset statistics
            // Sets the board variable to be the input _board
            board = _board;

            // Updates the urrentCell variable
            UpdateCurrentCell();

            // Sets the number of arrows equal to the number of Wumpus'
            numArrows = GetArrowCount;

            // Assigning X and Y values of the current cell
            cellX = currentCell.getX;
            cellY = currentCell.getY;

            // Life of the agent: stops it from going on forever
            int life = 50;

            // Continue this while loop while the gold is not found and agent is not dead
            while (!isSolved && !isDead && life > 0)
            {
                // Runs a reflex move method which moves the agent
                ReflexMove();

                // Gets the current cell the agent is in
                UpdateCurrentCell();

                // Runs a check to see if agent is dead
                DeathCheck();

                // If the current cell is gold:
                // The agent clears the board
                if (currentCell.GetState() == State.Gold)
                {
                    isSolved = true;
                    stats.incGoldFound();
                    stats.incWumpusFalls();
                }
                life--;
            }
            //stats.PrintStats(board); 
        }

        /// <summary>
        /// Moves the agent to an adjacent cell based on many turth values
        /// </summary>
        void ReflexMove()
        {
            // Variable to determine when to take risks that could lead to vicotry
            bool willTakeRisks = false;
            // Variable to determine when the agent will avoid pits
            bool willAvoidPits = false;
            // Variable to determine when the agent will avoid a Wumpus
            bool willAvoidWumpus = false;

            UpdateCurrentCell();

            // Contains turth values of the current cell
            // Informs agent of breeze, smell, glitter
            Modifier mods = board.GetModifiers(currentCell);

            // Change behavior if near gold
            if (mods.isGlitter)
                willTakeRisks = true;
            else
                willTakeRisks = false;

            // Change behavior if near pit
            if (mods.isBreeze)
                willAvoidPits = true;
            else
                willAvoidPits = false;

            // Change behavior if near Wumpus
            if (mods.isSmell)
                willAvoidWumpus = true;
            else
                willAvoidWumpus = false;

            // Base exploration behavior:
            // Goes to cells not yet explored in search of gold
            // Avoids dangers unless taking a risk is the only option
            // When a Wumpus is near, will shoot arrows to kill the Wumpus
            if (IsMoveValid(cellX, cellY + 1)
                && !willTakeRisks && !QueryVisited(board[cellX, cellY + 1]))
            {
                if (agentJustSpawned)
                {
                    MoveNorth();
                    UpdateCurrentCell();
                    stats.incCellsExplored();
                    lastCellDirection = Direction.South;
                }
                else if (!agentJustSpawned && !willAvoidPits && !willAvoidWumpus)
                {
                    MoveNorth();
                    UpdateCurrentCell();
                    lastCellDirection = Direction.South;
                }
                else if (!agentJustSpawned && (willAvoidPits || willAvoidWumpus) && !isDesparate)
                {
                    MoveBack();
                    UpdateCurrentCell();
                }
                else if (!agentJustSpawned && (willAvoidPits || willAvoidWumpus) && isDesparate)
                {
                    MoveNorth();
                    UpdateCurrentCell();
                    lastCellDirection = Direction.South;
                }
            }
            else if (IsMoveValid(cellX + 1, cellY)
                && !willTakeRisks && !QueryVisited(board[cellX + 1, cellY]))
            {
                if (agentJustSpawned)
                {
                    MoveEast();
                    UpdateCurrentCell();
                    lastCellDirection = Direction.West;
                }
                else if (!agentJustSpawned && !willAvoidPits && !willAvoidWumpus)
                {
                    MoveEast();
                    UpdateCurrentCell();
                    lastCellDirection = Direction.West;
                }
                else if (!agentJustSpawned && (willAvoidPits || willAvoidWumpus) && !isDesparate)
                {
                    MoveBack();
                    UpdateCurrentCell();
                }
                else if (!agentJustSpawned && (willAvoidPits || willAvoidWumpus) && isDesparate)
                {
                    MoveEast();
                    UpdateCurrentCell();
                    lastCellDirection = Direction.West;
                }
            }
            else if (IsMoveValid(cellX, cellY - 1)
                && !willTakeRisks && !QueryVisited(board[cellX, cellY - 1]))
            {
                if (agentJustSpawned)
                {
                    MoveSouth();
                    UpdateCurrentCell();
                    lastCellDirection = Direction.North;
                }
                else if (!agentJustSpawned && !willAvoidPits && !willAvoidWumpus)
                {
                    MoveSouth();
                    UpdateCurrentCell();
                    lastCellDirection = Direction.North;
                }
                else if (!agentJustSpawned && (willAvoidPits || willAvoidWumpus) && !isDesparate)
                {
                    MoveBack();
                    UpdateCurrentCell();
                }
                else if (!agentJustSpawned && (willAvoidPits || willAvoidWumpus) && isDesparate)
                {
                    MoveSouth();
                    UpdateCurrentCell();
                    lastCellDirection = Direction.North;
                }
            }
            else if (IsMoveValid(cellX - 1, cellY)
                && !willTakeRisks && !QueryVisited(board[cellX - 1, cellY]))
            {
                if (agentJustSpawned)
                {
                    MoveWest();
                    UpdateCurrentCell();
                    lastCellDirection = Direction.East;
                }
                else if (!agentJustSpawned && !willAvoidPits && !willAvoidWumpus)
                {
                    MoveWest();
                    UpdateCurrentCell();
                    lastCellDirection = Direction.East;
                }
                else if (!agentJustSpawned && (willAvoidPits || willAvoidWumpus) && !isDesparate)
                {
                    MoveBack();
                    UpdateCurrentCell();
                }
                else if (!agentJustSpawned && (willAvoidPits || willAvoidWumpus) && isDesparate)
                {
                    MoveWest();
                    UpdateCurrentCell();
                    lastCellDirection = Direction.East;
                }
            }

            // Prevents agent from getting stuck when
            // it's already explored nearby cells
            else if (!willTakeRisks)
            {
                bool decidedMove = false;
                int decisions = 0;

                // Decide where to move so long as it's a valid move
                while (!decidedMove && decisions <= 16)
                {
                    decisions++;
                    int choice = random.Next(4);
                    switch (choice)
                    {
                        case 0:
                            if (IsMoveValid(cellX, cellY + 1))
                            {
                                decidedMove = true;
                                MoveNorth();
                                UpdateCurrentCell();
                                lastCellDirection = Direction.South;
                            }
                            break;
                        case 1:
                            if (IsMoveValid(cellX + 1, cellY))
                            {
                                decidedMove = true;
                                MoveEast();
                                UpdateCurrentCell();
                                lastCellDirection = Direction.West;
                            }
                            break;
                        case 2:
                            if (IsMoveValid(cellX, cellY - 1))
                            {
                                decidedMove = true;
                                MoveSouth();
                                UpdateCurrentCell();
                                lastCellDirection = Direction.North;
                            }
                            break;
                        case 3:
                            if (IsMoveValid(cellX - 1, cellY))
                            {
                                decidedMove = true;
                                MoveWest();
                                UpdateCurrentCell();
                                lastCellDirection = Direction.East;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            // Remember the cell the gold is adjacent to then
            // go to each of those adjacent cells until gold is found
            if (willTakeRisks)
            {
                bool inGoldCell = false;
                int adjacentCell = 0;

                while (!inGoldCell && !isDead)
                {
                    switch (adjacentCell)
                    {
                        case 0:
                            if (IsMoveValid(cellX, cellY + 1) && !QueryVisited(board[cellX, cellY + 1]))
                            {
                                MoveNorth();
                                UpdateCurrentCell();
                                lastCellDirection = Direction.South;
                                DeathCheck();
                                if (currentCell.GetState() == State.Gold)
                                    inGoldCell = true;
                                else
                                {
                                    MoveBack();
                                    UpdateCurrentCell();
                                }
                            }
                            break;
                        case 1:
                            if (IsMoveValid(cellX + 1, cellY) && !QueryVisited(board[cellX + 1, cellY]))
                            {
                                MoveEast();
                                UpdateCurrentCell();
                                lastCellDirection = Direction.West;
                                DeathCheck();
                                if (currentCell.GetState() == State.Gold)
                                    inGoldCell = true;
                                else
                                {
                                    MoveBack();
                                    UpdateCurrentCell();
                                }
                            }
                            break;
                        case 2:
                            if (IsMoveValid(cellX, cellY - 1) && !QueryVisited(board[cellX, cellY - 1]))
                            {
                                MoveSouth();
                                UpdateCurrentCell();
                                lastCellDirection = Direction.North;
                                DeathCheck();
                                if (currentCell.GetState() == State.Gold)
                                    inGoldCell = true;
                                else
                                {
                                    MoveBack();
                                    UpdateCurrentCell();
                                }
                            }
                            break;
                        case 3:
                            if (IsMoveValid(cellX - 1, cellY) && !QueryVisited(board[cellX - 1, cellY]))
                            {
                                MoveWest();
                                UpdateCurrentCell();
                                lastCellDirection = Direction.South;
                                DeathCheck();
                                if (currentCell.GetState() == State.Gold)
                                    inGoldCell = true;
                                else
                                {
                                    MoveBack();
                                    UpdateCurrentCell();
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    adjacentCell++;
                }
            }
            agentJustSpawned = false;
        }

        /// <summary>
        /// Moves the agent to the last cell it was in
        /// </summary>
        void MoveBack()
        {
            if (desparateLevel >= 5)
                isDesparate = true;
            desparateLevel++;

            if (lastCellDirection == Direction.North)
            {
                MoveNorth();
                lastCellDirection = Direction.South;
            }
            else if (lastCellDirection == Direction.East)
            {
                MoveEast();
                lastCellDirection = Direction.West;
            }
            else if (lastCellDirection == Direction.South)
            {
                MoveSouth();
                lastCellDirection = Direction.North;
            }
            else if (lastCellDirection == Direction.West)
            {
                MoveWest();
                lastCellDirection = Direction.East;
            }
        }

        // Method to update the current cell the agent is in
        /// <summary>
        /// Updates information of the cells X and Y values as well
        /// as the current cell the agent is in
        /// </summary>
        void UpdateCurrentCell()
        {
            currentCell = getCell(board);
            if (!traveledCells.Contains(currentCell)) {
                traveledCells.Add(currentCell);
                stats.incCellsExplored();
            }
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

            if (board[_x, _y].GetState() == State.Obstacle && QueryVisited(board[_x, _y]))
                return false;

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
                isDead = true;
                stats.incWumpusFalls();
            }

            // If the current cell is a pit:
            // The agent dies
            if (currentCell.GetState() == State.Pit)
            {
                isDead = true;
                stats.incPitFalls();
            }
        }
    }
}