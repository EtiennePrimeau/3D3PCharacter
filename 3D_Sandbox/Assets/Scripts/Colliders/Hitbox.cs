using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
	/*
	bool canHit
	bool canReceiveHit
	AgentType agentType = Count
	list<AgentType> affectedAgentTypes


	enum AgentType
	Ally
	Enemy
	Neutral
	Count

	OnTriggerEnter
	var otherHitbox = other.GetComponent<Hitbox>()
	if otherHitbox == null
		return

	//Mettre dans une autre fonction
	if canHit && other.canReceiveHit
		if affectedAgentTypes.Contains(otherHitbox.agentType)
			otherHitbox.GetHit()
	*/

	[field:SerializeField] protected bool CanHit { get; set; }
	[field:SerializeField] protected bool CanReceiveHit { get; set; }
	[field: SerializeField] protected EAgentType AgentType { get; set; } = EAgentType.Count;
	[field: SerializeField] protected List<EAgentType> AffectedAgents { get; set; }

	protected enum EAgentType
	{
		Ally,
		Enemy,
		Neutral,
		Count
	}

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

        }

    }

    protected bool CanGetHit(Hitbox otherHitbox)
	{
		return CanHit &&
            otherHitbox.CanReceiveHit && 
			AffectedAgents.Contains(otherHitbox.AgentType);
	}
}
