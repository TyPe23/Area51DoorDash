using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] stars;

    //public int Rating { get; private set; }
    public int Rating;// { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Rating = 10;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject star in stars)
        {
            star.SetActive(false);
        }

        for (int i = 0; i <= Rating - 1; i++)
        {
            stars[i].SetActive(true);
        }
    }

    public void reduceRating()
    {
        if (Rating > 0)
        {
           Rating--;
        }
    }
}
