using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrickUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Image flow;
    public float timeSinceTrick;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceTrick += Time.deltaTime;
    }
}
