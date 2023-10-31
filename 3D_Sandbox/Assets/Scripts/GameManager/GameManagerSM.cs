using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class GameManagerSM : BaseStateMachine<GameState>
{
    public static GameManagerSM Instance { get; private set; }
    
    [SerializeField] protected CinemachineVirtualCamera m_gameplayCamera;
    [SerializeField] protected CinemachineVirtualCamera m_cinematicCamera;
    [SerializeField] protected AnimationCurve m_curve;
    [SerializeField] protected PlayableDirector m_timeline;
    [field: SerializeField] public CinemachineImpulseSource ImpulseSource { get; private set; }
    public bool IsInCinematic { get; private set; } = false;

    private float m_currentTimeScaleDuration = 1.0f;
    private bool m_isSlowingDownTime = false;


    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<GameState>();

        m_possibleStates.Add(new CinematicState(m_cinematicCamera));
        m_possibleStates.Add(new GameplayState(m_gameplayCamera));

    }

    protected override void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        base.Awake();
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    protected override void Start()
    {
        foreach (GameState state in m_possibleStates)
        {
            state.OnStart(this);
        }
        
        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();
    }
    
    protected override void Update()
    {
        base.Update();

        if (Input.GetKey(KeyCode.N) && m_currentState is GameplayState)
        {
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);

            m_timeline.Play();
        }
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
        CharacterControllerSM.Instance.Rb.useGravity = false;
    }

    public void EnableGravity()
    {
        CharacterControllerSM.Instance.Rb.useGravity = true;
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

    public void ResetCameraFollowAndLookAt()
    {
        var player = GameObject.FindWithTag("Player");
    
        m_gameplayCamera.m_Follow = player.transform;
        m_gameplayCamera.m_LookAt = player.transform;
        m_cinematicCamera.m_LookAt = player.transform;
    
        var dollyTrack = GameObject.FindWithTag("DollyTrack");
    
        m_cinematicCamera.m_Follow = dollyTrack.transform;
    }

    public void StopTimeline()
    {
        m_timeline.Stop();
    }
}
