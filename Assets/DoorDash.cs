using System.Collections.Generic;
using UnityEngine;

public class DoorDash : MonoBehaviour
{
    private GameObject dialogue;

    public List<GameObject> food = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        dialogue = FindObjectOfType<Dialogue>().gameObject;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            if (Randomizer() == dialogue.GetComponent<Dialogue>().portraitNumb)
            {
                
                dialogue.GetComponent<Dialogue>().image1 = true;
                /*if(dialogue.GetComponent<Dialogue>().correct.gameObject.activeSelf)
                { */
                    Instantiate(food[Randomizer()], GetComponentInChildren<Transform>());
                    FindObjectOfType<HealthManager>().rating++;
               // }
               
            }
            else
            {
                dialogue.GetComponent<Dialogue>().image2 = true;
            }
            
        }
    }

    int Randomizer()
    {
        int rand = Random.Range(0, 4);
        return rand;
    }
}
