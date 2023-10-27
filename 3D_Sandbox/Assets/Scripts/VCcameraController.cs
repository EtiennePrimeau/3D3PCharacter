using Cinemachine;
using UnityEngine;

public class VCcameraController : MonoBehaviour
{
    [SerializeField] private Vector2 m_clampingCameraDistance;
    [SerializeField] private Vector2 m_clampingXRotationValues;
    [SerializeField] private CinemachineVirtualCamera m_vcam;

    private float m_targetDistance = 6.0f;

    // Start is called before the first frame update
    void Start()
    {
        //m_vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset.y = 0.5f;
        m_vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset.y = 2.0f;

    }

    // Update is called once per frame
    void Update()
    {
        CalculateDistance();


        //Différence de l'angle à chaque frame
        float currentAngleY = Input.GetAxis("Mouse Y");
        //Valeur de mon transform
        var xRotationValue = transform.rotation.eulerAngles.x;
        //Résultat de ma rotation + différence
        float comparisonAngle = xRotationValue + currentAngleY;

        //S'assure que l'angle n'est pas converti à 360 lorsqu'il atteint 0 (0 étant à l'horizontal)
        //Permet d'avoir une limite du bas en valeur négative
        if (comparisonAngle > 180)
        {
            comparisonAngle -= 360;
        }
        //Early return si les valeurs de l'angle sortent de mon min,max (clamp)
        if ((currentAngleY < 0 && comparisonAngle < m_clampingXRotationValues.x) ||
            (currentAngleY > 0 && comparisonAngle > m_clampingXRotationValues.y))
        {
            return;
        }

        m_vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset.y += currentAngleY;
        //m_vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().VerticalArmLength += currentAngleY;


        //m_lerpedAngleY = Mathf.Lerp(m_lerpedAngleY, currentAngleY, m_lerpF);
        //
        //if (comparisonAngle > m_clampingXRotationValues.x && comparisonAngle < m_clampingXRotationValues.y)
        //{
        //    transform.RotateAround(m_objectToLookAt.position, transform.right, m_lerpedAngleY);
        //}


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

        m_vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = Mathf.Lerp(distance, m_targetDistance, 0.1f);
    }
}
