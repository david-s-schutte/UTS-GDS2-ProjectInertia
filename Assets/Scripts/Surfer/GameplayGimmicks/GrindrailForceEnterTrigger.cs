using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindrailForceEnterTrigger : MonoBehaviour
{
    [SerializeField] GrindRailController grindRailController;
    private void Awake() {
        if (!grindRailController) {
            Debug.LogWarning(GetComponentInParent<Transform>().name + " entry trigger does not have a GrindRailController set! Deleting trigger.");
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            PlayerController.EnterGrindRail(grindRailController);
        }
    }
}
