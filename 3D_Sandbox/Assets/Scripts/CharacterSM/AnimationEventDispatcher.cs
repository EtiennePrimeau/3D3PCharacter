using UnityEngine;

public class AnimationEventDispatcher : MonoBehaviour
{
    [SerializeField] private CapsuleCollider m_collider;

    public void EnableCollider()
    {
        m_collider.enabled = true;
    }
    public void DisableCollider()
    {
        m_collider.enabled = false;
    }
}
