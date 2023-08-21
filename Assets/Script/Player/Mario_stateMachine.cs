using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_stateMachine : MonoBehaviour
{
    Mario_state currentState;
    public void InitState(Mario_state _startState)
    {
        currentState = _startState;
    }
    public void ChangeState(Mario_state _changeState)
    {
        currentState.Exit();
        currentState = _changeState;
        currentState.Enter();
    }
}
