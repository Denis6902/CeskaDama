namespace CeskaDama;

public class HraCeskaDama
{
    private string[,] HerniDeska { get; set; }
    
    public HraCeskaDama()
    {
        HerniDeska = new string[8, 8];
        NastavHerniDesku();
    }
    
    private void NastavHerniDesku()
    {        
        // prvni pulka desky (bile policka)
        for (int i = 0; i < (HerniDeska.GetLength(0) / 2) - 1; i++)
        {
            for (int j = 0; j < HerniDeska.GetLength(1); j++)
            {
                if ((i + j) % 2 == 0)
                {
                    HerniDeska[i, j] = "B";
                }
                else
                {
                    HerniDeska[i, j] = " ";
                }
            }           
        }
        
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
                    HerniDeska[i, j] = "C";
                }
            }           
        }
    }
    
    public void VypisHerniDesku()
    {
        for (int i = 0; i < HerniDeska.GetLength(0); i++)
        {
            for (int j = 0; j < HerniDeska.GetLength(1); j++)
            {
                Console.Write(HerniDeska[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}