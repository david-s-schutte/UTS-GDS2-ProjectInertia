using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrickSystem : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField] private Trick[] trickQueue;
    public TrickUI ui;
    private int depretiation = 0;
    private int chainAmount;
    


    public void InputTrick(Trick trick)
    {
        foreach (Trick TRK in trickQueue)
        {
            if (TRK != null && TRK == trick)
            {
                depretiation++;
                break;
            }
        }
        AppendTrick(trick);
        
        
        
    }



    private void AppendTrick(Trick trick)
    {
        for (int x = 0; x < trickQueue.Length - 1; x++)
        {
            trickQueue[x] = trickQueue[x + 1];
        }
        trickQueue[trickQueue.Length - 1] = trick;
        //trickQueue.Append(trick); .append is a function ???????
    }
    
    

}
