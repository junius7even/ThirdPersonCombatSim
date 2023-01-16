using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 MovementValue { get; private set; } // Properties expose fields. Fields should be kept at class level

    public event Action JumpEvent; // Triggers when jump happens

    public event Action DodgeEvent;

    public event Action TargetEvent;

    public event Action CancelEvent;

    public bool IsAttacking { get; private set; }
    public bool isBlocking { get; private set; }

    private Controls controls;

    // Start is called before the first frame update
    private void Start()
    {
        this.controls = new Controls();
        this.controls.Player.SetCallbacks(this);

        controls.Player.Enable();
    }

    private void OnDestroy()
    {
        controls.Player.Disable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Jumps when they press the button, don't care when it's released
        if(!context.performed) // Performed means pressed
            return;
        JumpEvent?.Invoke(); // Checkes if the event is null first, then invokes the event
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        // Dodges when the button is pressed, don't care when it's released
        if (!context.performed)
            return;
        DodgeEvent?.Invoke(); // Checks if the event is null first, then invokes the event
    }

    void Controls.IPlayerActions.OnMove(InputAction.CallbackContext context)
    {
        // Method reads the value that the controller sends it
        MovementValue = context.ReadValue<Vector2>(); // Specify datatype inside the angle brackets
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // Cinemachine is using the component not us manually
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        TargetEvent?.Invoke();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        CancelEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed) {
            IsAttacking = true;
        }
        else if (context.canceled) 
        {
            IsAttacking = false;
        }
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            isBlocking = true;
        }
        else if (context.canceled)
        {
            isBlocking = false;
        }
    }
}
