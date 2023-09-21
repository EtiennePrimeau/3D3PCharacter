using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    public bool IsGrounded { get; private set; } = false;
    public bool TouchingGround { get; private set; } = false;

    private void OnTriggerEnter(Collider other)
    {
        TouchingGround = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsGrounded)
        {
            //Debug.Log("Touching ground");
        }
        //Debug.Log("IsGrounded");
        IsGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Leaving ground");
        IsGrounded = false;
        TouchingGround = false;
    }
}
