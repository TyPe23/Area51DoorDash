using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerConsole : MonoBehaviour
{
    public List<GameObject> lazers = new List<GameObject>();
    public List<GameObject> guards = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject lazer in lazers)
        {
            lazer.GetComponent<LazerDetection>().console = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            foreach (GameObject lazer in lazers)
            {
                lazer.SetActive(false);
            }
        }
    }
}
