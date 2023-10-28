using System.Collections.Generic;
using UnityEngine;

	public enum EAgentType
	{
		Ally,
		Enemy,
		Neutral,
		Count
	}
public class Hitbox : MonoBehaviour
{
	[field:SerializeField] protected bool CanHit { get; set; }
	[field:SerializeField] protected bool CanReceiveHit { get; set; }
	[field: SerializeField] protected EAgentType AgentType { get; set; } = EAgentType.Count;
	[field: SerializeField] protected List<EAgentType> AffectedAgents { get; set; }


    private void OnCollisionEnter(Collision collision)
    {
		var otherHitbox = collision.gameObject.GetComponent<Hitbox>();

        if (otherHitbox == null)
        {
            return;
        }

        if (CanGetHit(otherHitbox))
        {
            //Debug.Log(gameObject.name + " has hit " + otherHitbox);

			Vector3 contactPoint = collision.GetContact(0).point;
			FXManager.Instance.OnHit(AgentType, contactPoint);
        }
    }

    protected bool CanGetHit(Hitbox otherHitbox)
	{
		return CanHit &&
            otherHitbox.CanReceiveHit && 
			AffectedAgents.Contains(otherHitbox.AgentType);
	}
}
