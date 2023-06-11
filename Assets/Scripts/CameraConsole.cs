using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConsole : MonoBehaviour
{
    private Animator anim;
    public Camera[] cameras;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("Off");
            foreach (Camera cam in cameras)
            {
                cam.ChangeState(camStates.INACTIVE);
            }
        }
    }
}
