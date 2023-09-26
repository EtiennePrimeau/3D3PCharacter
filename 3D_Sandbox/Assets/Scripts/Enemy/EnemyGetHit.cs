using UnityEngine;

public class EnemyGetHit : MonoBehaviour
{

    [field:SerializeField] public PlayerHBdirection PlayerHitBoxDirection { get; private set; }
    [field:SerializeField] public Rigidbody Rb { get; private set; }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerHBdirection>() != null)
        {
            Rb.AddForce(-PlayerHitBoxDirection.HitBoxDirection * 10, ForceMode.Impulse);
        }
    }

}
