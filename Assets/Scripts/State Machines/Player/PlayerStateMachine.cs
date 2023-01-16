using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStateMachine : StateMachine
{
    // Reminder that properties expose fields
    [field: SerializeField] // Will make a public field for us and serialize it to access this property below
    public InputReader InputReader { get; private set; } // Can publicly get the input reader but only privately set it

    [field: SerializeField]
    public CharacterController Controller { get; private set; }

    [field: SerializeField]
    public float FreeLookMovementSpeed { get; private set; }

    [field: SerializeField]
    public float TargetingMovementSpeed { get; private set; }

    [field: SerializeField]
    public float RotationDamping { get; private set; }

    [field: SerializeField]
    public Animator Animator { get; private set; }

    [field: SerializeField]
    public Targeter Targeter { get; private set; }

    [field: SerializeField]
    public ForceReceiver ForceReceiver { get; private set; }

    [field: SerializeField]
    public Attack[] Attacks { get; private set; }

    [field: SerializeField]
    public WeaponDamage Weapon { get; private set; }

    public Transform MainCameraTransform { get; private set; }

    [field: SerializeField]
    public Health Health { get; private set; }

    [field: SerializeField]
    public Ragdoll Ragdoll{ get; private set; }

    private void Start()
    {
        MainCameraTransform = Camera.main.transform; // Searches the properties in the main scene for a Camera object and gets the transform property
        SwitchState(new PlayerFreeLookState(this));
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDie;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleDie;
    }

    private void HandleTakeDamage()
    {
        SwitchState(new PlayerImpactState(this));
    }

    private void HandleDie()
    {
        SwitchState(new PlayerDeadState(this));
    }
}
