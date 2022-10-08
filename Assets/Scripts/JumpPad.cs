using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] bool ignorePlayerEnterVelocity;
    [SerializeField] float strength = 20.0f;
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<PlayerController>()) {
            if (ignorePlayerEnterVelocity) {
                PlayerController.SetVelocity(new(0, strength, 0));
            } else {
                PlayerController.ApplyVelocity(new(0, strength, 0));
            }
        }
    }
}
