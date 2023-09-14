using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform m_objectToLookAt;
    [SerializeField] private Vector3 m_offset;
    private Vector3 m_targetPosition;
    [SerializeField] private float m_startDistance = 5.0f;
    private float m_targetDistance;
    private float m_lerpedDistance;
    [SerializeField] private float m_scrollSpeed = 0.5f;
    [SerializeField] private float m_lerpF = 0.1f;
    [SerializeField] private float m_rotationSpeed = 5.0f;
    private float m_lerpedAngleX;
    private float m_lerpedAngleY;
    [SerializeField] private Vector2 m_clampingXRotationValues;
    [SerializeField] private Vector2 m_clampingCameraDistance;



    // Start is called before the first frame update
    void Start()
    {
        transform.position = m_objectToLookAt.position + m_offset;
        m_targetDistance = m_startDistance;
    }

    private void FixedUpdate()
    {
        CalculateDistance();
        CalculateTargetPosition();

        RotateAroundObjectHorizontal();
        RotateAroundObjectVertical();


        transform.position = Vector3.Lerp(transform.position, m_targetPosition, m_lerpF);

        MoveCameraInFrontOfObstructionsFUpdate();
    }
    void CalculateDistance()
    {
        float mouseInput = Input.mouseScrollDelta.y * m_scrollSpeed;

        if ((mouseInput < 0 && m_targetDistance > m_clampingCameraDistance.x) ||
            (mouseInput > 0 && m_targetDistance < m_clampingCameraDistance.y))
        {
            m_targetDistance += mouseInput;
        }

        m_lerpedDistance = Mathf.Lerp(m_lerpedDistance, m_targetDistance, m_lerpF);
    }

    void CalculateTargetPosition()
    {
        Vector3 CameraForwardVec = transform.forward;
        CameraForwardVec.Normalize();
        Vector3 desiredCameraOffset = CameraForwardVec * m_lerpedDistance;

        m_targetPosition = m_objectToLookAt.position - desiredCameraOffset;
    }

    void RotateAroundObjectHorizontal()
    {
        //Version réactive
        //float currentAngleX = Input.GetAxis("Mouse X") * m_rotationSpeed;
        //transform.RotateAround(m_objectToLookAt.position, m_objectToLookAt.up, currentAngleX);


        //Version lerped
        float currentAngleX = Input.GetAxis("Mouse X") * m_rotationSpeed;
        m_lerpedAngleX = Mathf.Lerp(m_lerpedAngleX, currentAngleX, m_lerpF);

        transform.RotateAround(m_objectToLookAt.position, m_objectToLookAt.up, m_lerpedAngleX);
    }

    void RotateAroundObjectVertical()
    {
        //Différence de l'angle à chaque frame
        float currentAngleY = Input.GetAxis("Mouse Y") * m_rotationSpeed;
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

        //Version lerped   
        m_lerpedAngleY = Mathf.Lerp(m_lerpedAngleY, currentAngleY, m_lerpF);

        //À vérifier aulieu du early return
        if (comparisonAngle > m_clampingXRotationValues.x && comparisonAngle < m_clampingXRotationValues.y)
        {
            transform.RotateAround(m_objectToLookAt.position, transform.right, m_lerpedAngleY);
        }


    }
    void MoveCameraInFrontOfObstructionsFUpdate()
    {
        int layerMask = 1 << 8;
        RaycastHit hit;

        Vector3 vDiff = transform.position - m_objectToLookAt.position;
        float distance = vDiff.magnitude;

        if (Physics.Raycast(m_objectToLookAt.position, vDiff, out hit, distance, layerMask))
        {
            //Objet détecté
            Debug.DrawRay(m_objectToLookAt.position, vDiff.normalized * hit.distance, Color.yellow);

            transform.SetPositionAndRotation(hit.point, transform.rotation);
        }
        else
        {
            //Objet non détecté
            Debug.DrawRay(m_objectToLookAt.position, vDiff, Color.white);
        }
    }

}