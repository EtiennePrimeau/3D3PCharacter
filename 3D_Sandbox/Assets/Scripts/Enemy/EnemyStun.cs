using UnityEngine;

public class EnemyStun : MonoBehaviour
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public SphereCollider SphereColliderTrigger { get; private set; }

    private const float MAX_TIMER = 2.0f;
    private float m_timer;

    private float m_activationTime = 1.5f;
    private float m_deactivationTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_timer = MAX_TIMER;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_timer < -0.1f)
        {
            m_timer = MAX_TIMER;
            return;
        }
        if (m_timer <= m_activationTime && m_timer >= m_deactivationTime)
        {
            SphereColliderTrigger.enabled = true;
        }
        else
        {
            SphereColliderTrigger.enabled = false;
        }

        Animator.SetFloat("timer", m_timer);
        m_timer -= Time.deltaTime;
    }
}
