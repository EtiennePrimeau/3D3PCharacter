using UnityEngine;

public class Elevator : MonoBehaviour
{
    [field: SerializeField] private float VerticalSpeed { get; set; } = 2.0f;
    [field: SerializeField] private float MaxHeight { get; set; } = 20.0f;
    
    private Vector3 m_position;

    // Start is called before the first frame update
    void Start()
    {
        m_position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        m_position.y += VerticalSpeed * Time.deltaTime;

        if (m_position.y > MaxHeight)
        {
            m_position.y = 0.0f;
        }

        transform.position = m_position;
    }
}
