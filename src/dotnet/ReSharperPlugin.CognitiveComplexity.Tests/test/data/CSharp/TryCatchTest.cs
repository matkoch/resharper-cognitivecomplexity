using System;

public class A
{
    public void M1(bool a, bool b)
    {
        try
        {
            if (a) // +1
            {
                
                for (int i = 0; i < 10; i++) // +2 (N=1)
                {
                    
                    while (b) // +3 (N=2)
                    {
                    } 
                }
            }
        }
        catch (Exception e)
        {
            if (b) // +2 (N=1)
            {
            }
        }
    }
 
    public void M2()
    {
        if (true) // +1
        {
            try
            {  
                throw new Exception("ErrorType1");  
            }  
            catch(IndexOutOfRangeException ex) when (ex.Message=="ErrorType1") // +1
            {
            }
            catch(IndexOutOfRangeException ex) when (ex.Message=="ErrorType2") // +1?
            {
            }
        }
    }
}
