namespace CeskaDama;

public struct Kamen
{
    public Barvy Barva { get; set; }
    public bool Dama { get; set; }

    public Kamen(Barvy barva)
    {
        Barva = barva;
    }
}

public enum Barvy
{
    Bila,
    Cerna,
    Zadna
}