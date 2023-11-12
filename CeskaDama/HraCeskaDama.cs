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
                Console.Clear();
                VypisHerniDesku();
            }
            else
            {
                Console.WriteLine("Neplatny tah!");
            }
        }
    }

    private void OdeberKamen(int x, int y)
    {
        // pohnul se cerny, takze odebereme bily kamen
        if (HerniDeska[x, y] == "B")
        {
            PocetBilychKamenu--;
        }
        else
        {
            PocetCernychKamenu--;
        }
        
        Console.WriteLine($"XPocet bilych kamenu: {PocetBilychKamenu}");
        Console.WriteLine($"XPocet cernych kamenu: {PocetCernychKamenu}");
        Console.ReadKey();
    }

    private void PosunOJedno(int x, int y, int xChcesPohnout, int yChcesPohnout)
    {
        // posun dolu vlevo o jedno
        if (x - 1 == xChcesPohnout && y - 1 == yChcesPohnout)
        {
            HerniDeska[x, y] = " ";
        }

        // posun dolu vpravo o jedno
        if (x + 1 == xChcesPohnout && y - 1 == yChcesPohnout)
        {
            HerniDeska[x, y] = " ";
        }

        // posun nahoru vlevo o jedno
        if (x - 1 == xChcesPohnout && y + 1 == yChcesPohnout)
        {
            HerniDeska[x, y] = " ";
        }

        // posun nahoru vpravo o jedno
        if (x + 1 == xChcesPohnout && y + 1 == yChcesPohnout)
        {
            HerniDeska[x, y] = " ";
        }
    }

    private void PosunODva(int x, int y, int xChcesPohnout, int yChcesPohnout)
    {
       // posun dolu vlevo o dva
        if (x - 2 == xChcesPohnout && y - 2 == yChcesPohnout)
        {
            OdeberKamen(x - 1, y - 1); 
            HerniDeska[x, y] = " ";
            HerniDeska[x - 1, y - 1] = " ";
        }

        // posun dolu vpravo o dva
        if (x + 2 == xChcesPohnout && y - 2 == yChcesPohnout)
        {
            OdeberKamen(x + 1, y - 1);
            HerniDeska[x, y] = " ";
            HerniDeska[x + 1, y - 1] = " ";
        }

        // posun nahoru vlevo o dva
        if (x - 2 == xChcesPohnout && y + 2 == yChcesPohnout)
        {
            OdeberKamen(x - 1, y + 1);
            HerniDeska[x, y] = " ";
            HerniDeska[x - 1, y + 1] = " ";
            
        }

        // posun nahoru vpravo o dva
        if (x + 2 == xChcesPohnout && y + 2 == yChcesPohnout)
        {
            OdeberKamen(x + 1, y + 1);
            HerniDeska[x, y] = " ";
            HerniDeska[x + 1, y + 1] = " ";
        }
    }

    private void PohniKamen(int x, int y, int xChcesPohnout, int yChcesPohnout)
    {
        HerniDeska[xChcesPohnout, yChcesPohnout] = HerniDeska[x, y];

        PosunOJedno(x, y, xChcesPohnout, yChcesPohnout);
        PosunODva(x, y, xChcesPohnout, yChcesPohnout);
    }

    private bool KontrolaPohybuKamene(int x, int y, int xChcesPohnout, int yChcesPohnout, string barvaKamene)
    {
        // convert to if
        if (!KontrolaJeNaDesce(x, y))
        {
            Console.WriteLine($"Kamen {x},{y} neni na desce!");
            return false;
        }

        if (!KontrolaJeNaDesce(xChcesPohnout, yChcesPohnout))
        {
            Console.WriteLine($"Kamen {xChcesPohnout},{yChcesPohnout} neni na desce!");
            return false;
        }

        if (!KontrolaLzePohnout(x, y, xChcesPohnout, yChcesPohnout, barvaKamene))
        {
            Console.WriteLine($"Kamen {x},{y} nelze pohnout na {xChcesPohnout},{yChcesPohnout}!");
            return false;
        }

        if (!KontrolaDiaonalnihoPohybu(x, y, xChcesPohnout, yChcesPohnout, 1) &&
            !KontrolaDiaonalnihoPohybu(x, y, xChcesPohnout, yChcesPohnout, 2))
        {
            Console.WriteLine($"Kamen {x},{y} nelze pohnout diagonálně na {xChcesPohnout},{yChcesPohnout}!");
            return false;
        }

        return true;
    }

    private bool KontrolaDiaonalnihoPohybu(int x, int y, int xChcesPohnout, int yChcesPohnout, int posunO) =>
        x - posunO == xChcesPohnout && y - posunO == yChcesPohnout
        || x + posunO == xChcesPohnout && y + posunO == yChcesPohnout
        || x - posunO == xChcesPohnout && y + posunO == yChcesPohnout
        || x + posunO == xChcesPohnout && y - posunO == yChcesPohnout;


    private bool KontrolaLzePohnout(int x, int y, int xChcesPohnout, int yChcesPohnout, string barvaKamene) =>
        HerniDeska[x, y] != " " && HerniDeska[xChcesPohnout, yChcesPohnout] == " " && HerniDeska[x, y] == barvaKamene;


    private bool KontrolaJeNaDesce(int x, int y) =>
        x >= 0 && x <= HerniDeska.GetLength(0) && y >= 0 && y <= HerniDeska.GetLength(1);

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
        for (int x = 0; x < (HerniDeska.GetLength(0) / 2) - 1; x++)
        {
            for (int y = 0; y < HerniDeska.GetLength(1); y++)
            {
                if ((x + y) % 2 == 0)
                {
                    HerniDeska[x, y] = znak;
                }
                else
                {
                    HerniDeska[x, y] = " ";
                }
            }
        }
    }

    private void NastavCernePolicka()
    {
        string znak = "C";

        // druha pulka desky (cerna policka)
        for (int x = (HerniDeska.GetLength(0) / 2) + 1; x < HerniDeska.GetLength(0); x++)
        {
            for (int y = 0; y < HerniDeska.GetLength(1); y++)
            {
                if ((x + y) % 2 != 0)
                {
                    HerniDeska[x, y] = " ";
                }
                else
                {
                    HerniDeska[x, y] = znak;
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
        for (int x = 3; x < 5; x++)
        {
            for (int y = 0; y < HerniDeska.GetLength(1); y++)
            {
                HerniDeska[x, y] = " ";
            }
        }
    }

    private void VypisHerniDesku(bool barevne = false, int barevneX = -1, int barevneY = -1)
    {
        Console.Clear();
        Console.Write("    Y Y Y Y Y Y Y Y\n");
        Console.Write("    0 1 2 3 4 5 6 7\n");
        
        for (int x = 0; x < HerniDeska.GetLength(0); x++)
        {
            Console.Write($"X {x} ");
            for (int y = 0; y < HerniDeska.GetLength(1); y++)
            {
                if (barevne && x == barevneX && y == barevneY)
                {
                    VypisHodnotuDesky(x, y);
                }
                else
                {
                    Console.Write(HerniDeska[x, y] + " ");
                }
            }

            Console.WriteLine();
        }
    }

    private void VypisHodnotuDesky(int x, int y)
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(HerniDeska[x, y] + " ");
        Console.ResetColor();
    }
}