using UnityEngine;

public class CinematicState : IState
{

    protected Camera m_camera;

    public CinematicState(Camera camera)
    {
        m_camera = camera;
    }

    public bool CanEnter(IState currentState)
    {
        return Input.GetKeyDown(KeyCode.G);
    }

    public bool CanExit()
    {
        return Input.GetKeyDown(KeyCode.G);
    }

    public void OnEnter()
    {
        Debug.Log("Entering Cinematic State");
        m_camera.enabled = true;
    }

    public void OnExit()
    {
        Debug.Log("Exiting Cinematic State");
        m_camera.enabled = false;
    }

    public void OnFixedUpdate()
    {
    }

    public void OnStart()
    {
    }

    public void OnUpdate()
    {
    }
}
