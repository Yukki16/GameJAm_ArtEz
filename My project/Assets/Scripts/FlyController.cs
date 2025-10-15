using UnityEngine;
using UnityEngine.InputSystem;

public class FlyController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;

    [Header("Movement")]
    public float speed = 10f;

    [Header("Camera Settings")]
    public float sensitivity = 200f;
    [Range(-80f, -10f)] public float minAngle = -60f;
    [Range(10f, 80f)] public float maxAngle = 60f;

    private Transform cam;
    private float pitch = 0f; // vertical rotation (X axis)

    private void Awake()
    {
        cam = Camera.main.transform;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // prevent physics from affecting rotation
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void OnLook(InputValue lookValue)
    {
        Vector2 look = lookValue.Get<Vector2>();

        // Rotate player horizontally (yaw)
        transform.Rotate(Vector3.up * look.x * sensitivity * Time.deltaTime);

        // Tilt camera vertically (pitch)
        pitch -= look.y * sensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minAngle, maxAngle);

        cam.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void FixedUpdate()
    {
        // Move relative to the player's facing direction
        Vector3 moveDirection = (transform.forward * movementY + transform.right * movementX).normalized;

        rb.AddForce(moveDirection * speed, ForceMode.Acceleration);
    }
}
