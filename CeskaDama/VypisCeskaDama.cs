namespace CeskaDama;

public static class VypisCeskaDama
{
    public static void VypisHerniDesku(string[,] herniDeska, bool barevne = false, int barevneX = -1, int barevneY = -1)
    {
        Console.Clear();
        Console.Write("     Y Y Y Y Y Y Y Y \n");
        Console.Write("     0 1 2 3 4 5 6 7 \n");
        Console.Write("                     \n");

        for (int x = 0; x < herniDeska.GetLength(0); x++)
        {
            Console.Write($"X {x}  ");
            for (int y = 0; y < herniDeska.GetLength(1); y++)
            {
                if (barevne && x == barevneX && y == barevneY)
                {
                    VypisBarevneHodnotuDesky(herniDeska, x, y);
                }
                else
                {
                    Console.Write(herniDeska[x, y] + " ");
                }
            }

            Console.WriteLine();
        }
    }
    
    public static void VypisChybu(string chyba) => Console.WriteLine(chyba);
    public static void VypisKdoJeNaTahu(string barvaKamene) => Console.WriteLine($"Na tahu je {barvaKamene} hrac.");
    public static void VypisNeplatnyTah() => Console.WriteLine("Neplatny tah!");
    public static void VypisKamPosunoutKamen() => Console.WriteLine("Zadej souradnice, kam chces kamen pohnout (x, y): ");
    public static void VypisKteryKamenChcesPohnout() => Console.WriteLine("Zadej souradnice kamene, ktery chces pohnout (x, y): ");
    public static void VypisNelzePohnout() => Console.WriteLine("Nelze pohnout!");
    
    private static void VypisBarevneHodnotuDesky(string[,] herniDeska, int x, int y)
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(herniDeska[x, y] + " ");
        Console.ResetColor();
    }

    public static void VypisKonecHry(string kdoVyhral)
    {
        Console.WriteLine("Konec hry!");
        Console.WriteLine(kdoVyhral.ToLower() == "bily" ? "Vyhral bily hrac!" : "Vyhral cerny hrac!");
    }
    
    public static void VypisPocetKamenu(int pocetBilychKamenu, int pocetCernychKamenu)
    {
        Console.WriteLine($"Pocet bilych kamenu: {pocetBilychKamenu}");
        Console.WriteLine($"Pocet cernych kamenu: {pocetCernychKamenu}");
    }
}