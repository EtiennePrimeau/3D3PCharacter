using System.Collections.Generic;
using UnityEngine;


// Renommer CharacterControllerSM


public class CharacterController : MonoBehaviour
{
    [field: SerializeField] public Camera Camera { get; private set; }
    [field: SerializeField] public float AccelerationValue { get; private set; } = 20.0f;
    [field: SerializeField] public float JumpAccelerationValue { get; private set; } = 300.0f;
    [field: SerializeField] public float SlowedDownAccelerationValue { get; private set; } = 7.0f;
    [field: SerializeField] public float ForwardMaxVelocity { get; private set; } = 6.0f;
    [field: SerializeField] public float LateralMaxVelocity { get; private set; } = 4.0f;
    [field: SerializeField] public float BackwardMaxVelocity { get; private set; } = 2.0f;
    [field: SerializeField] public float SlowingVelocity { get; private set; } = 0.97f;
    [field: SerializeField] public Rigidbody Rb { get; private set; }
    [field: SerializeField] private Animator Animator { get; set; }

    // /////////////////
    public Vector3 ForwardVectorOnFloor { get; private set; }
    public Vector3 ForwardVectorForPlayer { get; private set; }
    public Vector3 RightVectorOnFloor { get; private set; }
    public Vector3 RightVectorForPlayer { get; private set; }

    // /////////////////

    private CharacterState m_currentState;
    private List<CharacterState> m_possibleStates;

    [SerializeField] private GroundDetection m_groundCollider;
    [SerializeField] private HitDetection m_hitDetection;


    private void Awake()
    {
        m_possibleStates = new List<CharacterState>();
        m_possibleStates.Add(new FreeState());
        m_possibleStates.Add(new JumpState());
        m_possibleStates.Add(new HitState());
    }

    // Start is called before the first frame update
    void Start()
    {
        //Rb = GetComponentInParent<Rigidbody>();

        foreach (CharacterState state in m_possibleStates)
        {
            state.OnStart(this);
        }

        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();

    }
    void Update()
    {
        m_currentState.OnUpdate();

        TryTransitionningState();

        SetForwardVectorFromGroundNormal();
    }

    private void FixedUpdate()
    {
        m_currentState.OnFixedUpdate();

        SetTouchingGroundAnimationBool();
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
            //Objet détecté
            //Debug.DrawRay(transform.position, vDiff.normalized * hit.distance, Color.red);

            hitNormal = hit.normal;
            //Debug.DrawRay(hit.point, hitNormal.normalized * 5.0f, Color.blue);
        }
        else
        {
            //Debug.DrawRay(transform.position, vDiff, Color.black);
        }


        ForwardVectorOnFloor = Vector3.ProjectOnPlane(Camera.transform.forward, Vector3.up);
        ForwardVectorForPlayer = Vector3.ProjectOnPlane(ForwardVectorOnFloor, hitNormal);
        ForwardVectorForPlayer = Vector3.Normalize(ForwardVectorForPlayer);

        RightVectorOnFloor = Vector3.ProjectOnPlane(Camera.transform.right, Vector3.up);
        RightVectorForPlayer = Vector3.ProjectOnPlane(RightVectorOnFloor, hitNormal);
        RightVectorForPlayer = Vector3.Normalize(RightVectorForPlayer);

        //Forward direction independent from camera height
        //Debug.DrawRay(transform.position, ForwardVectorForPlayer * 3.0f, Color.green);
    }

    private void TryTransitionningState()
    {
        if (!m_currentState.CanExit())
        {
            return;
        }
        foreach (var state in m_possibleStates)
        {
            if (m_currentState == state)
            {
                continue;
            }
            if (state.CanEnter())
            {
                //Quitter state actuel
                m_currentState.OnExit();
                m_currentState = state;
                //Rentrer dans state
                m_currentState.OnEnter();
                return;
            }
        }

    }

    public bool IsInContactWithFloor()
    {
        //Debug.Log("CC: " + m_groundCollider.IsGrounded);
        return m_groundCollider.IsGrounded;
    }

    public bool HasBeenHit()
    {
        return m_hitDetection.HasBeenHit;
    }

    public void UpdateAnimatorValues(Vector2 movement)
    {
        //Envoyer la velocity prise ds les states a cette fonction
        //pour qu'elle envoie à l'Animator

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

    public bool IsTouchingGround()
    {
        //Debug.Log(m_groundCollider.TouchingGround);
        return m_groundCollider.TouchingGround;
    }

    public void SetTouchingGroundAnimationBool()
    {
        Animator.SetBool("TouchingGround", IsTouchingGround());
    }
    
    public void SetTouchingGroundAnimationBoolToFalse()
    {
        Animator.SetBool("TouchingGround", false);
    }
}
