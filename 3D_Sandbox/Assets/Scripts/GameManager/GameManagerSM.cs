using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSM : BaseStateMachine<GameState>
{
    [SerializeField] protected Camera m_gameplayCamera;
    [SerializeField] protected Camera m_cinematicCamera;
    [SerializeField] protected AnimationCurve m_curve;

    public bool IsInCinematic { get; private set; } = false;

    [field: SerializeField] public CharacterControllerSM CCSM { get; private set; }
    [field: SerializeField] public CinemachineImpulseSource ImpulseSource { get; private set; }

    private float m_currentTimeScaleDuration = 1.0f;
    private bool m_isSlowingDownTime = false;


    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<GameState>();

        m_possibleStates.Add(new GameplayState(m_gameplayCamera));
        m_possibleStates.Add(new CinematicState(m_cinematicCamera));

    }

    protected override void Start()
    {
        //m_impulseSource = GetComponent<CinemachineImpulseSource>();
        
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

        if (m_isSlowingDownTime)
        {
            SlowDownTime();
        }

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

    public void DisableGravity()
    {
        CCSM.Rb.useGravity = false;
    }

    public void EnableGravity()
    {
        CCSM.Rb.useGravity = true;
    }

    public void GenerateCameraShake(float intensity)
    {
        ImpulseSource.GenerateImpulse(intensity);
    }

    public void SlowDownTime()
    {
        if (m_currentTimeScaleDuration < 1.0f)
        {
            m_currentTimeScaleDuration += Time.fixedUnscaledDeltaTime;
            Time.timeScale = m_curve.Evaluate(m_currentTimeScaleDuration);
            Mathf.Clamp(Time.timeScale, 0, 1);
        }
        else if (m_currentTimeScaleDuration > 0.7f)
        {
            m_isSlowingDownTime = false;
        }
    }

    public void SetSlowDownTimeBoolTrue()
    {
        m_isSlowingDownTime = true;
        m_currentTimeScaleDuration = 0.0f;
    }
}
