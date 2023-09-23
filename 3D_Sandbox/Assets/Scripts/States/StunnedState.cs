using UnityEngine;

public class StunnedState : CharacterState
{

    private const float STUNNED_DELAY_TIMER = 1.0f;
    private float m_currentStunnedDelayTimer = 0.0f;

    public override void OnEnter()
    {
        Debug.Log("Entering StunnedState");

        m_currentStunnedDelayTimer = STUNNED_DELAY_TIMER;

        //Trigger animation
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
        m_currentStunnedDelayTimer -= Time.deltaTime;
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
