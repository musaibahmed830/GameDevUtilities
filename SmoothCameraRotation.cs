using UnityEngine;

public class SmoothCameraRotation : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;        
    public float distance = 5f; 

    [Header("Rotation Settings")]
    public float rotationSpeed = 5f;
    public float smoothness = 0.1f; 
    public Vector3 startPos;
    private float currentRotationX = 0f; 
    private float currentRotationY = 0f;
    private float targetRotationX = 0f; 
    private float targetRotationY = 0f; 
    private float rotationVelocityX;   
    private float rotationVelocityY;  
    public float minYPosition = 1f;
    public float maxYPosition = 4f;
    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        targetRotationX = angles.x;
        targetRotationY = angles.y;
        transform.position = startPos;
        UpdateCameraPosition();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            targetRotationY += mouseX * rotationSpeed;
            targetRotationX -= mouseY * rotationSpeed;
            targetRotationX = Mathf.Clamp(targetRotationX, -80f, 80f);
        }
        currentRotationX = Mathf.SmoothDampAngle(currentRotationX, targetRotationX, ref rotationVelocityX, smoothness);
        currentRotationY = Mathf.SmoothDampAngle(currentRotationY, targetRotationY, ref rotationVelocityY, smoothness);
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        if (target == null) return;
        Quaternion rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0f);
        Vector3 position = target.position - (rotation * Vector3.forward * distance);
        position.y = Mathf.Clamp(position.y, minYPosition, maxYPosition);
        transform.position = position;
        transform.LookAt(target);
    }
}
