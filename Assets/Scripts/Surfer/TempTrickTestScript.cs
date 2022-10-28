using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempTrickTestScript : MonoBehaviour
{
    // Start is called before the first frame update
    private TrickSystem trickManager;
    [SerializeField] private TrickUI trickUI;
    public Canvas canvas;
    public Trick trick1;
    public Trick trick2;
    public Trick trick3;
    public bool bool1;
    public bool bool2;
    public bool bool3;
    
    void Start()
    {
        trickManager = ScriptableObject.CreateInstance<TrickSystem>();
        trickManager.ui = Instantiate(trickUI,canvas.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (bool1)
        {
            trickManager.InputTrick(trick1);
            bool1 = false;
        }
        if (bool2)
        {
            trickManager.InputTrick(trick2);
            bool2 = false;
        }
        if (bool3)
        {
            trickManager.InputTrick(trick3);
            bool3 = false;
        }
    }
}
