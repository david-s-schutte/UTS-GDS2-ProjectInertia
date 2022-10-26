using Surfer.Managers;
using UnityEngine;

public class TrickSystem : Manager
{
    // Start is called before the first frame update
    [SerializeField] private Trick[] trickQueue = new Trick[3];
    private int depretiation = 0;
    private float chainTime = 3f;
    private int chainamount = 0;
    private float timeSinceTrick;
    
    public delegate void TrickScore(string name,float value);
    public TrickScore OnTrickScoreUpdated;

    public float TimeSinceTrack
    {
        get => timeSinceTrick;
        set => timeSinceTrick = value;
    }

    
    public override void ManagerStart()
    {
        base.ManagerStart();
    }


    public void InputTrick(Trick trick)
    {
        if(timeSinceTrick > chainTime)
        {
            ResetQueue();
            depretiation = 0;
            chainamount = 0;
        }
        else
        {
            bool reset = true;
            foreach (Trick TRK in trickQueue)
            {
                if (TRK != null && TRK == trick)
                {
                    depretiation++;
                    reset = false;
                    break;
                }
            }

            if (reset)
            {
                depretiation = 0;
            }
            chainamount++;
        }
        AppendTrick(trick);
        
        OnTrickScoreUpdated?.Invoke(trick.name, Mathf.Floor(trick.BaseScore * Depretiate(depretiation) * Chain(chainamount)));
    }



    private void AppendTrick(Trick trick)
    {
        for (int x = 0; x < trickQueue.Length - 1; x++)
        {
            trickQueue[x] = trickQueue[x + 1];
        }
        trickQueue[trickQueue.Length-1] = trick;
        //trickQueue.Append(trick); .append is a function ???????
    }
    
    private void ResetQueue(){
        for (int x = 0; x < trickQueue.Length - 1; x++)
        {
            trickQueue[x] = null;
        }
    }

    private float Depretiate(float dep)
    {
        if (dep > 0)
        {
            return 1/dep;
        }

        return 1;
    }
    
    private float Chain(float chain)
    {
        return 1f + 0.5f*Mathf.Floor(chain / 5f);
    }
    
    

}
