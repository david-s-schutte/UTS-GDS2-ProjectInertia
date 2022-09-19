using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Vector3 axesToRotate;

    void FixedUpdate()
    {
        Vector3 rotation = axesToRotate * rotateSpeed;
        gameObject.transform.Rotate(rotation * Time.fixedDeltaTime);
    }
}
