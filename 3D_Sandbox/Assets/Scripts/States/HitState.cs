using UnityEngine;

public class HitState : CharacterState
{
    private const float STATE_EXIT_TIMER = 1.0f;
    private float m_currentStateTimer = 0.0f;



    public override void OnEnter()
    {
        Debug.Log("Entering HitState");

        m_currentStateTimer = STATE_EXIT_TIMER;

        Vector3 hitDir = new Vector3(-1, 0, 0);
        m_stateMachine.Rb.AddForce(hitDir * 100, ForceMode.Impulse); //Could be in enemy

        m_stateMachine.TriggerGettingHitAnimation();
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
