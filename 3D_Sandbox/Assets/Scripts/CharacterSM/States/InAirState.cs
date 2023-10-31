using UnityEngine;

public class InAirState : CharacterState
{
    private const float GROUNDCHECK_DELAY_TIMER = 0.5f;
    private float m_currentGCDelayTimer = 0.0f;
    private float m_highestPositionY;


    public override void OnEnter()
    {
        Debug.Log("Entering InAirState");

        m_currentGCDelayTimer = GROUNDCHECK_DELAY_TIMER;
        m_highestPositionY = m_stateMachine.Rb.transform.position.y;
    }

    public override void OnFixedUpdate()
    {
        AddForceFromInputs();
        CheckForFallDamage();
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

    private void CheckForFallDamage()
    {
        float currentY = m_stateMachine.Rb.transform.position.y;
        if (currentY > m_highestPositionY)
        {
            m_highestPositionY = currentY;
            return;
        }
        
        float differenceY = m_highestPositionY - currentY;
        if (differenceY >= m_stateMachine.MaxNoDamageFall)
        {
            m_stateMachine.SetIsStunnedToTrue();
            Debug.Log("Fall damage");
        }
    }

    public override void OnUpdate()
    {
        m_currentGCDelayTimer -= Time.deltaTime;
    }

    public override void OnExit()
    {
        Debug.Log("Exiting InAirState");

        if (m_stateMachine.IsInContactWithFloor())
        {
            FXManager.Instance.PlaySound(EFXType.McLand, m_stateMachine.transform.position);
        }
    }

    public override bool CanEnter(IState currentState)
    {
        if (currentState is FreeState)
        {
            return !m_stateMachine.IsInContactWithFloor();
        }
        return false;
    }
    public override bool CanExit()
    {
        if (m_currentGCDelayTimer <= 0)
        {
            return m_stateMachine.IsInContactWithFloor() ||
            m_stateMachine.HasBeenHit() ||
            m_stateMachine.HasBeenStunned();
        }
        return false;
    }
}
