namespace CeskaDama;

public class HraCeskaDama
{
    private Kamen[,] HerniDeska { get; set; } = new Kamen[8, 8];
    private bool HraSkoncila { get; set; } = false;
    private int PocetBilychKamenu { get; set; } = 12;
    private int PocetCernychKamenu { get; set; } = 12;

    public void OtestujDamu()
    {
        int[,] testovaciPole = new[,]
        {
            {2, 0},
            {3, 1},
            {5, 3},
            {4, 2},
            {3, 1},
            {5, 3},
            {5, 1},
            {4, 0},
            {2, 6},
            {3, 7},
            {6, 2},
            {5, 1},
            {5, 3},
            {6, 2},
            {6, 4},
            {5, 3},
            {3, 7},
            {4, 6},
            {7, 3},
            {6, 4},
            {6, 2},
            {7, 3},
            {2, 4},
            {3, 5},
            {4, 0},
            {3, 1}
        };

        Barvy barvaKamene = Barvy.Cerna;


        for (int i = 0; i < testovaciPole.GetLength(0) - 1; i += 2)
        {
            barvaKamene = barvaKamene == Barvy.Bila ? Barvy.Cerna : Barvy.Bila;
            PohniKamen(testovaciPole[i, 0], testovaciPole[i, 1], testovaciPole[i + 1, 0], testovaciPole[i + 1, 1],
                barvaKamene);


            Console.Clear();
            VypisCeskaDama.VypisHerniDesku(HerniDeska);
        }

        ResetKamene(ref HerniDeska[4, 6]);
        Console.Clear();
        VypisCeskaDama.VypisHerniDesku(HerniDeska);
    }

    public void ZacitHru()
    {
        Console.Clear();
        HraSkoncila = false;
        NastavHerniDesku();
        VypisCeskaDama.VypisHerniDesku(HerniDeska);

        if (true)
        {
            OtestujDamu();
            Console.WriteLine("konec testu");
            Console.ReadKey();
        }

        HerniSmycka();
    }

    private void HerniSmycka()
    {
        Barvy barvaKamene = Barvy.Cerna;

        while (!HraSkoncila)
        {
            barvaKamene = barvaKamene == Barvy.Bila ? Barvy.Cerna : Barvy.Bila;
            VypisCeskaDama.VypisKdoJeNaTahu(barvaKamene == Barvy.Bila ? "B" : "C");

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
                PohniKamen(x, y, xChcesPohnout, yChcesPohnout, barvaKamene);
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
        if (HerniDeska[x, y].Barva == Barvy.Bila)
        {
            PocetBilychKamenu--;
        }
        else
        {
            PocetCernychKamenu--;
        }

        VypisCeskaDama.VypisPocetKamenu(PocetBilychKamenu, PocetCernychKamenu);
        Thread.Sleep(1000);
    }

    private void PohniKamen(int x, int y, int xChcesPohnout, int yChcesPohnout, Barvy barvaKamene)
    {
        if (KontrolaJeDama(x, y))
        {
            bool posunVRozsahu = PosunVRozsahu(x, y, xChcesPohnout, yChcesPohnout);

            if (posunVRozsahu)
            {
                return;
            }

            VypisCeskaDama.VypisNelzePohnout();
        }

        bool posunutoOJedno = PosunOJedno(x, y, xChcesPohnout, yChcesPohnout);

        if (posunutoOJedno)
        {
            if (KontrolaZmenyNaDamu(xChcesPohnout, barvaKamene))
            {
                ZmenaNaDamu(xChcesPohnout, yChcesPohnout);
            }

            return;
        }

        bool posunutoODva = PosunODva(x, y, xChcesPohnout, yChcesPohnout);

        if (posunutoODva)
        {
            if (KontrolaZmenyNaDamu(xChcesPohnout, barvaKamene))
            {
                ZmenaNaDamu(xChcesPohnout, yChcesPohnout);
            }

            return;
        }

        VypisCeskaDama.VypisNelzePohnout();
    }

    private void ZmenaNaDamu(int xChcesPohnout, int yChcesPohnout) =>
        HerniDeska[xChcesPohnout, yChcesPohnout].Dama = true;

    private void VymenaKamenu(int x, int y, int xChcesPohnout, int yChcesPohnout)
    {
        HerniDeska[xChcesPohnout, yChcesPohnout].Barva = HerniDeska[x, y].Barva;
        HerniDeska[xChcesPohnout, yChcesPohnout].Dama = HerniDeska[x, y].Dama;
        ResetKamene(ref HerniDeska[x, y]);
    }

    private bool PosunOJedno(int x, int y, int xChcesPohnout, int yChcesPohnout)
    {
        switch (HerniDeska[x, y].Barva)
        {
            case Barvy.Bila:
            {
                // posun dolu vlevo o jedno
                if (x + 1 == xChcesPohnout && y - 1 == yChcesPohnout)
                {
                    VymenaKamenu(x, y, xChcesPohnout, yChcesPohnout);
                    return true;
                }

                // posun dolu vpravo o jedno
                if (x + 1 == xChcesPohnout && y + 1 == yChcesPohnout)
                {
                    VymenaKamenu(x, y, xChcesPohnout, yChcesPohnout);
                    return true;
                }

                return false;
            }
            case Barvy.Cerna:
            {
                // posun nahoru vpravo o jedno
                if (x - 1 == xChcesPohnout && y + 1 == yChcesPohnout)
                {
                    VymenaKamenu(x, y, xChcesPohnout, yChcesPohnout);
                    return true;
                }

                // posun nahoru vlevo o jedno
                if (x - 1 == xChcesPohnout && y - 1 == yChcesPohnout)
                {
                    VymenaKamenu(x, y, xChcesPohnout, yChcesPohnout);
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
        switch (HerniDeska[x, y].Barva)
        {
            case Barvy.Bila:
            {
                // posun dolu vlevo o dva
                if (x + 2 == xChcesPohnout && y - 2 == yChcesPohnout)
                {
                    OdeberKamen(x + 1, y - 1);
                    VymenaKamenu(x, y, xChcesPohnout, yChcesPohnout);
                    ResetKamene(ref HerniDeska[x + 1, y - 1]);
                    return true;
                }

                // posun dolu vpravo o dva
                if (x + 2 == xChcesPohnout && y + 2 == yChcesPohnout)
                {
                    OdeberKamen(x + 1, y + 1);
                    VymenaKamenu(x, y, xChcesPohnout, yChcesPohnout);
                    ResetKamene(ref HerniDeska[x + 1, y + 1]);
                    return true;
                }

                return false;
            }
            case Barvy.Cerna:
            {
                // posun nahoru vpravo o dva
                if (x - 2 == xChcesPohnout && y + 2 == yChcesPohnout)
                {
                    OdeberKamen(x - 1, y + 1);
                    VymenaKamenu(x, y, xChcesPohnout, yChcesPohnout);
                    ResetKamene(ref HerniDeska[x - 1, y + 1]);
                    return true;
                }

                // posun nahoru vlevo o dva
                if (x - 2 == xChcesPohnout && y - 2 == yChcesPohnout)
                {
                    OdeberKamen(x - 1, y - 1);
                    VymenaKamenu(x, y, xChcesPohnout, yChcesPohnout);
                    ResetKamene(ref HerniDeska[x - 1, y - 1]);
                    return true;
                }


                return false;
            }
            default:
                return false;
        }
    }

    // --- KONTROLA POHYBU ---

    private bool KontrolaPohybuKamene(int x, int y, int xChcesPohnout, int yChcesPohnout, Barvy barvaKamene)
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

        if (KontrolaJeDama(x, y))
        {
            return PosunVRozsahu(x, y, xChcesPohnout, yChcesPohnout);
        }

        if (!KontrolaDiaonalnihoPohybu(x, y, xChcesPohnout, yChcesPohnout, 1) &&
            !KontrolaDiaonalnihoPohybu(x, y, xChcesPohnout, yChcesPohnout, 2))
        {
            VypisCeskaDama.VypisChybu($"Kamen {x},{y} nelze pohnout diagonálně na {xChcesPohnout},{yChcesPohnout}!");
            return false;
        }

        return true;
    }

    private bool PosunVeSmeruARozsahu(int posun, string smer, int x, int y)
    {
        Barvy posledniBarva = Barvy.Zadna;

        for (int i = 1; i < posun; i++)
        {
            if (posledniBarva is Barvy.Bila or Barvy.Cerna)
            {
                VypisCeskaDama.VypisChybu("Nelze přeskočit více kamenů!");
                return false;
            }

            switch (smer.ToLower())
            {
                case "nahorevlevo":
                    posledniBarva = HerniDeska[x - i, y - i].Barva;

                    if (HerniDeska[x - i, y - i].Barva != Barvy.Zadna
                        && HerniDeska[x - i, y - i].Barva == HerniDeska[x, y].Barva)
                    {
                        VypisCeskaDama.VypisChybu("Nelze přeskočit kámen stejné barvy!");
                        return false;
                    }

                    if (HerniDeska[x - i, y - i].Barva != Barvy.Zadna)
                    {
                        OdeberKamen(x - i, y - i);
                        ResetKamene(ref HerniDeska[x - i, y - i]);
                    }

                    break;

                case "nahorevpravo":
                    posledniBarva = HerniDeska[x - i, y + i].Barva;

                    if (HerniDeska[x - i, y + i].Barva != Barvy.Zadna
                        && HerniDeska[x - i, y + i].Barva == HerniDeska[x, y].Barva)
                    {
                        VypisCeskaDama.VypisChybu("Nelze přeskočit kámen stejné barvy!");
                        return false;
                    }

                    if (HerniDeska[x - i, y + i].Barva != Barvy.Zadna)
                    {
                        OdeberKamen(x - i, y + i);
                        ResetKamene(ref HerniDeska[x - i, y + i]);
                    }

                    break;

                case "dolevlevo":
                    posledniBarva = HerniDeska[x + i, y - i].Barva;

                    if (HerniDeska[x + i, y - i].Barva != Barvy.Zadna
                        && HerniDeska[x + i, y - i].Barva == HerniDeska[x, y].Barva)
                    {
                        VypisCeskaDama.VypisChybu("Nelze přeskočit kámen stejné barvy!");
                        return false;
                    }

                    if (HerniDeska[x + i, y - i].Barva != Barvy.Zadna)
                    {
                        OdeberKamen(x + i, y - i);
                        ResetKamene(ref HerniDeska[x + i, y - i]);
                    }

                    break;

                case "dolevpravo":
                    posledniBarva = HerniDeska[x + i, y + i].Barva;

                    if (HerniDeska[x + i, y + i].Barva != Barvy.Zadna
                        && HerniDeska[x + i, y + i].Barva == HerniDeska[x, y].Barva)
                    {
                        VypisCeskaDama.VypisChybu("Nelze přeskočit kámen stejné barvy!");
                        return false;
                    }

                    if (HerniDeska[x + i, y + i].Barva != Barvy.Zadna)
                    {
                        OdeberKamen(x + i, y + i);
                        ResetKamene(ref HerniDeska[x + i, y + i]);
                    }

                    break;
            }
        }

        return true;
    }

    private bool PosunVRozsahu(int x, int y, int xChcesPohnout, int yChcesPohnout)
    {
        // kontrola nahoře vlevo
        if (xChcesPohnout < x && yChcesPohnout < y)
        {
            VymenaKamenu(x, y, xChcesPohnout, yChcesPohnout);
            return PosunVeSmeruARozsahu(x - xChcesPohnout, "nahorevlevo", x, y);
        }

        // kontrola nahoře vpravo
        if (xChcesPohnout < x && yChcesPohnout > y)
        {
            VymenaKamenu(x, y, xChcesPohnout, yChcesPohnout);
            return PosunVeSmeruARozsahu(x - xChcesPohnout, "nahorevpravo", x, y);
        }

        // kontrola dole vlevo
        if (xChcesPohnout > x && yChcesPohnout < y)
        {
            VymenaKamenu(x, y, xChcesPohnout, yChcesPohnout);
            return PosunVeSmeruARozsahu(xChcesPohnout - x, "dolevlevo", x, y);
        }

        // kontrola dole vpravo
        if (xChcesPohnout > x && yChcesPohnout > y)
        {
            VymenaKamenu(x, y, xChcesPohnout, yChcesPohnout);
            return PosunVeSmeruARozsahu(xChcesPohnout - x, "dolevpravo", x, y);
        }

        return false;
    }

    private bool KontrolaZmenyNaDamu(int xChcesPohnout, Barvy barvaKamene)
    {
        switch (barvaKamene)
        {
            case Barvy.Bila:
                return xChcesPohnout == HerniDeska.GetLength(0) - 1;
            case Barvy.Cerna:
                return xChcesPohnout == 0;
            default:
                return false;
        }
    }

    private bool KontrolaDiaonalnihoPohybu(int x, int y, int xChcesPohnout, int yChcesPohnout, int posunO) =>
        x - posunO == xChcesPohnout && y - posunO == yChcesPohnout
        || x + posunO == xChcesPohnout && y + posunO == yChcesPohnout
        || x - posunO == xChcesPohnout && y + posunO == yChcesPohnout
        || x + posunO == xChcesPohnout && y - posunO == yChcesPohnout;

    private bool KontrolaLzePohnout(int x, int y, int xChcesPohnout, int yChcesPohnout, Barvy barvaKamene) =>
        HerniDeska[x, y].Barva != Barvy.Zadna && HerniDeska[xChcesPohnout, yChcesPohnout].Barva == Barvy.Zadna &&
        HerniDeska[x, y].Barva == barvaKamene;

    private bool KontrolaJeNaDesce(int x, int y) =>
        x >= 0 && x <= HerniDeska.GetLength(0) && y >= 0 && y <= HerniDeska.GetLength(1);

    private bool KontrolaJeDama(int x, int y) => HerniDeska[x, y].Dama;

    // --- HERNI DESKA ---

    private void NastavHerniDesku()
    {
        NastavBilePolicka();
        NastavStredDesky();
        NastavCernePolicka();
    }

    private void NastavBilePolicka()
    {
        // prvni pulka desky (bile policka)
        for (int x = 0; x < (HerniDeska.GetLength(0) / 2) - 1; x++)
        {
            for (int y = 0; y < HerniDeska.GetLength(1); y++)
            {
                if ((x + y) % 2 == 0)
                {
                    Kamen bilyKamen = new Kamen(Barvy.Bila);
                    HerniDeska[x, y] = bilyKamen;
                }
                else
                {
                    Kamen prazdnyKamen = new Kamen(Barvy.Zadna);
                    HerniDeska[x, y] = prazdnyKamen;
                }
            }
        }
    }

    private void NastavCernePolicka()
    {
        // druha pulka desky (cerna policka)
        for (int x = (HerniDeska.GetLength(0) / 2) + 1; x < HerniDeska.GetLength(0); x++)
        {
            for (int y = 0; y < HerniDeska.GetLength(1); y++)
            {
                if ((x + y) % 2 != 0)
                {
                    Kamen prazdnyKamen = new Kamen(Barvy.Zadna);
                    HerniDeska[x, y] = prazdnyKamen;
                }
                else
                {
                    Kamen cernyKamen = new Kamen(Barvy.Cerna);
                    HerniDeska[x, y] = cernyKamen;
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
                Kamen prazdnyKamen = new Kamen(Barvy.Zadna);
                HerniDeska[x, y] = prazdnyKamen;
            }
        }
    }

    // --- KONEC HRY ---
    private bool KontrolaKonceHry()
    {
        // TODO: zkotrolovat nemůže provést svými kameny žádný tah.
        if (PocetCernychKamenu == 0)
        {
            KonecHry(Barvy.Bila);
            return true;
        }

        if (PocetBilychKamenu == 0)
        {
            KonecHry(Barvy.Cerna);
            return true;
        }

        return false;
    }

    private void KonecHry(Barvy barva)
    {
        HraSkoncila = true;
        ResetPocetKamenu();

        VypisCeskaDama.VypisKonecHry(barva);
    }

    private void ResetPocetKamenu()
    {
        PocetBilychKamenu = 0;
        PocetCernychKamenu = 0;
    }

    private void ResetKamene(ref Kamen kamen)
    {
        kamen.Barva = Barvy.Zadna;
        kamen.Dama = false;
    }
}