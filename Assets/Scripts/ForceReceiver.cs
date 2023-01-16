using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float drag = 0.3f;
    [SerializeField] private NavMeshAgent agent;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity; // Give movement based on gravity and knockback/external forces

    private Vector3 impact;

    private Vector3 dampingVelocity; // value for the method to use and modify. The ref keyword makes it so that this is passed by reference

    private float verticalVelocity;

    // Called every frame, to keep things framerate independent you must also multiply the deltatime
    private void Update()
    {
        if (verticalVelocity < 0f && controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        } 
        else 
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime; // gravity is negative
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

        if (agent != null) 
        {
            if (impact.sqrMagnitude < 0.2f * 0.2f)
            {
                impact = Vector3.zero;
                agent.enabled = true;
            }
        }
        
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
        if (agent != null)
        {
            agent.enabled = false;
        }
    }
}