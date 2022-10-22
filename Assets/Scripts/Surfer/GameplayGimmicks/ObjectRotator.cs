using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObjectRotator : StickyPlatform
{
    Rigidbody rb;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Vector3 axesToRotate;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 rotation = axesToRotate * rotateSpeed;
        // gameObject.transform.Rotate(rotation * Time.fixedDeltaTime);
        rb.MoveRotation(transform.rotation * Quaternion.Euler(rotation * Time.fixedDeltaTime));
    }
}
