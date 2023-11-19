namespace CzechQueen;

public struct Stone
{
    public Color Color { get; set; }
    public bool Queen { get; set; }

    public Stone(Color color)
    {
        Color = color;
    }
}