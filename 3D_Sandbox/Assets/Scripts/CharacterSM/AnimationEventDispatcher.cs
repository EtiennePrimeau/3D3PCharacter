using UnityEngine;

public class AnimationEventDispatcher : MonoBehaviour
{

    [SerializeField] private CharacterControllerSM m_stateMachine;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateCharacterAttackHitBox()
    {
        m_stateMachine.HandleAttackHitbox(true);
    }

    public void DeactivateCharacterAttackHitBox()
    {
        m_stateMachine.HandleAttackHitbox(false);
    }
}
