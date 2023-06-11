using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public List<GameObject> door = new List<GameObject>();
    public int door;
    // Update is called once per frame
    void Update()
    {
        if(door == 22)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
