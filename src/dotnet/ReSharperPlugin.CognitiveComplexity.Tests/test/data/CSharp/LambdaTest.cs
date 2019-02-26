using System;
using System.Threading.Tasks;

public class A
{
    public void M1(bool b)
    {
        var task = Task.Run(() =>
        {
            if (b) // +2 (N=1)
                Console.WriteLine();
        });
    }
    
    public void M2(bool b)
    {
        var task = Task.Run(delegate
        {
            if (b) // +2 (N=1)
                Console.WriteLine();
        });
    }
}
