using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    public enum EFXType
    {
        McHit,
        EnemyHit,
        Count
    }

    [SerializeField] private List<FXEvent> m_fXEventsList = new List<FXEvent>();
    [SerializeField] private GameManagerSM m_gmSM;


    public void OnHit(EAgentType agent, Vector3 position)
    {
        switch (agent)
        {
            case EAgentType.Ally:
                PlayFX(m_fXEventsList[(int)EFXType.McHit], position);
                m_gmSM.SetSlowDownTimeBoolTrue();
                break;
            case EAgentType.Enemy:
                PlayFX(m_fXEventsList[(int)EFXType.EnemyHit], position);
                break;

            case EAgentType.Neutral:
            case EAgentType.Count:
            default:
                break;
        }
    }

    private void PlayFX(FXEvent fx, Vector3 position)
    {
        var newObject = Instantiate(fx.go, position, Quaternion.identity);
        var audioSource = newObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(fx.clip);

        m_gmSM.GenerateCameraShake(fx.shakeIntensity);
    }

    [System.Serializable]
    public struct FXEvent
    {
        public EFXType type;
        public AudioClip clip;
        public GameObject go;
        public float shakeIntensity;
    }
}
