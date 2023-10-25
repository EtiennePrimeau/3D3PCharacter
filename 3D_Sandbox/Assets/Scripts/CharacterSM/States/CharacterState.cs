
public abstract class CharacterState : IState
{

    protected CharacterControllerSM m_stateMachine;
    public void OnStart(CharacterControllerSM controller)
    {
        m_stateMachine = controller;
    }

    public void OnStart()
    {
        throw new System.NotImplementedException();
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
