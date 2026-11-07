using UnityEngine;
using UnityEngine.InputSystem;

public class SpinningCylinder : MonoBehaviour
{
    [Header("Velocities")]
    [SerializeField] private float keyboardRotationSpeed = 150.0f;
    [SerializeField] private float dragSpeed = 0.3f;

    [Header("Input Actions")]
    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction lookDeltaAction;
    [SerializeField] private InputAction clickAction;

    private void OnEnable()
    {
        moveAction.Enable();
        lookDeltaAction.Enable();
        clickAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookDeltaAction.Disable();
        clickAction.Disable();
    }

    public void Update()
    {
        float horizontalInput = moveAction.ReadValue<float>();
        if (Mathf.Abs(horizontalInput) > 0.01f)
        {
            transform.Rotate(Vector3.forward, -horizontalInput * keyboardRotationSpeed * Time.deltaTime);
        }

        if (clickAction.IsPressed())
        {
            float pointerDeltaX = lookDeltaAction.ReadValue<float>();
            transform.Rotate(Vector3.forward, -pointerDeltaX * dragSpeed);
        }
        
    }
}