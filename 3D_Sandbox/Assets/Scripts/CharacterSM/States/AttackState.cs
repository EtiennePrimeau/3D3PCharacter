using UnityEngine;

public class AttackState : CharacterState
{

    private const float ATTACK_DELAY_TIMER = 1.0f;
    private float m_currentAttackDelayTimer = 0.0f;

    public override void OnEnter()
    {
        Debug.Log("Entering AttackState");

        //m_stateMachine.HitBox.enabled = true;
        m_currentAttackDelayTimer = ATTACK_DELAY_TIMER;

        m_stateMachine.TriggerIsAttackingAnimation();
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
        m_currentAttackDelayTimer -= Time.deltaTime;
    }

    public override void OnExit()
    {
        Debug.Log("Exiting AttackState");

        //m_stateMachine.HitBox.enabled = false;
        m_stateMachine.HandleAttackHitbox(false);
    }

    public override bool CanEnter(IState currentState)
    {
        if (currentState is FreeState)
        {
            return Input.GetKeyDown(KeyCode.LeftShift);
        }
        return false;
    }
    public override bool CanExit()
    {
        return m_currentAttackDelayTimer <= 0;
    }
}
