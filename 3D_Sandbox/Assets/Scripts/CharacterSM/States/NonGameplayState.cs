using UnityEngine;

public class NonGameplayState : CharacterState
{
    public override void OnEnter()
    {
        Debug.Log("Entering NonGameplayState");
    }

    public override void OnUpdate()
    {
    }

    public override void OnFixedUpdate()
    {
    }


    public override void OnExit()
    {
        Debug.Log("Exiting NonGameplayState");
    }

    public override bool CanEnter(IState currentState)
    {
        return m_stateMachine.IsInNonGameplay;
    }

    public override bool CanExit()
    {
        return !m_stateMachine.IsInNonGameplay;
    }
}
