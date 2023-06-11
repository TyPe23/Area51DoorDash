using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public List<Sprite> portraits = new List<Sprite>();
    public GameObject correct;
    public GameObject incorrect;
    public GameObject portrait;
    [HideInInspector]
    public int portraitNumb;


    float timer1 = 3f;
    float timer2 = 3f;
    [HideInInspector]
    public bool image1;
    [HideInInspector]
    public bool image2;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (image1 == true /*&& image2 != true*/)
        {
            correct.SetActive(true);
            portrait.SetActive(true);
            timer1 -= Time.deltaTime;
            if (timer1 <= 0)
            {
                Randomizer();
                timer1 = 3f;
                image1 = false;
                portrait.SetActive(false);
                correct.SetActive(false);
            }
        }

        if (image2 == true /*&& image1 != true*/)
        {
            incorrect.SetActive(true);
            portrait.SetActive(true);
            timer2 -= Time.deltaTime;
            if (timer2 <= 0)
            {
                Randomizer();
                timer2 = 3f;
                image2 = false;
                portrait.SetActive(false);
                incorrect.SetActive(false);
            }
        }
    }

    void Randomizer()
    {
        int rand = Random.Range(0, 4);
        portraitNumb = rand;
        portrait.GetComponent<Image>().sprite = portraits[rand];
    }
}
