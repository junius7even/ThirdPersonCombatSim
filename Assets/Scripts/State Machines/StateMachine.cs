using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentState != null)
            currentState.Tick(Time.deltaTime); 
        
    }

    public void SwitchState(State newState)
    {
        currentState?.Exit(); // Check if the currentstate is null, if not then exit the current state. This does not work on monobehavior objects
        currentState = newState;
        currentState?.Enter(); // Ensuring that the new state isn't null
    }
}
