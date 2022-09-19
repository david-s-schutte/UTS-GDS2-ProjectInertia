using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    //Modifiable variables
    [SerializeField] private Transform[] nodes;
    [SerializeField] private float moveSpeed;
    //[SerializeField] private float delayBetweenNodes;

    //Functionality stuff
    private float timer;
    private Vector3 startPosition;
    static Vector3 currentPosition;
    private int currentNodeIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentNodeIndex = 0;
        CheckNode();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * moveSpeed;
        if (gameObject.transform.position != currentPosition)
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, currentPosition, timer);
        else
        {
            if(currentNodeIndex < nodes.Length - 1)
            {
                currentNodeIndex++;
                CheckNode();
            }
            else
            {
                currentNodeIndex = 0;
                CheckNode();
            }
        }
    }

    private void CheckNode()
    {
        Debug.Log(gameObject.name + ", " + startPosition + ", " + currentPosition + ", " + currentNodeIndex);
        timer = 0f;
        startPosition = gameObject.transform.position;
        currentPosition = nodes[currentNodeIndex].position;
    }
}
