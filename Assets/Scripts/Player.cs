using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using state = playerStates;

public enum playerStates
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
public class Player : MonoBehaviour
{
    #region FILEDS & PROPERTIES
    private Dictionary<state, Action> statesStayMeths;
    private Dictionary<state, Action> statesEnterMeths;
    private Dictionary<state, Action> statesExitMeths;

    public playerStates state;
    public playerStates prevState;

    private AudioSource soundSrc;

    public float speed;
    #endregion

    #region LifeCycle
    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        soundSrc = GetComponent<AudioSource>();

        statesStayMeths = new Dictionary<state, Action>()
        {
            {state.MOVE, StateStayMove},
            {state.HIT, StateStayHit},
            {state.DEATH, StateStaySpawn},
            {state.SPAWN, StateStayIdle},
            {state.ATTACK, StateStayAttack},
        };

        statesEnterMeths = new Dictionary<state, Action>()
        {
            {state.MOVE, StateEnterMove},
            {state.HIT, StateEnterHit},
            {state.DEATH, StateEnterDeath},
            {state.SPAWN, StateEnterSpawn},
            {state.ATTACK, StateEnterAttack},
        };

        statesExitMeths = new Dictionary<state, Action>()
        {
            {state.MOVE, StateExitMove},
            {state.HIT, StateExitHit},
            {state.DEATH, StateExitDeath},
            {state.SPAWN, StateExitSpawn},
            {state.ATTACK, StateExitAttack},
        };

        state = state.MOVE;
        StateEnterMove();
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
    private void StateEnterAttack()
    {
    }

    private void StateEnterSpawn()
    {

    }

    private void StateEnterDeath()
    {
        Game.globalInstance.sndPlayer.PlaySound(SoundType.DEATH, soundSrc);
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
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector2.up * speed * Time.fixedDeltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector2.left * speed * Time.fixedDeltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector2.down * speed * Time.fixedDeltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
        }
    }
    #endregion

    #region Exit
    private void StateExitAttack()
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
