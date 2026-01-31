using UnityEngine;

public class Mask_Feu : Mask_Object
{
    
    public Mask_Feu () 
    {
        ID = 1;
    }
    
    public void Fire_Ball()
    {
        if(Can_Use)
        {
            Can_Use = false;
            
        }
    }

}

