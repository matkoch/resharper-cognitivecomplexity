using System;
using System.Reflection.Emit;

public class A
{
    public void M1()
    {
    MyLabel:
        for (var i = 0; i < 100; i++) // +1
        {
            foreach (var c in "") // +2 (N=1)
            {
                while (true) // +3 (N=2)
                {
                    do // +4 (N=3)
                    {
                        if (true) // +5 (N=4)
                            goto MyLabel; // +1
                    } while (false);
                }
            }
        }
    }
    
    public void M2()
    {
        for (var i = 0; i < 100; i++) // +1
        {
        }
        
        foreach (var c in "") // +1
        {
        }

        while (true) // +1
        {
        }
        
        do // +1
        {
        } while (false);
        
    MyLabel: // +1
        goto MyLabel;
    }

    public void M3()
    {
        foreach (var c in "") // +1
        {
            if (true) // +2 (N=1)
                continue; // +1

            if (false) // +2 (N=1)
                break; // +1

            Console.WriteLine();
        }
    }
}
