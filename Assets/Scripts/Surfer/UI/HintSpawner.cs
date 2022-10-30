using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

public class HintSpawner : MonoBehaviour
{
    [SerializeField] private GameObject hintToSpawn;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Hint"))
                Destroy(g);
            Instantiate(hintToSpawn);
        }
    }
}
