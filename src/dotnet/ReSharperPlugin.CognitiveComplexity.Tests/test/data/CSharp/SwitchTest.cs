using System;

public class A
{
    public string M1(int number)
    {
        switch (number) // +1
        {
            case 1:                   
                return "one";
            case 2:                   
                return "a couple";
            case 3:
                return "a few";
            default:                  
                return "lots";
        }
    }
}
