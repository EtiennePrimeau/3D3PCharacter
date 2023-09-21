using UnityEngine;

public class InAirState : CharacterState
{
    public override void OnEnter()
    {
        Debug.Log("Entering InAirState");

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
    }

    public override void OnExit()
    {
        Debug.Log("Exiting InAirState");
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
        return m_stateMachine.IsInContactWithFloor();
    }
}
