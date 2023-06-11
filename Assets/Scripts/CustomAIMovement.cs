using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

public class CustomAIMovement : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    private Transform player;

    private float distance;

    public int meleeDamage;
    public float speed = 200f;
    public float nextWaypointDistance = 1f;
    public float pathUpdateSpeed = .2f;
    public float AOA = 100; //Area of Awareness
    public bool AOAToggle = false;
    public float attackRange = 10;
    public bool randomizerToggle = false;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    public float delay;
    public Transform waypoint1;
    public Transform waypoint2;
    Transform nextPos;

    Seeker seeker;
    Rigidbody2D rb;

    public Animator anim;

    float timer;

    public bool alerted;

    public Transform alertLocation;

    public float rotationSpeed = 5; //Vision cone rotation speed

    public float minimalAwareness;

    public bool LOS;

    private Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        timer = delay;
        nextPos = waypoint1;
        if (randomizerToggle == true)
        {
            Randomizer();
        }

        player = FindObjectOfType<Player>().transform;
        anim = GetComponentInChildren<Animator>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSpeed);
    }//End of Start

    private void Update()
    {
        anim.SetFloat("DirX", rb.velocity.normalized.x);
        anim.SetFloat("DirY", rb.velocity.normalized.y);
        movement = new Vector2(rb.velocity.normalized.x, rb.velocity.normalized.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);
    }
    void FixedUpdate() //Ideal when working with physics
    {

        Attack();
        if (path == null)
        {
            Debug.LogWarning("No path");
            return;
        }


        if (currentWaypoint + 1 >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            Debug.Log("Reached End of path");
        }

        else
        {
            reachedEndOfPath = false;
        }


        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance && currentWaypoint < path.vectorPath.Count - 1)
        {
            currentWaypoint++;
        }

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the vision cone towards the target direction
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }//End of FixedUpdate

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }//End of OnPathComplete

    void UpdatePath()//Repeating function to update the path.
    {
        distance = Vector2.Distance(player.position, transform.position);

        if (seeker.IsDone()) //If the seeker is done calculating the path.
        {
            if (alerted == true)
            {
                seeker.StartPath(rb.position, alertLocation.position, OnPathComplete);
                if (distance <= AOA && AOAToggle == true)
                {
                    alerted = false;
                    return;
                }
            }
            if ((distance <= AOA && AOAToggle == true && player.GetComponent<AudioSource>().volume > .1) || distance < minimalAwareness || LOS == true)
            {
                seeker.StartPath(rb.position, player.position, OnPathComplete);

            }
            if (AOAToggle == false)
            {
                seeker.StartPath(rb.position, player.position, OnPathComplete);
            }

            if (distance >= AOA && AOAToggle && reachedEndOfPath == true)
            {
                timer -= Time.deltaTime;
                Debug.Log(timer);
                if (timer <= 0f)
                {
                    patrol();
                    timer = delay;
                }
            }




        }


    }// End of UpdatePath

    void patrol()
    {
        if (distance >= AOA && AOAToggle == true)
        {
            for (int i = 0; i <= waypoints.Count; i++)
            {
                Debug.Log(i);
                float dis = Vector2.Distance(waypoints[i].transform.position, transform.position);
                seeker.StartPath(rb.position, waypoints[i].position, OnPathComplete);

                if (dis <= .1)
                {
                    i++;
                }

            }
        }
        /*if (distance >= AOA && AOAToggle == true)
        {

            seeker.StartPath(rb.position, nextPos.position, OnPathComplete);

            float dis1 = Vector2.Distance(waypoint1.transform.position, transform.position);
            float dis2 = Vector2.Distance(waypoint2.transform.position, transform.position);

            if (dis1 <= 1)
            {
                nextPos = waypoint2;

            }
            if (dis2 <= 1)
            {
                nextPos = waypoint1;

            }
        }
        else
        {
            return;
        }
        */

    }


    void OnDrawGizmosSelected()
    {
        //Drawing AOA
        if (AOAToggle == true)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AOA);
        }

        //Drawing melee attackRange
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);

    }

    void Randomizer()
    {
        speed = Random.Range(700f, 800f);
        Debug.Log(speed);
        pathUpdateSpeed = Random.Range(.1f, 1f);
        Debug.Log(pathUpdateSpeed);
    }

    void Attack()
    {

        if (distance <= attackRange /*&& !invuln.invul*/)
        {

            // target.gameObject.GetComponent<SuperPupSystems.Helper.Health>().Damage(meleeDamage); //Logans Code. Works with Erics Health Script.
            // anim.SetTrigger("Attack");
        }
        else
        {
            //anim.ResetTrigger("Attack"); //Redundency just in case
            return;
        }
    }

    public void Die()
    {
        // anim.SetTrigger("Death");
    }


}//End of class
