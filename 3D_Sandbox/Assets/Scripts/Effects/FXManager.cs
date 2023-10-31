using System.Collections.Generic;
using UnityEngine;

public enum EFXType
{
    McHit,
    EnemyHit,
    McJump,
    McLand,
    McStunned,
    Count
}

public class FXManager : MonoBehaviour
{
    public static FXManager Instance { get; private set; }

    [SerializeField] private List<FXEvent> m_fXEventsList = new List<FXEvent>();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void OnHit(EAgentType agent, Vector3 position)
    {
        switch (agent)
        {
            case EAgentType.Ally:
                PlayHitFX(m_fXEventsList[(int)EFXType.McHit], position);
                GameManagerSM.Instance.SetSlowDownTimeBoolTrue();
                break;
            case EAgentType.Enemy:
                PlayHitFX(m_fXEventsList[(int)EFXType.EnemyHit], position);
                break;

            case EAgentType.Neutral:
            case EAgentType.Count:
            default:
                break;
        }
    }

    private void PlayHitFX(FXEvent fx, Vector3 position)
    {
        var newObject = Instantiate(fx.go, position, Quaternion.identity);
        var audioSource = newObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(fx.clip);

        GameManagerSM.Instance.GenerateCameraShake(fx.shakeIntensity);
    }

    public void PlaySound(EFXType type, Vector3 position)
    {
        FXEvent fx = m_fXEventsList[(int)type];

        var newObject = Instantiate(fx.go, position, Quaternion.identity);
        var audioSource = newObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(fx.clip);
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
