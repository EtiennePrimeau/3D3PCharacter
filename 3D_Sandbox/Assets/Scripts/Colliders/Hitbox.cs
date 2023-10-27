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
	[field: SerializeField] protected FXManager FxManager { get; set; }


    /*private void OnTriggerEnter(Collider other)
    {
		var otherHitbox = other.GetComponent<Hitbox>();
		if (otherHitbox == null)
		{
			return;
		}

		if (CanGetHit(otherHitbox))
		{
			Debug.Log(gameObject.name + " has hit " + otherHitbox);

		}

    }*/

    private void OnCollisionEnter(Collision collision)
    {
		var otherHitbox = collision.gameObject.GetComponent<Hitbox>();


		/*
		 contacts[0].point + transform.LookAt
		 contacts[0].normal
		 */
		
        if (otherHitbox == null)
        {
            return;
        }

        if (CanGetHit(otherHitbox))
        {
            Debug.Log(gameObject.name + " has hit " + otherHitbox);

			Vector3 contactPoint = collision.GetContact(0).point;
			FxManager.OnHit(AgentType, contactPoint);

			

			//Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), contactPoint, Quaternion.identity);
        }

    }

    protected bool CanGetHit(Hitbox otherHitbox)
	{
		return CanHit &&
            otherHitbox.CanReceiveHit && 
			AffectedAgents.Contains(otherHitbox.AgentType);
	}
}
