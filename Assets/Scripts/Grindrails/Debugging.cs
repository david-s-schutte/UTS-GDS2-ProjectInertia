using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugging : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 point = new Vector3(-1, -2);
        Vector3 normal = point.normalized;
        Vector3 tangent;
        Vector3 t1 = Vector3.Cross(normal, Vector3.forward);
        Vector3 t2 = Vector3.Cross(normal, Vector3.up);
        if (t1.magnitude > t2.magnitude)
        {
            tangent = t1;
        }
        else
        {
            tangent = t2;
        }

        Debug.Log(normal);
        Debug.Log(tangent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
