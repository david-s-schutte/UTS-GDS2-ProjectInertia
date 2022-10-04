using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    //Modifiable variables
    [SerializeField] private Transform[] nodes;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float distToChange;

    static Vector3 currentPosition;
    private int currentNodeIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentNodeIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (nodes.Length > 0)
        {
            if (Vector3.Distance(nodes[currentNodeIndex].transform.position, transform.position) < distToChange)
            {
                GetNextNode();
            }
            gameObject.transform.position = Vector3.MoveTowards(transform.position, nodes[currentNodeIndex].position, Time.deltaTime * moveSpeed);
        }
        else
            Debug.Log("ERROR: " + gameObject.name + "'s Object Mover Component does not have any nodes");
        //Debug.Log("Moving towards: " + nodes[currentNodeIndex].position);
    }

    private void GetNextNode()
    {
        currentNodeIndex++;
        if (currentNodeIndex >= nodes.Length)
            currentNodeIndex = 0;
    }
}
