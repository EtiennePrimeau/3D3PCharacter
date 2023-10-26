using Cinemachine;
using UnityEngine;

public class VCcameraController : MonoBehaviour
{
    //[SerializeField] private Camera m_camera;
    [SerializeField] private float m_scrollSpeed;
    [SerializeField] private float m_targetDistance;
    [SerializeField] private Vector2 m_clampingCameraDistance;

    [SerializeField] private CinemachineVirtualCamera m_vcam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseInput = Input.mouseScrollDelta.y * m_scrollSpeed;
        float cameraScroll = 0.0f;
        float distance = m_vcam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z;


        if ((mouseInput < 0 && distance > m_clampingCameraDistance.x) ||
            (mouseInput > 0 && distance < m_clampingCameraDistance.y))
        {
            cameraScroll += mouseInput;
        }

        m_vcam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z += cameraScroll;

    }
}
