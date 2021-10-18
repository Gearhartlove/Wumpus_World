﻿using System;

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
            // Sets the current board's current agent as this agent
            _board.SetAgent(this);

            // Sets the board variable to be the input _board
            board = _board;

            // Updates the urrentCell variable
            UpdateCurrentCell();

            // Sets the number of arrows equal to the number of Wumpus'
            numArrows = GetArrowCount();

            // Assigning X and Y values of the current cell
            cellX = currentCell.getX;
            cellY = currentCell.getY;

            // Print the initial state of the board
            Console.WriteLine(_board.ToString());

            // Life of the agent: stops it from going on forever
            int life = 50;

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
                    isSolved = true;
                    stats.IncrementStat('G');
                }

                life--;
            }
            // Print stats after death or victory
            stats.PrintStats();
        }

        /// <summary>
        /// Moves the agent to an adjacent cell based on many turth values
        /// </summary>
        void ReflexMove()
        {
            // Variable to track direction while shooting
            int forDirection = 0;
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
                    for (int i = 4; i > 0; i--)
                    {
                        if (numArrows > 0 && willAvoidWumpus)
                        {
                            Shoot();
                            turnRight();
                            forDirection++;
                            numArrows--;
                        }
                        switch (forDirection)
                        {
                            case 0:
                                lastCellDirection = Direction.North;
                                break;
                            case 1:
                                lastCellDirection = Direction.East;
                                break;
                            case 2:
                                lastCellDirection = Direction.South;
                                break;
                            case 3:
                                lastCellDirection = Direction.West;
                                break;
                            case 4:
                                lastCellDirection = Direction.North;
                                break;
                            default:
                                break;
                        }
                    }
                    forDirection = 0;
                    MoveNorth();
                    UpdateCurrentCell();
                    stats.IncrementStat('A');
                    lastCellDirection = Direction.South;
                }
                else if (!agentJustSpawned && !willAvoidPits && !willAvoidWumpus)
                {
                    MoveNorth();
                    UpdateCurrentCell();
                    stats.IncrementStat('A');
                    lastCellDirection = Direction.South;
                }
                else if (!agentJustSpawned && (willAvoidPits || willAvoidWumpus) && !isDesparate)
                {
                    for (int i = 4; i > 0; i--)
                    {
                        if (numArrows > 0 && willAvoidWumpus)
                        {
                            Shoot();
                            turnRight();
                            forDirection++;
                            numArrows--;
                        }
                        switch (forDirection)
                        {
                            case 0:
                                lastCellDirection = Direction.North;
                                break;
                            case 1:
                                lastCellDirection = Direction.East;
                                break;
                            case 2:
                                lastCellDirection = Direction.South;
                                break;
                            case 3:
                                lastCellDirection = Direction.West;
                                break;
                            case 4:
                                lastCellDirection = Direction.North;
                                break;
                            default:
                                break;
                        }
                    }
                    forDirection = 0;
                    MoveBack();
                    UpdateCurrentCell();
                    stats.IncrementStat('A');
                }
                else if (!agentJustSpawned && (willAvoidPits || willAvoidWumpus) && isDesparate)
                {
                    for (int i = 4; i > 0; i--)
                    {
                        if (numArrows > 0 && willAvoidWumpus)
                        {
                            Shoot();
                            turnRight();
                            forDirection++;
                            numArrows--;
                        }
                        switch (forDirection)
                        {
                            case 0:
                                lastCellDirection = Direction.North;
                                break;
                            case 1:
                                lastCellDirection = Direction.East;
                                break;
                            case 2:
                                lastCellDirection = Direction.South;
                                break;
                            case 3:
                                lastCellDirection = Direction.West;
                                break;
                            case 4:
                                lastCellDirection = Direction.North;
                                break;
                            default:
                                break;
                        }
                    }
                    forDirection = 0;
                    MoveNorth();
                    UpdateCurrentCell();
                    stats.IncrementStat('A');
                    lastCellDirection = Direction.South;
                }
            }
            else if (IsMoveValid(cellX + 1, cellY)
                && !willTakeRisks && !QueryVisited(board[cellX + 1, cellY]))
            {
                if (agentJustSpawned)
                {
                    for (int i = 4; i > 0; i--)
                    {
                        if (numArrows > 0 && willAvoidWumpus)
                        {
                            Shoot();
                            turnRight();
                            forDirection++;
                            numArrows--;
                        }
                        switch (forDirection)
                        {
                            case 0:
                                lastCellDirection = Direction.East;
                                break;
                            case 1:
                                lastCellDirection = Direction.South;
                                break;
                            case 2:
                                lastCellDirection = Direction.West;
                                break;
                            case 3:
                                lastCellDirection = Direction.North;
                                break;
                            case 4:
                                lastCellDirection = Direction.East;
                                break;
                            default:
                                break;
                        }
                    }
                    forDirection = 0;
                    MoveEast();
                    UpdateCurrentCell();
                    stats.IncrementStat('A');
                    lastCellDirection = Direction.West;
                }
                else if (!agentJustSpawned && !willAvoidPits && !willAvoidWumpus)
                {
                    MoveEast();
                    UpdateCurrentCell();
                    stats.IncrementStat('A');
                    lastCellDirection = Direction.West;
                }
                else if (!agentJustSpawned && (willAvoidPits || willAvoidWumpus) && !isDesparate)
                {
                    for (int i = 4; i > 0; i--)
                    {
                        if (numArrows > 0 && willAvoidWumpus)
                        {
                            Shoot();
                            turnRight();
                            forDirection++;
                            numArrows--;
                        }
                        switch (forDirection)
                        {
                            case 0:
                                lastCellDirection = Direction.East;
                                break;
                            case 1:
                                lastCellDirection = Direction.South;
                                break;
                            case 2:
                                lastCellDirection = Direction.West;
                                break;
                            case 3:
                                lastCellDirection = Direction.North;
                                break;
                            case 4:
                                lastCellDirection = Direction.East;
                                break;
                            default:
                                break;
                        }
                    }
                    forDirection = 0;
                    MoveBack();
                    UpdateCurrentCell();
                    stats.IncrementStat('A');
                }
                else if (!agentJustSpawned && (willAvoidPits || willAvoidWumpus) && isDesparate)
                {
                    for (int i = 4; i > 0; i--)
                    {
                        if (numArrows > 0 && willAvoidWumpus)
                        {
                            Shoot();
                            turnRight();
                            forDirection++;
                            numArrows--;
                        }
                        switch (forDirection)
                        {
                            case 0:
                                lastCellDirection = Direction.East;
                                break;
                            case 1:
                                lastCellDirection = Direction.South;
                                break;
                            case 2:
                                lastCellDirection = Direction.West;
                                break;
                            case 3:
                                lastCellDirection = Direction.North;
                                break;
                            case 4:
                                lastCellDirection = Direction.East;
                                break;
                            default:
                                break;
                        }
                    }
                    forDirection = 0;
                    MoveEast();
                    UpdateCurrentCell();
                    stats.IncrementStat('A');
                    lastCellDirection = Direction.West;
                }
            }
            else if (IsMoveValid(cellX, cellY - 1)
                && !willTakeRisks && !QueryVisited(board[cellX, cellY - 1]))
            {
                if (agentJustSpawned)
                {
                    for (int i = 4; i > 0; i--)
                    {
                        if (numArrows > 0 && willAvoidWumpus)
                        {
                            Shoot();
                            turnRight();
                            forDirection++;
                            numArrows--;
                        }
                        switch (forDirection)
                        {
                            case 0:
                                lastCellDirection = Direction.South;
                                break;
                            case 1:
                                lastCellDirection = Direction.West;
                                break;
                            case 2:
                                lastCellDirection = Direction.North;
                                break;
                            case 3:
                                lastCellDirection = Direction.East;
                                break;
                            case 4:
                                lastCellDirection = Direction.South;
                                break;
                            default:
                                break;
                        }
                    }
                    forDirection = 0;
                    MoveSouth();
                    UpdateCurrentCell();
                    stats.IncrementStat('A');
                    lastCellDirection = Direction.North;
                }
                else if (!agentJustSpawned && !willAvoidPits && !willAvoidWumpus)
                {
                    MoveSouth();
                    UpdateCurrentCell();
                    stats.IncrementStat('A');
                    lastCellDirection = Direction.North;
                }
                else if (!agentJustSpawned && (willAvoidPits || willAvoidWumpus) && !isDesparate)
                {
                    for (int i = 4; i > 0; i--)
                    {
                        if (numArrows > 0 && willAvoidWumpus)
                        {
                            Shoot();
                            turnRight();
                            forDirection++;
                            numArrows--;
                        }
                        switch (forDirection)
                        {
                            case 0:
                                lastCellDirection = Direction.South;
                                break;
                            case 1:
                                lastCellDirection = Direction.West;
                                break;
                            case 2:
                                lastCellDirection = Direction.North;
                                break;
                            case 3:
                                lastCellDirection = Direction.East;
                                break;
                            case 4:
                                lastCellDirection = Direction.South;
                                break;
                            default:
                                break;
                        }
                    }
                    forDirection = 0;
                    MoveBack();
                    UpdateCurrentCell();
                    stats.IncrementStat('A');
                }
                else if (!agentJustSpawned && (willAvoidPits || willAvoidWumpus) && isDesparate)
                {
                    for (int i = 4; i > 0; i--)
                    {
                        if (numArrows > 0 && willAvoidWumpus)
                        {
                            Shoot();
                            turnRight();
                            forDirection++;
                            numArrows--;
                        }
                        switch (forDirection)
                        {
                            case 0:
                                lastCellDirection = Direction.South;
                                break;
                            case 1:
                                lastCellDirection = Direction.West;
                                break;
                            case 2:
                                lastCellDirection = Direction.North;
                                break;
                            case 3:
                                lastCellDirection = Direction.East;
                                break;
                            case 4:
                                lastCellDirection = Direction.South;
                                break;
                            default:
                                break;
                        }
                    }
                    forDirection = 0;
                    MoveSouth();
                    UpdateCurrentCell();
                    stats.IncrementStat('A');
                    lastCellDirection = Direction.North;
                }
            }
            else if (IsMoveValid(cellX - 1, cellY)
                && !willTakeRisks && !QueryVisited(board[cellX - 1, cellY]))
            {
                if (agentJustSpawned)
                {
                    for (int i = 4; i > 0; i--)
                    {
                        if (numArrows > 0 && willAvoidWumpus)
                        {
                            Shoot();
                            turnRight();
                            forDirection++;
                            numArrows--;
                        }
                        switch (forDirection)
                        {
                            case 0:
                                lastCellDirection = Direction.West;
                                break;
                            case 1:
                                lastCellDirection = Direction.North;
                                break;
                            case 2:
                                lastCellDirection = Direction.East;
                                break;
                            case 3:
                                lastCellDirection = Direction.South;
                                break;
                            case 4:
                                lastCellDirection = Direction.West;
                                break;
                            default:
                                break;
                        }
                    }
                    forDirection = 0;
                    MoveWest();
                    UpdateCurrentCell();
                    stats.IncrementStat('A');
                    lastCellDirection = Direction.East;
                }
                else if (!agentJustSpawned && !willAvoidPits && !willAvoidWumpus)
                {
                    MoveWest();
                    UpdateCurrentCell();
                    stats.IncrementStat('A');
                    lastCellDirection = Direction.East;
                }
                else if (!agentJustSpawned && (willAvoidPits || willAvoidWumpus) && !isDesparate)
                {
                    for (int i = 4; i > 0; i--)
                    {
                        if (numArrows > 0 && willAvoidWumpus)
                        {
                            Shoot();
                            turnRight();
                            forDirection++;
                            numArrows--;
                        }
                        switch (forDirection)
                        {
                            case 0:
                                lastCellDirection = Direction.West;
                                break;
                            case 1:
                                lastCellDirection = Direction.North;
                                break;
                            case 2:
                                lastCellDirection = Direction.East;
                                break;
                            case 3:
                                lastCellDirection = Direction.South;
                                break;
                            case 4:
                                lastCellDirection = Direction.West;
                                break;
                            default:
                                break;
                        }
                    }
                    forDirection = 0;
                    MoveBack();
                    UpdateCurrentCell();
                    stats.IncrementStat('A');
                }
                else if (!agentJustSpawned && (willAvoidPits || willAvoidWumpus) && isDesparate)
                {
                    for (int i = 4; i > 0; i--)
                    {
                        if (numArrows > 0 && willAvoidWumpus)
                        {
                            Shoot();
                            turnRight();
                            forDirection++;
                            numArrows--;
                        }
                        switch (forDirection)
                        {
                            case 0:
                                lastCellDirection = Direction.West;
                                break;
                            case 1:
                                lastCellDirection = Direction.North;
                                break;
                            case 2:
                                lastCellDirection = Direction.East;
                                break;
                            case 3:
                                lastCellDirection = Direction.South;
                                break;
                            case 4:
                                lastCellDirection = Direction.West;
                                break;
                            default:
                                break;
                        }
                    }
                    forDirection = 0;
                    MoveWest();
                    UpdateCurrentCell();
                    stats.IncrementStat('A');
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
                                stats.IncrementStat('A');
                                lastCellDirection = Direction.South;
                            }
                            break;
                        case 1:
                            if (IsMoveValid(cellX + 1, cellY))
                            {
                                decidedMove = true;
                                MoveEast();
                                UpdateCurrentCell();
                                stats.IncrementStat('A');
                                lastCellDirection = Direction.West;
                            }
                            break;
                        case 2:
                            if (IsMoveValid(cellX, cellY - 1))
                            {
                                decidedMove = true;
                                MoveSouth();
                                UpdateCurrentCell();
                                stats.IncrementStat('A');
                                lastCellDirection = Direction.North;
                            }
                            break;
                        case 3:
                            if (IsMoveValid(cellX - 1, cellY))
                            {
                                decidedMove = true;
                                MoveWest();
                                UpdateCurrentCell();
                                stats.IncrementStat('A');
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
                                stats.IncrementStat('A');
                                UpdateCurrentCell();
                                lastCellDirection = Direction.South;
                                DeathCheck();
                                if (currentCell.GetState() == State.Gold)
                                    inGoldCell = true;
                                else
                                {
                                    Console.WriteLine(board.ToString());
                                    MoveBack();
                                    UpdateCurrentCell();
                                    stats.IncrementStat('A');
                                    Console.WriteLine(board.ToString());
                                }
                            }
                            break;
                        case 1:
                            if (IsMoveValid(cellX + 1, cellY) && !QueryVisited(board[cellX + 1, cellY]))
                            {
                                MoveEast();
                                stats.IncrementStat('A');
                                UpdateCurrentCell();
                                lastCellDirection = Direction.West;
                                DeathCheck();
                                if (currentCell.GetState() == State.Gold)
                                    inGoldCell = true;
                                else
                                {
                                    Console.WriteLine(board.ToString());
                                    MoveBack();
                                    UpdateCurrentCell();
                                    stats.IncrementStat('A');
                                    Console.WriteLine(board.ToString());
                                }
                            }
                            break;
                        case 2:
                            if (IsMoveValid(cellX, cellY - 1) && !QueryVisited(board[cellX, cellY - 1]))
                            {
                                MoveSouth();
                                stats.IncrementStat('A');
                                UpdateCurrentCell();
                                lastCellDirection = Direction.North;
                                DeathCheck();
                                if (currentCell.GetState() == State.Gold)
                                    inGoldCell = true;
                                else
                                {
                                    Console.WriteLine(board.ToString());
                                    MoveBack();
                                    UpdateCurrentCell();
                                    stats.IncrementStat('A');
                                    Console.WriteLine(board.ToString());
                                }
                            }
                            break;
                        case 3:
                            if (IsMoveValid(cellX - 1, cellY) && !QueryVisited(board[cellX - 1, cellY]))
                            {
                                MoveWest();
                                stats.IncrementStat('A');
                                UpdateCurrentCell();
                                lastCellDirection = Direction.South;
                                DeathCheck();
                                if (currentCell.GetState() == State.Gold)
                                    inGoldCell = true;
                                else
                                {
                                    Console.WriteLine(board.ToString());
                                    MoveBack();
                                    UpdateCurrentCell();
                                    stats.IncrementStat('A');
                                    Console.WriteLine(board.ToString());
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
                stats.IncrementStat('D');
                stats.IncrementStat('W');
            }

            // If the current cell is a pit:
            // The agent dies
            if (currentCell.GetState() == State.Pit)
            {
                isDead = true;
                stats.IncrementStat('D');
                stats.IncrementStat('P');
            }
        }
    }
}