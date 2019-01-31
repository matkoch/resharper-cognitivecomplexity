using System;

public class A
{
    public void M1()
    {
        M1(); // +1
    }

    public bool M2(bool a)
    {
        if (a && M2(a)) // +3 (if, and, recursive)
        {
            return true;
        }
        
        return false;
    }

    public bool M3(bool a)
    {
        return a && !M3(a); // +2
    }
}
