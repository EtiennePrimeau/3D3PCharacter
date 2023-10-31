using System.Collections.Generic;
using UnityEngine;


public class CharacterControllerSM : BaseStateMachine<CharacterState>
{
    public static CharacterControllerSM Instance { get; private set; }

    [field: SerializeField] public Camera Camera { get; private set; }
    [field: SerializeField] public Rigidbody Rb { get; private set; }
    [field: SerializeField] private Animator Animator { get; set; }
    [field: SerializeField] public BoxCollider HitBox { get; private set; }
    [field: SerializeField] public float AccelerationValue { get; private set; } = 20.0f;
    [field: SerializeField] public float JumpAccelerationValue { get; private set; } = 320.0f;
    [field: SerializeField] public float SlowedDownAccelerationValue { get; private set; } = 7.0f;
    [field: SerializeField] public float ForwardMaxVelocity { get; private set; } = 6.0f;
    [field: SerializeField] public float LateralMaxVelocity { get; private set; } = 4.0f;
    [field: SerializeField] public float BackwardMaxVelocity { get; private set; } = 2.0f;
    [field: SerializeField] public float SlowingVelocity { get; private set; } = 0.97f;
    [field: SerializeField] public float MaxNoDamageFall { get; private set; } = 10.0f;
    [field: SerializeField] public float RotationSpeed { get; private set; } = 3.0f;
    [field: SerializeField] public GameObject ObjectToLookAt { get; private set; }
    [field: SerializeField] public GameObject MC { get; private set; }

    // /////////////////
    public Vector3 ForwardVectorOnFloor { get; private set; }
    public Vector3 ForwardVectorForPlayer { get; private set; }
    public Vector3 RightVectorOnFloor { get; private set; }
    public Vector3 RightVectorForPlayer { get; private set; }
    public bool IsStunned { get; private set; } = false;

    public bool IsInNonGameplay { get; private set; } = true;

    // /////////////////

    [SerializeField] private GroundDetection m_groundCollider;
    [SerializeField] private HitDetection m_hitDetection;


    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<CharacterState>();
        m_possibleStates.Add(new NonGameplayState());
        m_possibleStates.Add(new FreeState());
        m_possibleStates.Add(new JumpState());
        m_possibleStates.Add(new HitState());
        m_possibleStates.Add(new InAirState());
        m_possibleStates.Add(new AttackState());
        m_possibleStates.Add(new StunnedState());
    }

    protected override void Awake()
    {
        base.Awake();
    
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    protected override void Start()
    {
        foreach (CharacterState state in m_possibleStates)
        {
            state.OnStart(this);
        }

        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();

    }
    protected override void Update()
    {
        base.Update();

        SetForwardVectorFromGroundNormal();

        SetIsGroundedAnimationBool();
        SetTouchingGroundAnimationBool();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    protected override void FixedUpdate()
    {
        RotatePlayer();
        base.FixedUpdate();
    }

    private void RotatePlayer()
    {
        if (m_currentState is not NonGameplayState)
        {
            float currentAngleX = Input.GetAxis("Mouse X") * RotationSpeed;
            MC.transform.RotateAround(ObjectToLookAt.transform.position, ObjectToLookAt.transform.up, currentAngleX);
        }
    }

    private void SetForwardVectorFromGroundNormal()
    {
        int layerMask = 1 << 8;
        RaycastHit hit;
        Vector3 hitNormal = Vector3.up;
        float vDiffMagnitude = 3.0f;
        Vector3 vDiff = transform.position - new Vector3(transform.position.x, transform.position.y + vDiffMagnitude, transform.position.z);

        if (Physics.Raycast(transform.position, vDiff, out hit, vDiffMagnitude, layerMask))
        {
            hitNormal = hit.normal;
        }

        ForwardVectorOnFloor = Vector3.ProjectOnPlane(MC.transform.forward, Vector3.up);
        ForwardVectorForPlayer = Vector3.ProjectOnPlane(ForwardVectorOnFloor, hitNormal);
        ForwardVectorForPlayer = Vector3.Normalize(ForwardVectorForPlayer);

        RightVectorOnFloor = Vector3.ProjectOnPlane(MC.transform.right, Vector3.up);
        RightVectorForPlayer = Vector3.ProjectOnPlane(RightVectorOnFloor, hitNormal);
        RightVectorForPlayer = Vector3.Normalize(RightVectorForPlayer);
    }

    public bool IsInContactWithFloor()
    {
        return m_groundCollider.IsGrounded;
    }

    public bool IsTouchingGround()
    {
        return m_groundCollider.TouchingGround;
    }

    public bool HasBeenHit()
    {
        return m_hitDetection.HasBeenHit;
    }

    public bool HasBeenStunned()
    {
        return m_hitDetection.HasBeenStunned || IsStunned;
    }

    public void SetIsStunnedToFalse()
    {
        IsStunned = false;
    }

    public void SetIsStunnedToTrue()
    {
        IsStunned = true;
    }

    public void HandleAttackHitbox(bool isEnabled)
    {
        HitBox.enabled = isEnabled;
    }

    public void UpdateAnimatorMovementValues(Vector2 movement)
    {
        float lateralMovement = movement.x / LateralMaxVelocity;
        float forwardMovement = movement.y;

        if (forwardMovement >= 0)
        {
            forwardMovement /= ForwardMaxVelocity;
        }
        else
        {
            forwardMovement /= BackwardMaxVelocity;
        }

        Animator.SetFloat("MoveLR", lateralMovement);
        Animator.SetFloat("MoveFB", forwardMovement);
    }

    public void TriggerJumpAnimation()
    {
        Animator.SetTrigger("Jump");
    }

    public void TriggerGettingHitAnimation()
    {
        Animator.SetTrigger("GettingHit");
    }

    public void TriggerIsAttackingAnimation()
    {
        Animator.SetTrigger("IsAttacking");
    }

    public void TriggerIsStunnedAnimation()
    {
        Animator.SetTrigger("IsStunned");
    }

    private void SetTouchingGroundAnimationBool()
    {
        Animator.SetBool("TouchingGround", IsTouchingGround());
    }

    private void SetIsGroundedAnimationBool()
    {
        Animator.SetBool("IsGrounded", IsInContactWithFloor());
    }

    public void SetNonGameplayState(bool value)
    {
        IsInNonGameplay = value;
    }
}
