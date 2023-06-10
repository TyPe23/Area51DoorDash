using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using state = enemyStates;

public enum enemyStates
{
    MOVE,
    HIT,
    DEATH,
    SPAWN,
    GRABBED,
    THROWN,
    PRIME,
    ATTACK,
}

[RequireComponent(typeof(AudioSource))]
public class FSM_Template : MonoBehaviour
{
    #region FILEDS & PROPERTIES
    private Dictionary<state, Action> statesStayMeths;
    private Dictionary<state, Action> statesEnterMeths;
    private Dictionary<state, Action> statesExitMeths;

    public enemyStates state;
    public enemyStates prevState;
    #endregion

    #region LifeCycle
    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        statesStayMeths = new Dictionary<state, Action>()
        {
            {state.MOVE, StateStayMove},
            {state.HIT, StateStayHit},
            {state.DEATH, StateStaySpawn},
            {state.SPAWN, StateStayIdle},
            {state.GRABBED, StateStayGrabbed},
            {state.THROWN, StateStayThrown},
            {state.PRIME, StateStayPrime},
            {state.ATTACK, StateStayAttack},
        };

        statesEnterMeths = new Dictionary<state, Action>()
        {
            {state.MOVE, StateEnterMove},
            {state.HIT, StateEnterHit},
            {state.DEATH, StateEnterDeath},
            {state.SPAWN, StateEnterSpawn},
            {state.GRABBED, StateEnterGrabbed},
            {state.THROWN, StateEnterThrown},
            {state.PRIME, StateEnterPrime},
            {state.ATTACK, StateEnterAttack},
        };

        statesExitMeths = new Dictionary<state, Action>()
        {
            {state.MOVE, StateExitMove},
            {state.HIT, StateExitHit},
            {state.DEATH, StateExitDeath},
            {state.SPAWN, StateExitSpawn},
            {state.GRABBED, StateExitGrabbed},
            {state.THROWN, StateExitThrown},
            {state.PRIME, StateExitPrime},
            {state.ATTACK, StateExitAttack},
        };

        state = state.SPAWN;
        StateEnterSpawn();
    }

    void FixedUpdate()
    {
        statesStayMeths[state].Invoke();
    }

    public void ChangeState(state newState)
    {
        if (state != newState && state != state.DEATH)
        {
            statesExitMeths[state].Invoke();
            prevState = state;
            state = newState;
            statesEnterMeths[state].Invoke();
        }
    }
    #endregion

    #region Enter
    private void StateEnterPrime()
    {

    }

    private void StateEnterAttack()
    {

    }

    private void StateEnterThrown()
    {

    }

    private void StateEnterGrabbed()
    {

    }

    private void StateEnterSpawn()
    {

    }

    private void StateEnterDeath()
    {

    }

    private void StateEnterHit()
    {

    }

    private void StateEnterMove()
    {

    }
    #endregion

    #region Stay
    private void StateStayAttack()
    {

    }

    private void StateStayPrime()
    {

    }

    private void StateStayThrown()
    {

    }

    private void StateStayGrabbed()
    {

    }
    private void StateStayIdle()
    {
    }
    private void StateStaySpawn()
    {
    }
    private void StateStayHit()
    {
    }

    private void StateStayMove()
    {

    }
    #endregion

    #region Exit
    private void StateExitPrime()
    {

    }

    private void StateExitAttack()
    {

    }

    private void StateExitThrown()
    {

    }

    private void StateExitGrabbed()
    {

    }

    private void StateExitDeath()
    {

    }

    private void StateExitSpawn()
    {

    }

    private void StateExitHit()
    {

    }

    private void StateExitMove()
    {

    }
    #endregion

    #region HELPER

    #endregion
}
