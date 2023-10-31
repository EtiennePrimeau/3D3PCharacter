using Cinemachine;
using UnityEngine;

public class CinematicState : GameState
{
    protected CinemachineVirtualCamera m_camera;

    public CinematicState(CinemachineVirtualCamera camera)
    {
        m_camera = camera;
    }

    public override bool CanEnter(IState currentState)
    {
        return m_stateMachine.IsInCinematic;
    }

    public override bool CanExit()
    {
        return !m_stateMachine.IsInCinematic;
    }

    public override void OnEnter()
    {
        Debug.Log("Entering Cinematic State");

        CharacterControllerSM.Instance.SetNonGameplayState(true);
    }

    public override void OnExit()
    {
        Debug.Log("Exiting Cinematic State");
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }
}
