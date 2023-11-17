namespace CeskaDama;

public class HraCeskaDama
{
    private string[,] HerniDeska { get; set; } = new string[8, 8];
    private bool HraSkoncila { get; set; } = false;
    private int PocetBilychKamenu { get; set; } = 12;
    private int PocetCernychKamenu { get; set; } = 12;

    public void ZacitHru()
    {
        Console.Clear();
        HraSkoncila = false;
        NastavHerniDesku();
        VypisCeskaDama.VypisHerniDesku(HerniDeska);
        HerniSmycka();
    }

    private void HerniSmycka()
    {
        string barvaKamene = "C";

        while (!HraSkoncila)
        {
            barvaKamene = barvaKamene == "B" ? "C" : "B";
            VypisCeskaDama.VypisKdoJeNaTahu(barvaKamene);

            VypisCeskaDama.VypisKteryKamenChcesPohnout();
            string souradniceKamene = Console.ReadLine();

            int x = int.Parse(souradniceKamene.Split(",")[0]);
            int y = int.Parse(souradniceKamene.Split(",")[1]);

            VypisCeskaDama.VypisHerniDesku(HerniDeska, true, x, y);

            VypisCeskaDama.VypisKamPosunoutKamen();
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
                VypisCeskaDama.VypisHerniDesku(HerniDeska);
            }
            else
            {
                VypisCeskaDama.VypisNeplatnyTah();
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

        VypisCeskaDama.VypisPocetKamenu(PocetBilychKamenu, PocetCernychKamenu);
        Console.ReadKey();
    }

    private void PohniKamen(int x, int y, int xChcesPohnout, int yChcesPohnout)
    {
        bool posunutoOJedno = PosunOJedno(x, y, xChcesPohnout, yChcesPohnout);

        if (posunutoOJedno)
        {
            return;
        }

        bool posunutoODva = PosunODva(x, y, xChcesPohnout, yChcesPohnout);

        if (!posunutoODva)
        {
            VypisCeskaDama.VypisNelzePohnout();
        }
    }

    private bool PosunOJedno(int x, int y, int xChcesPohnout, int yChcesPohnout)
    {
        switch (HerniDeska[x, y])
        {
            case "B":
            {
                // posun dolu vlevo o jedno
                if (x + 1 == xChcesPohnout && y - 1 == yChcesPohnout)
                {
                    HerniDeska[xChcesPohnout, yChcesPohnout] = HerniDeska[x, y];
                    HerniDeska[x, y] = " ";
                    return true;
                }

                // posun dolu vpravo o jedno
                if (x + 1 == xChcesPohnout && y + 1 == yChcesPohnout)
                {
                    HerniDeska[xChcesPohnout, yChcesPohnout] = HerniDeska[x, y];
                    HerniDeska[x, y] = " ";
                    return true;
                }

                return false;
            }
            case "C":
            {
                // posun nahoru vpravo o jedno
                if (x - 1 == xChcesPohnout && y + 1 == yChcesPohnout)
                {
                    HerniDeska[xChcesPohnout, yChcesPohnout] = HerniDeska[x, y];
                    HerniDeska[x, y] = " ";
                    return true;
                }

                // posun nahoru vlevo o jedno
                if (x - 1 == xChcesPohnout && y - 1 == yChcesPohnout)
                {
                    HerniDeska[xChcesPohnout, yChcesPohnout] = HerniDeska[x, y];
                    HerniDeska[x, y] = " ";
                    return true;
                }

                return false;
            }
            default:
                return false;
        }
    }

    private bool PosunODva(int x, int y, int xChcesPohnout, int yChcesPohnout)
    {
        switch (HerniDeska[x, y])
        {
            case "B":
            {
                // posun dolu vlevo o dva
                if (x + 2 == xChcesPohnout && y - 2 == yChcesPohnout)
                {
                    OdeberKamen(x + 1, y - 1);
                    HerniDeska[xChcesPohnout, yChcesPohnout] = HerniDeska[x, y];
                    HerniDeska[x, y] = " ";
                    HerniDeska[x + 1, y - 1] = " ";
                }

                // posun dolu vpravo o dva
                if (x + 2 == xChcesPohnout && y + 2 == yChcesPohnout)
                {
                    OdeberKamen(x + 1, y + 1);
                    HerniDeska[xChcesPohnout, yChcesPohnout] = HerniDeska[x, y];
                    HerniDeska[x, y] = " ";
                    HerniDeska[x + 1, y + 1] = " ";
                }

                return true;
            }
            case "C":
            {
                // posun nahoru vlevo o dva
                if (x - 2 == xChcesPohnout && y - 2 == yChcesPohnout)
                {
                    OdeberKamen(x - 1, y - 1);
                    HerniDeska[xChcesPohnout, yChcesPohnout] = HerniDeska[x, y];
                    HerniDeska[x, y] = " ";
                    HerniDeska[x - 1, y - 1] = " ";
                }

                // posun nahoru vpravo o dva
                if (x - 2 == xChcesPohnout && y + 2 == yChcesPohnout)
                {
                    OdeberKamen(x - 1, y + 1);
                    HerniDeska[xChcesPohnout, yChcesPohnout] = HerniDeska[x, y];
                    HerniDeska[x, y] = " ";
                    HerniDeska[x - 1, y + 1] = " ";
                }

                return true;
            }
            default:
                return false;
        }
    }

    private bool KontrolaPohybuKamene(int x, int y, int xChcesPohnout, int yChcesPohnout, string barvaKamene)
    {
        if (!KontrolaJeNaDesce(x, y))
        {
            VypisCeskaDama.VypisChybu($"Kamen {x},{y} neni na desce!");
            return false;
        }

        if (!KontrolaJeNaDesce(xChcesPohnout, yChcesPohnout))
        {
            VypisCeskaDama.VypisChybu($"Kamen {xChcesPohnout},{yChcesPohnout} neni na desce!");
            return false;
        }

        if (!KontrolaLzePohnout(x, y, xChcesPohnout, yChcesPohnout, barvaKamene))
        {
            VypisCeskaDama.VypisChybu($"Kamen {x},{y} nelze pohnout na {xChcesPohnout},{yChcesPohnout}!");
            return false;
        }

        if (!KontrolaDiaonalnihoPohybu(x, y, xChcesPohnout, yChcesPohnout, 1) &&
            !KontrolaDiaonalnihoPohybu(x, y, xChcesPohnout, yChcesPohnout, 2))
        {
            VypisCeskaDama.VypisChybu($"Kamen {x},{y} nelze pohnout diagonálně na {xChcesPohnout},{yChcesPohnout}!");
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
        HraSkoncila = true;
        ResetPocetKamenu();

        VypisCeskaDama.VypisKonecHry(kdoVyhral);
    }

    private void ResetPocetKamenu()
    {
        PocetBilychKamenu = 0;
        PocetCernychKamenu = 0;
    }

    private void NastavHerniDesku()
    {
        NastavBilePolicka();
        NastavStredDesky();
        NastavCernePolicka();
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
}