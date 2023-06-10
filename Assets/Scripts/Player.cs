using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using state = playerStates;

public enum playerStates
{
    MOVE,
    CROUCH,
    HIT,
    DEATH,
    SPAWN,
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
    public AudioSource footSteps;

    public float speed;
    private float crouchMod = 1;
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
            {state.CROUCH, StateStayCrouch},
            {state.HIT, StateStayHit},
            {state.DEATH, StateStaySpawn},
            {state.SPAWN, StateStayIdle},
            {state.ATTACK, StateStayAttack},
        };

        statesEnterMeths = new Dictionary<state, Action>()
        {
            {state.MOVE, StateEnterMove},
            {state.CROUCH, StateEnterCrouch},
            {state.HIT, StateEnterHit},
            {state.DEATH, StateEnterDeath},
            {state.SPAWN, StateEnterSpawn},
            {state.ATTACK, StateEnterAttack},
        };

        statesExitMeths = new Dictionary<state, Action>()
        {
            {state.MOVE, StateExitMove},
            {state.CROUCH, StateExitCrouch},
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ChangeState(state.HIT);
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
        StartCoroutine(hitStun());
    }

    private void StateEnterMove()
    {
        crouchMod = 1;

    }
    private void StateEnterCrouch()
    {
        crouchMod = 0.5f;

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
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            ChangeState(state.CROUCH);
        }
        else
        {
            crouchMod = 1;
        }

        Vector2 dir = new Vector2(x, y);

        footSteps.volume = dir.magnitude * crouchMod / 4;

        transform.Translate(dir * speed  * crouchMod * Time.fixedDeltaTime);
    }
    private void StateStayCrouch()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            ChangeState(state.MOVE);
        }

        Vector2 dir = new Vector2(x, y);

        footSteps.volume = dir.magnitude * crouchMod / 8;

        transform.Translate(dir * speed * crouchMod * Time.fixedDeltaTime);
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

    private void StateExitCrouch()
    {
    }
    #endregion

    #region HELPER
    private IEnumerator hitStun()
    {
        yield return new WaitForSeconds(0.25f);
        ChangeState(state.MOVE);
    }
    #endregion
}