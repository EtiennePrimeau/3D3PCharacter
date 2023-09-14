using UnityEngine;

public class HitState : CharacterState
{
    private const float STATE_EXIT_TIMER = 1.0f;
    private float m_currentStateTimer = 0.0f;



    public override void OnEnter()
    {
        Debug.Log("Entering HitState");

        m_currentStateTimer = STATE_EXIT_TIMER;
    }

    public override void OnUpdate()
    {
        m_currentStateTimer -= Time.deltaTime;
    }

    public override void OnFixedUpdate()
    {
        AddForceFromInputs();
    }

    private void AddForceFromInputs()
    {
        float inputForward = 0.0f;
        float inputRight = 0.0f;

        if (Input.GetKey(KeyCode.W))
        {
            inputForward += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputForward -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputRight -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputRight += 1;
        }

        m_stateMachine.Rb.AddForce(inputForward * m_stateMachine.ForwardVectorForPlayer * m_stateMachine.SlowedDownAccelerationValue,
                ForceMode.Acceleration);
        m_stateMachine.Rb.AddForce(inputRight * m_stateMachine.RightVectorForPlayer * m_stateMachine.SlowedDownAccelerationValue,
                ForceMode.Acceleration);
    }

    public override void OnExit()
    {
        Debug.Log("Exiting HitState");
    }

    public override bool CanEnter()
    {
        return m_stateMachine.HasBeenHit();
    }

    public override bool CanExit()
    {
        return m_currentStateTimer <= 0;
    }

}
