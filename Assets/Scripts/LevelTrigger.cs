using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public int sceneIndex = 0;
    [SerializeField] public LevelTransition levelTransition;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
