namespace CzechQueen;

public class GameCzechQueen
{
    private Stone[,] GameBoard { get; set; } = new Stone[8, 8];
    private bool GameEnd { get; set; } = false;
    private int WhiteStonesCount { get; set; } = 12;
    private int BlackStonesCount { get; set; } = 12;
    
    public void StartGame()
    {
        Console.Clear();
        GameEnd = false;
        NastavHerniDesku();
        WriteCzechQueen.GameBoard(GameBoard);
        GameLoop();
    }

    private void GameLoop()
    {
        Color stoneColor = Color.Black;

        while (!GameEnd)
        {
            stoneColor = stoneColor == Color.White ? Color.Black : Color.White;
            WriteCzechQueen.WhoIsOnMove(stoneColor == Color.White ? "B" : "C");

            WriteCzechQueen.WhichStoneYouWantMove();
            string coordinatesStones = Console.ReadLine();

            int x = int.Parse(coordinatesStones.Split(",")[0]);
            int y = int.Parse(coordinatesStones.Split(",")[1]);

            WriteCzechQueen.GameBoard(GameBoard, true, x, y);

            WriteCzechQueen.WhereMoveStone();
            string coordinatesStonesWhereMove = Console.ReadLine();

            int xWantMove = int.Parse(coordinatesStonesWhereMove.Split(",")[0]);
            int yWantMove = int.Parse(coordinatesStonesWhereMove.Split(",")[1]);

            if (CheckEndGame())
            {
                break;
            }

            if (CheckStoneMove(x, y, xWantMove, yWantMove, stoneColor))
            {
                MoveStone(x, y, xWantMove, yWantMove, stoneColor);
                Console.Clear();
                WriteCzechQueen.GameBoard(GameBoard);
            }
            else
            {
                WriteCzechQueen.WrongMove();
            }
        }
    }

    private void RemoveStoneCount(int x, int y)
    {
        if (GameBoard[x, y].Color == Color.White)
        {
            WhiteStonesCount--;
        }
        else
        {
            BlackStonesCount--;
        }

        WriteCzechQueen.StoneCount(WhiteStonesCount, BlackStonesCount);
        Thread.Sleep(1000);
    }

    private void MoveStone(int x, int y, int xWantMove, int yWantMove, Color barvaKamene)
    {
        CzechRequiredJump(x, y, xWantMove, barvaKamene);

        if (CheckIsQueen(x, y))
        {
            bool moveInRange = MoveInRange(x, y, xWantMove, yWantMove);

            if (moveInRange)
            {
                return;
            }

            WriteCzechQueen.CantMove();
        }

        bool moveByOne = MoveByOne(x, y, xWantMove, yWantMove);

        if (moveByOne)
        {
            if (CheckQueenChange(xWantMove, barvaKamene))
            {
                QueenChange(xWantMove, yWantMove);
            }

            return;
        }

        bool moveByTwo = MoveByTwo(x, y, xWantMove, yWantMove);

        if (moveByTwo)
        {
            if (CheckQueenChange(xWantMove, barvaKamene))
            {
                QueenChange(xWantMove, yWantMove);
            }

            return;
        }

        WriteCzechQueen.CantMove();
    }

    private void CzechRequiredJump(int x, int y, int xWantMove, Color barvaKamene)
    {
        bool canJumpOnTwo = false;
        Color oppositeColor = barvaKamene == Color.White ? Color.Black : Color.White;
        int oldX = x;
        int oldY = y;

        if (Math.Max(x, xWantMove) - Math.Min(x, xWantMove) == 1)
        {
            for (x = 0; x < GameBoard.GetLength(0); x++)
            {
                for (y = 0; y < GameBoard.GetLength(1); y++)
                {
                    if (y > 2 && x < GameBoard.GetLength(0) - 2)
                    {
                        canJumpOnTwo = GameBoard[x + 2, y - 2].Color == Color.None &&
                                       GameBoard[x + 1, y - 1].Color == oppositeColor &&
                                       GameBoard[x, y].Color != oppositeColor &&
                                       GameBoard[x, y].Color != Color.None;
                    }

                    if (!canJumpOnTwo && y < GameBoard.GetLength(1) - 2 && x < GameBoard.GetLength(0) - 2)
                    {
                        canJumpOnTwo = GameBoard[x + 2, y + 2].Color == Color.None &&
                                       GameBoard[x + 1, y + 1].Color == oppositeColor &&
                                       GameBoard[x, y].Color != oppositeColor &&
                                       GameBoard[x, y].Color != Color.None;
                    }

                    if (!canJumpOnTwo && y < GameBoard.GetLength(0) - 2 && x > 2)
                    {
                        canJumpOnTwo = GameBoard[x - 2, y + 2].Color == Color.None &&
                                       GameBoard[x - 1, y + 1].Color == oppositeColor &&
                                       GameBoard[x, y].Color != oppositeColor &&
                                       GameBoard[x, y].Color != Color.None;
                    }

                    if (!canJumpOnTwo && y > 2 && x > 2)
                    {
                        canJumpOnTwo = GameBoard[x - 2, y - 2].Color == Color.None &&
                                       GameBoard[x - 1, y - 1].Color == oppositeColor &&
                                       GameBoard[x, y].Color != oppositeColor &&
                                       GameBoard[x, y].Color != Color.None;
                    }

                    if (canJumpOnTwo)
                    {
                        WriteCzechQueen.CanMove();
                        WriteCzechQueen.LosingStone(oldX, oldY);
                        RemoveStoneCount(oldX, oldY);
                        SetStoneToNone(ref GameBoard[oldX, oldY]);
                        Thread.Sleep(300);
                        return;
                    }
                }
            }
        }
    }

    private void QueenChange(int xWantMove, int yWantMove) =>
        GameBoard[xWantMove, yWantMove].Queen = true;

    private void StoneSwap(int x, int y, int xWantMove, int yWantMove)
    {
        GameBoard[xWantMove, yWantMove].Color = GameBoard[x, y].Color;
        GameBoard[xWantMove, yWantMove].Queen = GameBoard[x, y].Queen;
        SetStoneToNone(ref GameBoard[x, y]);
    }

    private bool MoveByOne(int x, int y, int xWantMove, int yWantMove)
    {
        switch (GameBoard[x, y].Color)
        {
            case Color.White:
            {
                if (x + 1 == xWantMove && y - 1 == yWantMove)
                {
                    StoneSwap(x, y, xWantMove, yWantMove);
                    return true;
                }

                if (x + 1 == xWantMove && y + 1 == yWantMove)
                {
                    StoneSwap(x, y, xWantMove, yWantMove);
                    return true;
                }

                return false;
            }
            case Color.Black:
            {
                if (x - 1 == xWantMove && y + 1 == yWantMove)
                {
                    StoneSwap(x, y, xWantMove, yWantMove);
                    return true;
                }

                if (x - 1 == xWantMove && y - 1 == yWantMove)
                {
                    StoneSwap(x, y, xWantMove, yWantMove);
                    return true;
                }

                return false;
            }
            default:
                return false;
        }
    }

    private bool MoveByTwo(int x, int y, int xWantMove, int yWantMove)
    {
        switch (GameBoard[x, y].Color)
        {
            case Color.White:
            {
                if (x + 2 == xWantMove && y - 2 == yWantMove)
                {
                    RemoveStoneCount(x + 1, y - 1);
                    StoneSwap(x, y, xWantMove, yWantMove);
                    SetStoneToNone(ref GameBoard[x + 1, y - 1]);
                    return true;
                }

                if (x + 2 == xWantMove && y + 2 == yWantMove)
                {
                    RemoveStoneCount(x + 1, y + 1);
                    StoneSwap(x, y, xWantMove, yWantMove);
                    SetStoneToNone(ref GameBoard[x + 1, y + 1]);
                    return true;
                }

                return false;
            }
            case Color.Black:
            {
                if (x - 2 == xWantMove && y + 2 == yWantMove)
                {
                    RemoveStoneCount(x - 1, y + 1);
                    StoneSwap(x, y, xWantMove, yWantMove);
                    SetStoneToNone(ref GameBoard[x - 1, y + 1]);
                    return true;
                }

                if (x - 2 == xWantMove && y - 2 == yWantMove)
                {
                    RemoveStoneCount(x - 1, y - 1);
                    StoneSwap(x, y, xWantMove, yWantMove);
                    SetStoneToNone(ref GameBoard[x - 1, y - 1]);
                    return true;
                }


                return false;
            }
            default:
                return false;
        }
    }

    // --- KONTROLA POHYBU ---

    private bool CheckStoneMove(int x, int y, int xWantMove, int yWantMove, Color stoneColor)
    {
        if (!CheckIsOnBoard(x, y))
        {
            WriteCzechQueen.Error($"Kamen {x},{y} neni na desce!");
            return false;
        }

        if (!CheckIsOnBoard(xWantMove, yWantMove))
        {
            WriteCzechQueen.Error($"Kamen {xWantMove},{yWantMove} neni na desce!");
            return false;
        }

        if (!CheckCanMove(x, y, xWantMove, yWantMove, stoneColor))
        {
            WriteCzechQueen.Error($"Kamen {x},{y} nelze pohnout na {xWantMove},{yWantMove}!");
            return false;
        }

        if (CheckIsQueen(x, y))
        {
            return MoveInRange(x, y, xWantMove, yWantMove);
        }

        if (!CheckDiagonalMove(x, y, xWantMove, yWantMove, 1) &&
            !CheckDiagonalMove(x, y, xWantMove, yWantMove, 2))
        {
            WriteCzechQueen.Error($"Kamen {x},{y} nelze pohnout diagonálně na {xWantMove},{yWantMove}!");
            return false;
        }

        return true;
    }

    private bool PosunVeSmeruARozsahu(int move, Directions direction, int x, int y)
    {
        Color lastColor = Color.None;

        for (int i = 1; i < move; i++)
        {
            if (lastColor is Color.White or Color.Black)
            {
                WriteCzechQueen.Error("Nelze přeskočit více kamenů!");
                return false;
            }

            switch (direction)
            {
                case Directions.LeftUp:
                    lastColor = GameBoard[x - i, y - i].Color;

                    if (GameBoard[x - i, y - i].Color != Color.None
                        && GameBoard[x - i, y - i].Color == GameBoard[x, y].Color)
                    {
                        WriteCzechQueen.Error("Nelze přeskočit kámen stejné barvy!");
                        return false;
                    }

                    if (GameBoard[x - i, y - i].Color != Color.None)
                    {
                        RemoveStoneCount(x - i, y - i);
                        SetStoneToNone(ref GameBoard[x - i, y - i]);
                    }

                    break;

                case Directions.RightUp:
                    lastColor = GameBoard[x - i, y + i].Color;

                    if (GameBoard[x - i, y + i].Color != Color.None
                        && GameBoard[x - i, y + i].Color == GameBoard[x, y].Color)
                    {
                        WriteCzechQueen.Error("Nelze přeskočit kámen stejné barvy!");
                        return false;
                    }

                    if (GameBoard[x - i, y + i].Color != Color.None)
                    {
                        RemoveStoneCount(x - i, y + i);
                        SetStoneToNone(ref GameBoard[x - i, y + i]);
                    }

                    break;

                case Directions.LeftDown:
                    lastColor = GameBoard[x + i, y - i].Color;

                    if (GameBoard[x + i, y - i].Color != Color.None
                        && GameBoard[x + i, y - i].Color == GameBoard[x, y].Color)
                    {
                        WriteCzechQueen.Error("Nelze přeskočit kámen stejné barvy!");
                        return false;
                    }

                    if (GameBoard[x + i, y - i].Color != Color.None)
                    {
                        RemoveStoneCount(x + i, y - i);
                        SetStoneToNone(ref GameBoard[x + i, y - i]);
                    }

                    break;

                case Directions.RightDown:
                    lastColor = GameBoard[x + i, y + i].Color;

                    if (GameBoard[x + i, y + i].Color != Color.None
                        && GameBoard[x + i, y + i].Color == GameBoard[x, y].Color)
                    {
                        WriteCzechQueen.Error("Nelze přeskočit kámen stejné barvy!");
                        return false;
                    }

                    if (GameBoard[x + i, y + i].Color != Color.None)
                    {
                        RemoveStoneCount(x + i, y + i);
                        SetStoneToNone(ref GameBoard[x + i, y + i]);
                    }

                    break;
            }
        }

        return true;
    }

    private bool MoveInRange(int x, int y, int xWantMove, int yWantMove)
    {
        if (xWantMove < x && yWantMove < y)
        {
            StoneSwap(x, y, xWantMove, yWantMove);
            return PosunVeSmeruARozsahu(x - xWantMove, Directions.LeftUp, x, y);
        }

        if (xWantMove < x && yWantMove > y)
        {
            StoneSwap(x, y, xWantMove, yWantMove);
            return PosunVeSmeruARozsahu(x - xWantMove, Directions.RightUp, x, y);
        }

        if (xWantMove > x && yWantMove < y)
        {
            StoneSwap(x, y, xWantMove, yWantMove);
            return PosunVeSmeruARozsahu(xWantMove - x, Directions.LeftDown, x, y);
        }

        if (xWantMove > x && yWantMove > y)
        {
            StoneSwap(x, y, xWantMove, yWantMove);
            return PosunVeSmeruARozsahu(xWantMove - x, Directions.RightDown, x, y);
        }

        return false;
    }

    private bool CheckQueenChange(int xWantMove, Color stoneColor)
    {
        switch (stoneColor)
        {
            case Color.White:
                return xWantMove == GameBoard.GetLength(0) - 1;
            case Color.Black:
                return xWantMove == 0;
            default:
                return false;
        }
    }

    private bool CheckDiagonalMove(int x, int y, int xWantMove, int yWantMove, int moveOf) =>
        x - moveOf == xWantMove && y - moveOf == yWantMove
        || x + moveOf == xWantMove && y + moveOf == yWantMove
        || x - moveOf == xWantMove && y + moveOf == yWantMove
        || x + moveOf == xWantMove && y - moveOf == yWantMove;

    private bool CheckCanMove(int x, int y, int xWantMove, int yWantMove, Color stoneColor) =>
        GameBoard[x, y].Color != Color.None && GameBoard[xWantMove, yWantMove].Color == Color.None &&
        GameBoard[x, y].Color == stoneColor;

    private bool CheckIsOnBoard(int x, int y) =>
        x >= 0 && x <= GameBoard.GetLength(0) && y >= 0 && y <= GameBoard.GetLength(1);

    private bool CheckIsQueen(int x, int y) => GameBoard[x, y].Queen;

    // --- HERNI DESKA ---

    private void NastavHerniDesku()
    {
        SetWhiteStones();
        SetMiddleBoard();
        SetBlackStones();
    }

    private void SetWhiteStones()
    {
        for (int x = 0; x < (GameBoard.GetLength(0) / 2) - 1; x++)
        {
            for (int y = 0; y < GameBoard.GetLength(1); y++)
            {
                if ((x + y) % 2 == 0)
                {
                    Stone whiteStone = new(Color.White);
                    GameBoard[x, y] = whiteStone;
                }
                else
                {
                    Stone noneStone = new(Color.None);
                    GameBoard[x, y] = noneStone;
                }
            }
        }
    }

    private void SetBlackStones()
    {
        for (int x = (GameBoard.GetLength(0) / 2) + 1; x < GameBoard.GetLength(0); x++)
        {
            for (int y = 0; y < GameBoard.GetLength(1); y++)
            {
                if ((x + y) % 2 != 0)
                {
                    Stone noneStone = new(Color.None);
                    GameBoard[x, y] = noneStone;
                }
                else
                {
                    Stone blackStone = new(Color.Black);
                    GameBoard[x, y] = blackStone;
                }
            }
        }
    }

    private void SetMiddleBoard()
    {
        for (int x = 3; x < 5; x++)
        {
            for (int y = 0; y < GameBoard.GetLength(1); y++)
            {
                Stone noneStone = new(Color.None);
                GameBoard[x, y] = noneStone;
            }
        }
    }

    // --- KONEC HRY ---
    private bool CheckEndGame()
    {
        // TODO: zkotrolovat nemůže provést svými kameny žádný tah.
        if (BlackStonesCount == 0)
        {
            EndGame(Color.White);
            return true;
        }

        if (WhiteStonesCount == 0)
        {
            EndGame(Color.Black);
            return true;
        }

        return false;
    }

    private void EndGame(Color color)
    {
        GameEnd = true;
        ResetStonesCount();

        WriteCzechQueen.EndGame(color);
    }

    private void ResetStonesCount()
    {
        WhiteStonesCount = 0;
        BlackStonesCount = 0;
    }

    private void SetStoneToNone(ref Stone stone)
    {
        stone.Color = Color.None;
        stone.Queen = false;
    }
}