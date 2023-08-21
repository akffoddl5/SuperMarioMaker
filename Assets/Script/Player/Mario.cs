using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mario : MonoBehaviour
{
    [Header("Move Info")]
    public float moveSpeed;
    public float jumpPower;
    

    Mario_stateMachine stateMachine;
 
    public Mario_idle idleState;
    public Mario_run runState;

	private void Awake()
	{
        stateMachine = new Mario_stateMachine();

        idleState = new Mario_idle(this, stateMachine, "Idle");
        runState = new Mario_run(this, stateMachine, "Move");
	}
	private void Start()
	{
        stateMachine.InitState(idleState);

	}
	// Update is called once per frame
	void Update()
    {
		stateMachine.currentState.Update();
	}
}
