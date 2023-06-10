using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using state = camStates;

public enum camStates
{
    SCAN,
    ALERT,
    RESET,
    INACTIVE,
}

[RequireComponent(typeof(AudioSource))]
public class Camera : MonoBehaviour
{
    #region FILEDS & PROPERTIES
    private Dictionary<state, Action> statesStayMeths;
    private Dictionary<state, Action> statesEnterMeths;
    private Dictionary<state, Action> statesExitMeths;

    private Transform startTransform;
    private Transform player;
    private Vector3 target1;
    private Vector3 target2;
    public Vector3 currTarget;

    public camStates state;
    public camStates prevState;
    public bool alerted;



    [SerializeField]
    private GameObject vision;
    
    [SerializeField]
    private float rotAngle = 45;
    
    [SerializeField]
    private float swivelSpeed = 30;

    // whether or not the camera is able to scan back and forth or is static
    [SerializeField]
    private bool canMove = true;
    #endregion

    #region LifeCycle
    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        statesStayMeths = new Dictionary<state, Action>()
        {
            {state.SCAN, StateStayScan},
            {state.ALERT, StateStayAlert},
            {state.RESET, StateStayReset},
            {state.INACTIVE, StateStayInactive},
        };

        statesEnterMeths = new Dictionary<state, Action>()
        {
            {state.SCAN, StateEnterScan},
            {state.ALERT, StateEnterAlert},
            {state.RESET, StateEnterReset},
            {state.INACTIVE, StateEnterInactive},
        };

        statesExitMeths = new Dictionary<state, Action>()
        {
            {state.SCAN, StateExitScan},
            {state.ALERT, StateExitAlert},
            {state.RESET, StateExitReset},
            {state.INACTIVE, StateExitInactive},
        };

        target1 = transform.rotation.eulerAngles;
        target1 += new Vector3(rotAngle, 0, 0);

        target2 = transform.rotation.eulerAngles;
        target2 += new Vector3(-rotAngle, 0, 0);

        startTransform = transform;

        state = state.RESET;
        StateEnterReset();
    }

    void FixedUpdate()
    {
        statesStayMeths[state].Invoke();
    }

    public void ChangeState(state newState)
    {
        if (state != newState)
        {
            statesExitMeths[state].Invoke();
            prevState = state;
            state = newState;
            statesEnterMeths[state].Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && state == state.SCAN)
        {
            ChangeState(state.ALERT);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && state == state.ALERT)
        {
            ChangeState(state.RESET);
        }
    }
    #endregion

    #region Enter
    private void StateEnterInactive()
    {
        vision.SetActive(false);
    }

    private void StateEnterReset()
    {
        StartCoroutine(resetCam());
    }

    private void StateEnterAlert()
    {
        alerted = true;
    }

    private void StateEnterScan()
    {
        currTarget = target1;
    }
    #endregion

    #region Stay
    private void StateStayInactive()
    {
    }
    private void StateStayReset()
    {

    }
    private void StateStayAlert()
    {
        transform.LookAt(player.position);
    }

    private void StateStayScan()
    {
        float x = Mathf.PingPong(swivelSpeed * Time.time, rotAngle * 2);

        transform.localEulerAngles = new Vector3(x - 45, -90, 0);
    }
    #endregion

    #region Exit
    private void StateExitReset()
    {
        vision.SetActive(true);
    }

    private void StateExitInactive()
    {

    }

    private void StateExitAlert()
    {
        alerted = false;
    }

    private void StateExitScan()
    {

    }
    #endregion

    #region HELPER
    private IEnumerator resetCam()
    {
        yield return new WaitForSeconds(3);
        ChangeState(state.SCAN);
    }
    #endregion
}
