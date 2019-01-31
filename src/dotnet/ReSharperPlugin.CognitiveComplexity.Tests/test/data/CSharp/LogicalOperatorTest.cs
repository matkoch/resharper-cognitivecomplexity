using System;

public class A
{
    public void M1(bool a, bool b, bool c, bool d)
    {
        var x = a || b || c; // +1
        var x1 = a && b && c && d; // +1
    }

    public void M2(bool a, bool b, bool c, bool d, bool e, bool f)
    {
        if (a // +1 for if
            && b && c // +1
            || d || e // +1
            && f) // +1
        {
        }
    }
}
