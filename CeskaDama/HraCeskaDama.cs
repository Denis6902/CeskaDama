namespace CeskaDama;

public class HraCeskaDama
{
    private string[,] HerniDeska { get; set; } = new string[8, 8];
    private bool HraSkoncila { get; set; } = false;
    private int PocetBilychKamenu { get; set; } = 12;
    private int PocetCernychKamenu { get; set; } = 12;

    private void ResetPocetKamenu()
    {
        PocetBilychKamenu = 0;
        PocetCernychKamenu = 0;
    }

    public void ZacitHru()
    {
        Console.Clear();
        Console.WriteLine("Zaciname hru Ceska dama!");
        HraSkoncila = false;
        NastavHerniDesku();
        VypisHerniDesku();
        HerniSmycka();
    }

    private void HerniSmycka()
    {
        string barvaKamene = "C";

        while (!HraSkoncila)
        {
            barvaKamene = barvaKamene == "B" ? "C" : "B";
            Console.WriteLine($"Na tahu je {barvaKamene} hrac.");

            Console.WriteLine("Zadej souradnice kamene, ktery chces pohnout (x, y): ");
            string souradniceKamene = Console.ReadLine();

            int x = int.Parse(souradniceKamene.Split(",")[0]);
            int y = int.Parse(souradniceKamene.Split(",")[1]);

            VypisHerniDesku(true, x, y);

            Console.WriteLine("Zadej souradnice, kam chces kamen pohnout (x, y): ");
            string souradniceKameneChcesPohnout = Console.ReadLine();

            int xChcesPohnout = int.Parse(souradniceKameneChcesPohnout.Split(",")[0]);
            int yChcesPohnout = int.Parse(souradniceKameneChcesPohnout.Split(",")[1]);

            if (KontrolaKonceHry())
            {
                break;
            }

            if (KontrolaPohybuKamene(x, y, xChcesPohnout, yChcesPohnout, barvaKamene))
            {
                PohniKamen(x, y, xChcesPohnout, yChcesPohnout);
                OdeberKamen(x, y);
                Console.Clear();
                VypisHerniDesku();
            }
            else
            {
                Console.WriteLine("Neplatny tah!");
            }
        }
    }

    private void OdeberKamen(int i, int y)
    {
        // pohnul se cerny, takze odebereme bily kamen
        if (HerniDeska[i, y] == "B")
        {
            PocetCernychKamenu--;
        }
        else
        {
            PocetBilychKamenu--;
        }
    }

    private void PohniKamen(int i, int y, int xChcesPohnout, int yChcesPohnout)
    {
        HerniDeska[xChcesPohnout, yChcesPohnout] = HerniDeska[i, y];

        // skok diagonálně
        if (Math.Abs(i - xChcesPohnout) == 2 && Math.Abs(y - yChcesPohnout) == 2)
        {
            HerniDeska[i, y] = " ";
            return;
        }

        HerniDeska[i, y] = " ";
    }

    private bool KontrolaPohybuKamene(int i, int y, int xChcesPohnout, int yChcesPohnout, string barvaKamene)
    {
        // convert to if
        if (!KontrolaJeNaDesce(i, y))
        {
            Console.WriteLine($"Kamen {i},{y} neni na desce!");
            return false;
        }

        if (!KontrolaJeNaDesce(xChcesPohnout, yChcesPohnout))
        {
            Console.WriteLine($"Kamen {xChcesPohnout},{yChcesPohnout} neni na desce!");
            return false;
        }

        if (!KontrolaLzePohnout(i, y, xChcesPohnout, yChcesPohnout, barvaKamene))
        {
            Console.WriteLine($"Kamen {i},{y} nelze pohnout na {xChcesPohnout},{yChcesPohnout}!");
            return false;
        }

        if (!KontrolaDiaonalnihoPohybu(i, y, xChcesPohnout, yChcesPohnout, 1) &&
            !KontrolaDiaonalnihoPohybu(i, y, xChcesPohnout, yChcesPohnout, 2))
        {
            Console.WriteLine($"Kamen {i},{y} nelze pohnout diagonálně na {xChcesPohnout},{yChcesPohnout}!");
            return false;
        }

        return true;
    }

    private bool KontrolaDiaonalnihoPohybu(int i, int y, int xChcesPohnout, int yChcesPohnout, int posunO) =>
        i - posunO == xChcesPohnout && y - posunO == yChcesPohnout
        || i + posunO == xChcesPohnout && y + posunO == yChcesPohnout
        || i - posunO == xChcesPohnout && y + posunO == yChcesPohnout
        || i + posunO == xChcesPohnout && y - posunO == yChcesPohnout;


    private bool KontrolaLzePohnout(int i, int y, int xChcesPohnout, int yChcesPohnout, string barvaKamene) =>
        HerniDeska[i, y] != " " && HerniDeska[xChcesPohnout, yChcesPohnout] == " " && HerniDeska[i, y] == barvaKamene;


    private bool KontrolaJeNaDesce(int i, int y) =>
        i >= 0 && i <= HerniDeska.GetLength(0) && y >= 0 && y <= HerniDeska.GetLength(1);

    private bool KontrolaKonceHry()
    {
        // TODO: zkotrolovat nemůže provést svými kameny žádný tah.
        if (PocetCernychKamenu == 0)
        {
            KonecHry("bily");
            return true;
        }

        if (PocetBilychKamenu == 0)
        {
            KonecHry("cerny");
            return true;
        }

        return false;
    }

    private void KonecHry(string kdoVyhral)
    {
        Console.WriteLine("Konec hry!");
        HraSkoncila = true;
        ResetPocetKamenu();


        Console.WriteLine(kdoVyhral.ToLower() == "bily" ? "Vyhral bily hrac!" : "Vyhral cerny hrac!");
    }

    private void NastavBilePolicka()
    {
        string znak = "B";

        // prvni pulka desky (bile policka)
        for (int i = 0; i < (HerniDeska.GetLength(0) / 2) - 1; i++)
        {
            for (int j = 0; j < HerniDeska.GetLength(1); j++)
            {
                if ((i + j) % 2 == 0)
                {
                    HerniDeska[i, j] = znak;
                }
                else
                {
                    HerniDeska[i, j] = " ";
                }
            }
        }
    }

    private void NastavCernePolicka()
    {
        string znak = "C";

        // druha pulka desky (cerna policka)
        for (int i = (HerniDeska.GetLength(0) / 2) + 1; i < HerniDeska.GetLength(0); i++)
        {
            for (int j = 0; j < HerniDeska.GetLength(1); j++)
            {
                if ((i + j) % 2 != 0)
                {
                    HerniDeska[i, j] = " ";
                }
                else
                {
                    HerniDeska[i, j] = znak;
                }
            }
        }
    }

    private void NastavHerniDesku()
    {
        NastavBilePolicka();
        NastavStredDesky();
        NastavCernePolicka();
    }

    private void NastavStredDesky()
    {
        for (int i = 3; i < 5; i++)
        {
            for (int j = 0; j < HerniDeska.GetLength(1); j++)
            {
                HerniDeska[i, j] = " ";
            }
        }
    }

    private void VypisHerniDesku(bool barevne = false, int barevneX = -1, int barevneY = -1)
    {
        Console.Clear();

        for (int i = 0; i < HerniDeska.GetLength(0); i++)
        {
            for (int j = 0; j < HerniDeska.GetLength(1); j++)
            {
                if (barevne && i == barevneX && j == barevneY)
                {
                    VypisHodnotuDesky(i, j);
                }
                else
                {
                    Console.Write(HerniDeska[i, j] + " ");
                }
            }

            Console.WriteLine();
        }

        Console.WriteLine("-------------------");
        Console.Write("   Y Y Y Y Y Y Y Y\n");
        Console.Write("   0 1 2 3 4 5 6 7\n");
        Console.WriteLine("X 0");
        Console.WriteLine("X 1");
        Console.WriteLine("X 2");
        Console.WriteLine("X 3");
        Console.WriteLine("X 4");
        Console.WriteLine("X 5");
        Console.WriteLine("X 6");
        Console.WriteLine("X 7");
    }

    private void VypisHodnotuDesky(int x, int y)
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(HerniDeska[x, y] + " ");
        Console.ResetColor();
    }
}