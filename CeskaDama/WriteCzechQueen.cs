namespace CzechQueen;

public static class WriteCzechQueen
{
    public static void GameBoard(Stone[,] gameBoard, bool colored = false, int coloredX = -1, int coloredY = -1)
    {
        Console.Clear();
        Console.Write("     Y Y Y Y Y Y Y Y \n");
        Console.Write("     0 1 2 3 4 5 6 7 \n");
        Console.Write("                     \n");

        for (int x = 0; x < gameBoard.GetLength(0); x++)
        {
            Console.Write($"X {x}  ");
            for (int y = 0; y < gameBoard.GetLength(1); y++)
            {
                if (colored && x == coloredX && y == coloredY)
                {
                    ColoredValue(gameBoard, x, y);
                }
                else
                {
                    switch (gameBoard[x, y].Color)
                    {
                        case Color.White when gameBoard[x, y].Queen:
                            Queen(gameBoard[x, y]);
                            break;
                        case Color.White:
                            Console.Write("B" + " ");
                            break;
                        case Color.Black when gameBoard[x, y].Queen:
                            Queen(gameBoard[x, y]);
                            break;
                        case Color.Black:
                            Console.Write("C" + " ");
                            break;
                        case Color.None:
                            Console.Write(" " + " ");
                            break;
                        default:
                            Console.Write(" " + " ");
                            break;
                    }
                }
            }

            Console.WriteLine();
        }
    }

    private static void Queen(Stone stone)
    {
        if (stone.Color == Color.White)
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("B");
            Console.ResetColor();
            Console.Write(" ");
        }
        else
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("C");
            Console.ResetColor();
            Console.Write(" ");
        }
    }

    private static void ColoredValue(Stone[,] gameBoard, int x, int y)
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(gameBoard[x, y].Color == Color.White ? "B" : "C");
        Console.ResetColor();
        Console.Write(" ");
    }

    public static void Error(string error) => Console.WriteLine(error);
    public static void WhoIsOnMove(string stoneColor) => Console.WriteLine($"Na tahu je {stoneColor} hrac.");
    public static void WrongMove() => Console.WriteLine("Neplatny tah!");

    public static void WhereMoveStone() =>
        Console.WriteLine("Zadej souradnice, kam chces kamen pohnout (x, y): ");

    public static void WhichStoneYouWantMove() =>
        Console.WriteLine("Zadej souradnice kamene, ktery chces pohnout (x, y): ");

    public static void CantMove() => Console.WriteLine("Nelze pohnout!");

    public static void CanMove() => Console.WriteLine("Slo pohnout!");

    public static void LosingStone(int x, int y) => Console.WriteLine($"Ztracis kamen na pozici {x},{y}!");

    public static void EndGame(Color color)
    {
        Console.WriteLine("Konec hry!");
        Console.WriteLine(color == Color.White ? "Vyhral bily hrac!" : "Vyhral cerny hrac!");
    }

    public static void StoneCount(int whiteStonesCount, int blackStonesCount)
    {
        Console.WriteLine($"Pocet bilych kamenu: {whiteStonesCount}");
        Console.WriteLine($"Pocet cernych kamenu: {blackStonesCount}");
    }
}