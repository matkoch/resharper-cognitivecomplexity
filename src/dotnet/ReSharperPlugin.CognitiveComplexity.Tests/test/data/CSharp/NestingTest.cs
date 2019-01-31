using System;

public class A
{
    public void M1()
    {
        for (var i = 0; i < 100; i++) // +1
        {
            for (var j = 0; j < 100; j++) // +2
            {
            }
        }
    }
    
    public void M2()
    {
        for (var i = 0; i < 100; i++) // +1
        {
        }
        for (var j = 0; j < 100; j++) // +1
        {
        }
    }
}
