using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<PlayerController>()) {
            PlayerController.SetVelocity(new(0, 20, 0));
        }
    }
}
