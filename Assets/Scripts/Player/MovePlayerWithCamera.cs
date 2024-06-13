using UnityEngine;

public class MovePlayerWithCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public float rotationSpeed = 20f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;
        if (cameraForward != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
