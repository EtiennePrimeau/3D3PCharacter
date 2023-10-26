using UnityEngine;

public class GameplayState : GameState
{

    protected Camera m_camera;

    //private bool m_hasStartedGameplay = false;
    //private bool m_hasExitedGameplay = false;

    //[field: SerializeField] public CharacterControllerSM CCSM { get; private set; }


    public GameplayState(Camera camera)
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
        //m_hasStartedGameplay = false;
        m_camera.gameObject.SetActive(true);

        m_stateMachine.CCSM.SetNonGameplayState(false);
    }

    public override void OnExit()
    {
        Debug.Log("Exiting Gameplay State");
        //m_hasExitedGameplay = false;
        //m_camera.enabled = false;
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }
}
