using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_stateMachine
{
    public Mario_state currentState;
    public void InitState(Mario_state _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    
    public void ChangeState(Mario_state _changeState)
    {
        //Debug.Log(currentState.animBoolName + " << ");

        currentState.Exit();
        currentState = _changeState;
        currentState.Enter();
    }
}
