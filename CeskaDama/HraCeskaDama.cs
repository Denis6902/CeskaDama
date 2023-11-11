namespace CeskaDama;

public class HraCeskaDama
{
    public string[,] HerniDeska { get; set; }
    
    public HraCeskaDama()
    {
        NastavHerniDesku();
    }
    
    public void NastavHerniDesku()
    {
        HerniDeska = new string[8, 8];
        
        // prvni pulka desky (bile policka)
        for (int i = 0; i < HerniDeska.GetLength(0) / 2; i++)
        {
            for (int j = 0; j < HerniDeska.GetLength(1); j++)
            {

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