using Cinemachine;
using UnityEngine;

public class GameplayState : GameState
{
    protected CinemachineVirtualCamera m_camera;

    public GameplayState(CinemachineVirtualCamera camera)
    {
        m_camera = camera;
    }

    public override bool CanEnter(IState currentState)
    {
        return !m_stateMachine.IsInCinematic;
    }

    public override bool CanExit()
    {
        return m_stateMachine.IsInCinematic;
    }

    public override void OnEnter()
    {
        Debug.Log("Entering Gameplay State");
        m_camera.gameObject.SetActive(true);

        CharacterControllerSM.Instance.SetNonGameplayState(false);
    }

    public override void OnExit()
    {
        Debug.Log("Exiting Gameplay State");
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }
}
