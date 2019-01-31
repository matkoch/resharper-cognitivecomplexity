using System;

public class A
{
    public void M1(object obj)
    {
        string str = null;
        if (obj != null) // +1
            str = obj.ToString();
    }

    public void M2(object obj)
    {
        var str = obj?.ToString();
    }
}
