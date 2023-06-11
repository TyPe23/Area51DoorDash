using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] stars;

    //public int Rating { get; private set; }
    public int rating;// { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        rating = 10;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject star in stars)
        {
            star.SetActive(false);
        }

        for (int i = 0; i <= rating - 1; i++)
        {
            stars[i].SetActive(true);
        }

        if(rating == 0)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void reduceRating()
    {
        if (rating > 0)
        {
           rating--;
        }
    }
}
