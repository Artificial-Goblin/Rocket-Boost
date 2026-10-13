using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{
    [SerializeField] float thrustStrenght;
    [SerializeField] float rotationStrength;
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] AudioClip engineExhaustSFX;
    [SerializeField] ParticleSystem engineExhaustParticles;
    [SerializeField] ParticleSystem rightExhaustParticles;
    [SerializeField] ParticleSystem leftExhaustParticles;

    Rigidbody _rigidbody;
    AudioSource audioSource;

    void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    void OnDisable()
    {
        engineExhaustParticles.Stop();
        rightExhaustParticles.Stop();
        leftExhaustParticles.Stop();
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void StartThrusting()
    {
        _rigidbody.AddRelativeForce(Vector3.up * thrustStrenght * Time.fixedDeltaTime);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(engineExhaustSFX);
        }
        if (!engineExhaustParticles.isPlaying)
        {
            engineExhaustParticles.Play();
        }
    }

    private void StopThrusting()
    {
        audioSource.Stop();
        engineExhaustParticles.Stop();
    }

    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();

        if (rotationInput > 0)
        {
            RotateLeft();
        }

        else if (rotationInput < 0)
        {
            RotateRight();
        }

        else
        {
            StopRotating();
        }
    }

    private void RotateLeft()
    {
        ApplyRotation(rotationStrength);

        if (!leftExhaustParticles.isPlaying)
        {
            rightExhaustParticles.Stop();
            leftExhaustParticles.Play();
        }
    }

    private void RotateRight()
    {
        ApplyRotation(-rotationStrength);

        if (!rightExhaustParticles.isPlaying)
        {
            leftExhaustParticles.Stop();
            rightExhaustParticles.Play();
        }
    }

    private void StopRotating()
    {
        rightExhaustParticles.Stop();
        leftExhaustParticles.Stop();
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        _rigidbody.freezeRotation = true;
        transform.Rotate(Vector3.back * rotationThisFrame * Time.fixedDeltaTime);
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    }
}
