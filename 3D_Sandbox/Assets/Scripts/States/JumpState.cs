using UnityEngine;


public class JumpState : CharacterState
{
    private const float GROUNDCHECK_DELAY_TIMER = 0.5f;
    private float m_currentGCDelayTimer = 0.0f;


    public override void OnEnter()
    {
        Debug.Log("Entering JumpState");

        m_stateMachine.Rb.AddForce(Vector3.up * m_stateMachine.JumpAccelerationValue,
                ForceMode.Acceleration); //TODO: fonction dans le stateMachine 

        m_currentGCDelayTimer = GROUNDCHECK_DELAY_TIMER;

        m_stateMachine.TriggerJumpAnimation();
    }

    public override void OnFixedUpdate()
    {
        AddForceFromInputs();
    }

    private void AddForceFromInputs()
    {
        Vector2 inputs = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            inputs.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputs.y -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputs.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputs.x += 1;
        }
        inputs.Normalize();

        m_stateMachine.Rb.AddForce(inputs.y * m_stateMachine.ForwardVectorForPlayer * m_stateMachine.SlowedDownAccelerationValue,
                ForceMode.Acceleration);
        m_stateMachine.Rb.AddForce(inputs.x * m_stateMachine.RightVectorForPlayer * m_stateMachine.SlowedDownAccelerationValue,
                ForceMode.Acceleration);
    }

    public override void OnUpdate()
    {
        m_currentGCDelayTimer -= Time.deltaTime;
    }

    public override void OnExit()
    {
        Debug.Log("Exiting JumpState");
    }

    public override bool CanEnter(IState currentState)
    {
        if (currentState is FreeState ||
            currentState is HitState)
        {
            if (!m_stateMachine.IsInContactWithFloor())
            {
                return false;
            }
            return Input.GetKeyDown(KeyCode.Space);
        }
        return false;
    }
    public override bool CanExit()
    {
        if (m_currentGCDelayTimer <= 0)
        {
            return m_stateMachine.IsInContactWithFloor() || m_stateMachine.HasBeenHit();
        }
        return false;
    }

}
