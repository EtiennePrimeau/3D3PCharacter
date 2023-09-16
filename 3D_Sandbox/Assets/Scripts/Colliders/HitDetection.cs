using UnityEngine;

public class HitDetection : MonoBehaviour
{
    public bool HasBeenHit { get; private set; } = false;

    private const float HIT_EXIT_TIMER = 1.0f;
    private float m_currentTimer = 0.0f;
    private bool m_activeTimer = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyHit")
        {
            Debug.Log("Hit Enemy");
            HasBeenHit = true;
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
                m_activeTimer = false;
                Debug.Log("Timer done");
                return;
            }

            m_currentTimer -= Time.deltaTime;
        }
    }
}
