using System;

public class A
{
    public void M1(bool a, bool b)
    {
        try // +1
        {
            if (a) // +1
            {
                
                for (int i = 0; i < 10; i++) // +2 (nesting=1)
                {
                    
                    while (b) // +3 (nesting=2)
                    {
                    } 
                }
            }
        }
        catch (Exception e) when (e.Message != null)
        {
            if (b) // +2 (nesting=1)
            {
            }
        }
    }
 
    public void M2()
    {
        try // +1
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
