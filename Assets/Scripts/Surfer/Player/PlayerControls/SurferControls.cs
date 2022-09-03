using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurferControls : MonoBehaviour
{
    [Header("Control Variables")]
    [SerializeField] private float surfSpeed;
    [SerializeField] private float rotationRate;
    [SerializeField] private float brakeForce;
    [SerializeField] private float ollieHeight;

    [Header("Physics Variables")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer;

    [Header("Component References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider col;

    private bool m_HitDetect;
    RaycastHit m_Hit;
    public float m_MaxDistance;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void MovePlayer(Vector3 moveDirection, Transform playerModel)
    {
        //Don't allow the player to accelerate if they input forward
        float yStore = rb.velocity.y;
        Vector3 currentTrajectory = new Vector3(playerModel.forward.x * surfSpeed, yStore, playerModel.forward.z * surfSpeed);
        if(moveDirection.x != 0)
        {
            //newRot.y *= moveDirection.x;
        }    
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
        playerModel.Rotate(0, moveDirection.x * rotationRate * Time.deltaTime, 0, Space.World);
        Vector3 newVel = new Vector3(playerModel.forward.x * surfSpeed, yStore, playerModel.forward.z * surfSpeed);
        rb.velocity = newVel;
        //if (isGrounded())
        //{
        //    Debug.Log("Surfer is grounded");
        //}
        //else
        //{
        //    Debug.Log("Surfer is mid-air");
        //}

        
    }

    private void FixedUpdate()
    {
        //Test to see if there is a hit using a BoxCast
        //Calculate using the center of the GameObject's Collider(could also just use the GameObject's position), half the GameObject's size, the direction, the GameObject's rotation, and the maximum distance as variables.
        //Also fetch the hit data
        m_HitDetect = Physics.BoxCast(col.bounds.center, transform.localScale, -transform.up, out m_Hit, transform.rotation, m_MaxDistance, groundLayer);
        if (m_HitDetect)
        {
            //Output the name of the Collider your Box hit
            Debug.Log("Hit : " + m_Hit.collider.name);
        }
    }

    private bool isGrounded()
    {
        return Physics.BoxCast(transform.position, transform.localScale, Vector3.down/*, transform.rotation, ground*/);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (m_HitDetect)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position, -transform.up * m_Hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(transform.position + -transform.up * m_Hit.distance, transform.localScale);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, -transform.up * m_MaxDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + -transform.up * m_MaxDistance, transform.localScale);
        }
    }

    public void InitialiaseSurferControls(Collider physicsCol)
    {
        col = physicsCol;
    }

}
