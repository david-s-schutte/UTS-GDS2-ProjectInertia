using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
public class CharacterPhysics : MonoBehaviour
{
    [Header("Control Variables")]
    private Vector3 movementDirection;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float doubleJumpModifier;
    [SerializeField] private int numberOfJumps;
    
    [Header("Physics Variables")]
    [SerializeField] private float gravityScale;

    [Header("Component References")]
    [SerializeField] private CharacterController controller;
    
    
    private CapsuleCollider _collider;
    private Rigidbody _rb;

    private void OnEnable()
    {
        _collider = GetComponent<CapsuleCollider>();
        _rb = GetComponent<Rigidbody>();
    }

    public void MoveCharacter()
    {
        
    }
    
    
    
    
}