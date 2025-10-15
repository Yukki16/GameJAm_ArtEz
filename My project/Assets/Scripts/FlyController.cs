using FMOD.Studio;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlyController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;

    [Header("Movement")]
    public float speed = 10f;
    public float verticalSpeed = 1f;
    float verticalM;

    [Header("Camera Settings")]
    public float sensitivity = 200f;
    [Range(-80f, -10f)] public float minAngle = -60f;
    [Range(10f, 80f)] public float maxAngle = 60f;


    Pulsing sonar;
    private Transform cam;
    private float pitch = 0f; // vertical rotation (X axis)

    //Audio
    EventInstance flightAudio;

    EventInstance metroDistant;

    private void Awake()
    {
        sonar = GetComponentInChildren<Pulsing>();
        cam = Camera.main.transform;

        AudioManager.instance.InitializeAmbience(FMODEvents.instance.ambience);
        AudioManager.instance.SetAmbienceLabelParameter("Level", "This");
        AudioManager.instance.SetAmbienceParameter("Volume", 0.3f);

        flightAudio = AudioManager.instance.CreateInstance(FMODEvents.instance.playerFlight);

        AudioManager.instance.PlayOneShot(FMODEvents.instance.narrator, this.transform.position);
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

    void OnVerticalM(InputValue verticalMValue)
    {
       verticalM = verticalMValue.Get<float>(); 
    }

    void OnSonar(InputValue sonarValue)
    {
        sonar.pulse();
        AudioManager.instance.PlayOneShot(FMODEvents.instance.sonar, this.transform.position);
        //It already casts so invoke here whatever you want it to happen.
    }

    private void FixedUpdate()
    {
        // Move relative to the player's facing direction
        Vector3 moveDirection = (transform.forward * movementY + transform.right * movementX).normalized + new Vector3(0, verticalM * verticalSpeed, 0).normalized;

        rb.AddForce(moveDirection * speed, ForceMode.Acceleration);

        UpdateSound();
    }

    private void UpdateSound()
    {
        if (rb.linearVelocity.magnitude > 0)
        {
            PLAYBACK_STATE playbackstate;
            flightAudio.getPlaybackState(out playbackstate);

            if(playbackstate.Equals(PLAYBACK_STATE.STOPPED))
            {
                flightAudio.start();
            }
        }
        else
        {
            flightAudio.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}
