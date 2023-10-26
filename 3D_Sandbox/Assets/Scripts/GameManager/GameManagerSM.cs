using System.Collections.Generic;
using UnityEngine;

public class GameManagerSM : BaseStateMachine<GameState>
{
    [SerializeField] protected Camera m_gameplayCamera;
    [SerializeField] protected Camera m_cinematicCamera;

    public bool IsInCinematic { get; private set; } = true;

    [field: SerializeField] public CharacterControllerSM CCSM { get; private set; }


    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<GameState>();

        m_possibleStates.Add(new CinematicState(m_cinematicCamera));
        m_possibleStates.Add(new GameplayState(m_gameplayCamera));

    }

    protected override void Start()
    {
        foreach (GameState state in m_possibleStates)
        {
            state.OnStart(this);
        }
        
        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();
        
        //base.Start(); //
    }
    
    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void SetCinematicBoolToTrue()
    {
        IsInCinematic = true;
    }
    
    public void SetCinematicBoolToFalse()
    {
        IsInCinematic = false;
        Debug.Log("bool");
    }
}
