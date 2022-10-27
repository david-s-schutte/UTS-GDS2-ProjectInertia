using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoostPad : MonoBehaviour
{
    [SerializeField] float boostSpeed;
    const float cooldownTime = 0.5f;
    float cooldown = 0;
    [SerializeField] bool additive = false;

    public delegate void OnTriggered();
    public OnTriggered OnTriggeredPad;

    
    private void OnTriggerEnter(Collider other) {
        if (cooldown > 0) return;
        if (other.GetComponent<PlayerController>()) {
            PlayerController.SurfBoost(boostSpeed, Quaternion.LookRotation(transform.forward, transform.up), additive);
            OnTriggeredPad?.Invoke();
            StartCoroutine(Cooldown());
        }
    }
    
    IEnumerator Cooldown() {
        cooldown = cooldownTime;
        while (cooldown > 0) {
            cooldown -= Time.deltaTime;
            yield return null;
        }
        cooldown = 0;
    }
}
