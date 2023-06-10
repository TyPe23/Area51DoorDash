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

    private Transform player;
    private Vector3 target1;
    private Vector3 target2;
    private Color alertColor = new Color(1, 0, 0, 0.5f);
    private Color scanColor = new Color(1, 1, 0, 0.5f);

    public camStates state;
    public camStates prevState;
    public bool alerted;
    public Vector3 currTarget;

    [SerializeField]
    private GameObject vision;

    [SerializeField]
    private CustomAIMovement[] guards;
    
    [SerializeField]
    private float rotAngle = 45;
    
    [SerializeField]
    private float swivelSpeed = 30;

    [SerializeField]
    private float startAngle = 0;

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
        if (collision.CompareTag("Player") && (state == state.SCAN || state == state.RESET))
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
        vision.GetComponent<SpriteRenderer>().color = alertColor;
        Transform playerPos = GameObject.FindWithTag("Player").transform;
        foreach (CustomAIMovement guard in guards)
        {
            guard.alerted = true;
            guard.alertLocation = playerPos;
        }
    }

    private void StateEnterScan()
    {
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

        transform.localEulerAngles = new Vector3(x + startAngle - rotAngle, -90, 0);
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
        vision.GetComponent<SpriteRenderer>().color = scanColor;
    }

    private void StateExitScan()
    {

    }
    #endregion

    #region HELPER
    private IEnumerator resetCam()
    {
        yield return new WaitForSeconds(3);

        if (state == state.RESET)
        {
            ChangeState(state.SCAN);
        }
    }
    #endregion
}
