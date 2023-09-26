using UnityEngine;

public class PlayerHBdirection : MonoBehaviour
{
    [field: SerializeField] public Transform Player { get; private set; }
    [field: SerializeField] public Vector3 HitBoxDirection { get; private set; }
    
    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = Player.position;
        HitBoxDirection = Player.position - transform.position;
    }
}
