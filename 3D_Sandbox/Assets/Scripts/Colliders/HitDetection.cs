using UnityEngine;

public class HitDetection : MonoBehaviour
{
    public bool HasBeenHit { get; private set; } = false;
    public bool HasBeenStunned { get; private set; } = false;

    private const float HIT_EXIT_TIMER = 0.5f;
    private float m_currentTimer = 0.0f;
    private bool m_activeTimer = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<EnemyHit>() != null && HasBeenHit == false)
        {
            HasBeenHit = true;
            m_activeTimer = true;
            m_currentTimer = HIT_EXIT_TIMER;
        }
        if (collision.gameObject.GetComponentInParent<EnemyStun>() != null && HasBeenStunned == false)
        {
            HasBeenStunned = true;
            m_activeTimer = true;
            m_currentTimer = HIT_EXIT_TIMER;
        }

    }

    private void Update()
    {
        if (m_activeTimer)
        {
            if (m_currentTimer <= 0)
            {
                HasBeenHit = false;
                HasBeenStunned = false;
                m_activeTimer = false;
                return;
            }

            m_currentTimer -= Time.deltaTime;
        }
    }
}
