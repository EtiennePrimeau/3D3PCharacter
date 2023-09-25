using UnityEngine;

public class StunnedState : CharacterState
{

    private const float STUNNED_DELAY_TIMER = 1.0f;
    private float m_currentStunnedDelayTimer = 0.0f;

    private bool m_hasTriggeredAnimation = false;

    public override void OnEnter()
    {
        Debug.Log("Entering StunnedState");

        m_currentStunnedDelayTimer = STUNNED_DELAY_TIMER;
        m_hasTriggeredAnimation = false;
        //Trigger animation
        //m_stateMachine.TriggerIsStunnedAnimation();
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
        if (m_stateMachine.IsInContactWithFloor())
        {
            m_currentStunnedDelayTimer -= Time.deltaTime;
            if (m_hasTriggeredAnimation == false)
            {
                Debug.Log("Triggering Stunned Animation - Fall Damage");
                m_stateMachine.TriggerIsStunnedAnimation();
                m_hasTriggeredAnimation = true;
            }
        }
    }

    public override void OnExit()
    {
        Debug.Log("Exiting StunnedState");
    }

    public override bool CanEnter(IState currentState)
    {
        return m_stateMachine.HasBeenStunned();
    }
    public override bool CanExit()
    {
        if (m_currentStunnedDelayTimer <= 0)
        {
            m_stateMachine.SetIsStunnedToFalse();
            return true;
        }
        return false;
    }
}
