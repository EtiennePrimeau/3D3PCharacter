using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float m_destroyTime;
    private float m_timer;

    void Update()
    {
        m_timer += Time.deltaTime;

        if (m_timer > m_destroyTime)
        {
            Destroy(gameObject);
        }
    }
}
