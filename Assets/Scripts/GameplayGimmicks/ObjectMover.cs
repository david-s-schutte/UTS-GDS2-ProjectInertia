using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    //Modifiable variables
    [SerializeField] private Transform[] nodes;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float distToChange;
    [SerializeField] private float delayTime;
    private float timeToNextMove;

    static Vector3 currentPosition;
    private int currentNodeIndex;

    // Start is called before the first frame update
    void Start()
    {
        timeToNextMove = delayTime;
        currentNodeIndex = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (nodes.Length > 0)
        {
            if (Vector3.Distance(transform.position, nodes[currentNodeIndex].transform.position) < distToChange)
            {
                GetNextNode();
            }
            gameObject.transform.position = Vector3.MoveTowards(transform.position, nodes[currentNodeIndex].position, Time.deltaTime * moveSpeed);
            //gameObject.transform.Translate((transform.position - nodes[currentNodeIndex].position) * moveSpeed * Time.deltaTime, Space.World);
        }
        else
            Debug.Log("ERROR: " + gameObject.name + "'s Object Mover Component does not have any nodes");
    }

    private void GetNextNode()
    {
        if (timeToNextMove >= 0)
            timeToNextMove -= Time.deltaTime;
        else
        {
            timeToNextMove = delayTime;
            currentNodeIndex++;
            if (currentNodeIndex >= nodes.Length)
                currentNodeIndex = 0;
        }
    }
}
