using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerDetection : MonoBehaviour
{
    [HideInInspector]
    public LazerConsole console;
    public bool alert;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Colided");
            alert = true;
            foreach(GameObject guard in console.guards)
            {
                guard.GetComponent<CustomAIMovement>().alerted = alert;
                guard.GetComponent<CustomAIMovement>().alertLocation = transform;
                Debug.Log("Transform aquired");
            }
            
        }
    }
}
