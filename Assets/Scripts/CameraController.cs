using UnityEngine;
using UnityEngine.InputSystem; // ✅ Required for new input system

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform playerCamera;
    public float mouseSensitivity = 5f;
    public float smoothing = 2f;
    public float minVerticalAngle = -85f;
    public float maxVerticalAngle = 85f;

    private Vector2 smoothMouse;
    private Vector2 currentLookingPos;

    private Vector2 mouseInput; // stores mouse delta from input system

    private void OnEnable()
    {
        // Enable input system
        Mouse.current?.MakeCurrent();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // ✅ Get mouse delta from the new Input System
        if (Mouse.current != null)
        {
            mouseInput = Mouse.current.delta.ReadValue();
        }

        // Smooth it
        smoothMouse = Vector2.Lerp(smoothMouse, mouseInput, 1f / smoothing);
        currentLookingPos += smoothMouse * mouseSensitivity * Time.deltaTime;

        // Clamp vertical rotation
        currentLookingPos.y = Mathf.Clamp(currentLookingPos.y, minVerticalAngle, maxVerticalAngle);

        // Apply rotations
        playerCamera.localRotation = Quaternion.Euler(-currentLookingPos.y, 0, 0);
        transform.rotation = Quaternion.Euler(0, currentLookingPos.x, 0);
    }
}
