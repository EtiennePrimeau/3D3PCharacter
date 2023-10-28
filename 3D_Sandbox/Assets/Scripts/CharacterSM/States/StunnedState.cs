using UnityEngine;

public class StunnedState : CharacterState
{

    private const float STUNNED_DELAY_TIMER = 1.0f;
    private float m_currentStunnedDelayTimer = 0.0f;

    private float m_shakeIntensity = 0.5f;
    private bool m_hasTriggeredAnimation = false;
    private bool m_hasTriggeredFX = false;

    public override void OnEnter()
    {
        Debug.Log("Entering StunnedState");

        m_currentStunnedDelayTimer = STUNNED_DELAY_TIMER;
        m_hasTriggeredAnimation = false;
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
        if (m_stateMachine.IsInContactWithFloor())
        {
            if (!m_hasTriggeredFX)
            {
                FXManager.Instance.PlaySound(EFXType.McStunned, m_stateMachine.transform.position);
                GameManagerSM.Instance.GenerateCameraShake(m_shakeIntensity);
                m_hasTriggeredFX = true;
            }

            m_currentStunnedDelayTimer -= Time.deltaTime;
            if (m_hasTriggeredAnimation == false)
            {
                Debug.Log("Triggering Stunned Animation - Fall Damage");
                m_stateMachine.TriggerIsStunnedAnimation();
                m_hasTriggeredAnimation = true;
            }
        }
    }

    public override void OnExit()
    {
        Debug.Log("Exiting StunnedState");
        m_hasTriggeredFX = false;
    }

    public override bool CanEnter(IState currentState)
    {
        return m_stateMachine.HasBeenStunned();
    }
    public override bool CanExit()
    {
        if (m_currentStunnedDelayTimer <= 0)
        {
            m_stateMachine.SetIsStunnedToFalse();
            return true;
        }
        return false;
    }
}
