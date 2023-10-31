using Cinemachine;
using UnityEngine;

public class VCcameraController : MonoBehaviour
{
    [SerializeField] private Vector2 m_clampingCameraDistance;
    [SerializeField] private Vector2 m_clampingXRotationValues;
    [SerializeField] private CinemachineVirtualCamera m_vcam;

    private float m_targetDistance = 6.0f;
    private float m_lerpF = 0.1f;

    void Start()
    {
        m_vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset.y = 2.0f;

    }

    void Update()
    {
        CalculateDistance();

        RotateCameraVertically();
    }

    private void RotateCameraVertically()
    {
        float currentAngleY = Input.GetAxis("Mouse Y");
        var xRotationValue = transform.rotation.eulerAngles.x;
        float comparisonAngle = xRotationValue + currentAngleY;

        if (comparisonAngle > 180)
        {
            comparisonAngle -= 360;
        }
        if ((currentAngleY < 0 && comparisonAngle < m_clampingXRotationValues.x) ||
            (currentAngleY > 0 && comparisonAngle > m_clampingXRotationValues.y))
        {
            return;
        }

        m_vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset.y += currentAngleY;
    }

    private void CalculateDistance()
    {
        float mouseInput = Input.mouseScrollDelta.y;
        float distance = m_vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance;

        if ((mouseInput < 0 && m_targetDistance > m_clampingCameraDistance.x) ||
            (mouseInput > 0 && m_targetDistance < m_clampingCameraDistance.y))
        {
            m_targetDistance += mouseInput;
        }

        m_vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = Mathf.Lerp(distance, m_targetDistance, m_lerpF);
    }
}
