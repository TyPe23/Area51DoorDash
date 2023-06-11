using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConsole : MonoBehaviour
{
    public Camera[] cameras;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            foreach (Camera cam in cameras)
            {
                cam.ChangeState(camStates.INACTIVE);
            }
        }
    }
}
