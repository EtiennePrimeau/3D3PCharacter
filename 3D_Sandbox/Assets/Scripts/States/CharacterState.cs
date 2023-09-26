
public abstract class CharacterState : IState
{

    protected CharacterController m_stateMachine;
    public void OnStart(CharacterController controller)
    {
        m_stateMachine = controller;
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnFixedUpdate()
    {
    }

    public virtual void OnUpdate()
    {
    }

    public virtual void OnExit()
    {
    }

    public virtual bool CanEnter(IState currentState)
    {
        return true;
    }
    public virtual bool CanExit()
    {
        return true;
    }

}
