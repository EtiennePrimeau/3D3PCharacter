using Unity.VisualScripting;
using UnityEngine;


public class JumpState : CharacterState
{
    private const float GROUNDCHECK_DELAY_TIMER = 1.0f;
    private float m_currentGCDelayTimer = 0.0f;
    private float m_highestPositionY;

    private Vector2 m_jumpHeight;


    public override void OnEnter()
    {
        Debug.Log("Entering JumpState");

        m_stateMachine.Rb.velocity *= 0;
        m_stateMachine.Rb.AddForce(Vector3.up * m_stateMachine.JumpAccelerationValue,
                ForceMode.Acceleration); 

        m_currentGCDelayTimer = GROUNDCHECK_DELAY_TIMER;
        m_highestPositionY = m_stateMachine.Rb.transform.position.y;

        m_stateMachine.TriggerJumpAnimation();

        m_jumpHeight.x = m_stateMachine.transform.position.y;
        m_jumpHeight.y = m_stateMachine.transform.position.y;

        FXManager.Instance.PlaySound(EFXType.McJump, m_stateMachine.transform.position);
    }

    public override void OnFixedUpdate()
    {
        AddForceFromInputs();
        CheckForFallDamage();

        if (m_stateMachine.transform.position.y > m_jumpHeight.y)
        {
            m_jumpHeight.y = m_stateMachine.transform.position.y;
        }
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
        Debug.Log("Exiting JumpState");

        float height = m_jumpHeight.y - m_jumpHeight.x;
        //Debug.Log("Jump: " + height);

        if (m_stateMachine.IsInContactWithFloor())
        {
            FXManager.Instance.PlaySound(EFXType.McLand, m_stateMachine.transform.position);
        }

    }

    public override bool CanEnter(IState currentState)
    {
        if (currentState is FreeState)
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
            //if (m_stateMachine.IsInContactWithFloor())
            //{
            //    FXManager.Instance.PlaySound(EFXType.McLand, m_stateMachine.transform.position);
            //}

            return m_stateMachine.IsInContactWithFloor() || 
                m_stateMachine.HasBeenHit() || 
                m_stateMachine.HasBeenStunned();
        }
        return false;
    }

}
