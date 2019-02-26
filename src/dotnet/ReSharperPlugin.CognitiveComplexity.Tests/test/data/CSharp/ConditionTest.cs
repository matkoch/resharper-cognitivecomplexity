using System;
using System.Threading.Tasks;

public class A
{
    public void M1(bool b)
    {
        if (b) // +1
            Console.WriteLine();
        else // +1
            Console.WriteLine();
    }
    
    public void M2(bool b)
    {
        if (true) // +1
        {
            if (b) // +2 (N=1)
                Console.WriteLine();
            else if (!b) // +1
                Console.WriteLine();
            else // +1
                Console.WriteLine();
        }
    }
}
