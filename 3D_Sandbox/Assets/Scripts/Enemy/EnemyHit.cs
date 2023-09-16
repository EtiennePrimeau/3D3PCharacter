using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public CapsuleCollider CapsuleColliderTrigger { get; private set; }

    private const float MAX_TIMER = 2.0f; 
    private float m_timer;

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
            CapsuleColliderTrigger.enabled = true;
            return;
        }

        if (m_timer <= 1.2f)
        {
            CapsuleColliderTrigger.enabled = false;
        }

        Animator.SetFloat("timer", m_timer);
        m_timer -= Time.deltaTime;
    }
}
