using UnityEngine;

public class CinematicState : GameState
{

    protected Camera m_camera;

    //protected bool m_hasExitedCinematic = false;
    //protected bool m_hasStartedCinematic = false;

    //[field: SerializeField] public CharacterControllerSM CCSM { get; private set; }

    public CinematicState(Camera camera)
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
        //m_hasStartedCinematic = false;
        //m_camera.enabled = true;

        m_stateMachine.CCSM.SetNonGameplayState(true);
    }

    public override void OnExit()
    {
        Debug.Log("Exiting Cinematic State");
        //m_hasExitedCinematic = false;
        //m_camera.enabled = false;
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }
}
