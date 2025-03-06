using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine; // Make sure to include this

public class PlayerKartController : MonoBehaviour
{
    public int ID;
    Player playerID;
    [SerializeField] private PlayerInput playerKartInput; // Reference to PlayerInput component
    public bool moveForward;
    public bool moveBackward;
    public bool drift;
    public bool drifting;
    public float kartYPosition;
    public Transform kartNormal;
    public Transform kartModel;
    public Transform frontThrow;

    public Rigidbody sphere;
    public float speed, currentSpeed;
    float rotate, currentRotate;
    public int driftDirection;
    float driftPower;
    int driftMode = 0;
    //bool first, second, third;

    [Header("Parameters")]
    public float topSpeed;
    public float acceleration;
    public float deceleration;
    public float steering;
    public float gravity;
    public LayerMask layerMask;

    [Header("Drift Parameters")]
    public float level1Threshold = 50f;
    public float level2Threshold = 100f;
    public float level3Threshold = 150f;

    public float level1Boost = 20f;
    public float level2Boost = 40f;
    public float level3Boost = 60f;
    public float driftBoostAmount = 0f;
    public float boostDuration = 2f;
    public float boostTimer = 0f;

    public float driftAssistStrength = 0.5f; // Strength of steering assist during drift
    public float driftAssistAngle = 15f; // Angle threshold for drift assist activation

    [Header("Drift Particle")]
    public GameObject[] wheelsParticle;
    public ParticleSystem[] particleSystem;
    public ParticleSystem[] flareParticle;
    public ParticleSystem[] boostParticle;
    public Color newColor;
    public Color[] driftColor;
    public CinemachineVirtualCamera virtualCam;
    private float defaultFOV;
    public float boostFOV; // Adjust this value for a stronger zoom effect
    public float zoomOutSpeed; // Controls how fast the zoom effect happens
    public float ZoomInSpeed; // Controls how fast the zoom effect happens

    void Awake()
    {
        playerID = GetComponentInParent<Player>();
        ID = playerID.id;
        if (playerKartInput == null)
        {
            playerKartInput = GetComponentInParent<PlayerInput>();
        }
        // Assign input actions dynamically
        playerKartInput.actions["Forward"].started += ctx => moveForward = true;
        playerKartInput.actions["Forward"].performed += ctx => moveForward = true;
        playerKartInput.actions["Forward"].canceled += ctx => moveForward = false;

        playerKartInput.actions["Backward"].started += ctx => moveBackward = true;
        playerKartInput.actions["Backward"].canceled += ctx => moveBackward = false;

        playerKartInput.actions["Drift"].started += ctx => drift = true;
        playerKartInput.actions["Drift"].canceled += ctx => drift = false;

        if (virtualCam != null)
        {
            defaultFOV = virtualCam.m_Lens.FieldOfView;
            virtualCam.Follow = this.transform;
            virtualCam.LookAt = this.transform;
        }

        if (this.gameObject.activeSelf == true)
        {
            this.sphere.GetComponent<PlayerItemHolder>().itemFront = frontThrow;
            virtualCam.Follow = this.transform;
            virtualCam.LookAt = this.transform;
        }
    }

    void Update()
    {
        //Debug.Log("Current Speed: " + currentSpeed);

        if (moveForward && !moveBackward)
        {
            speed = topSpeed;
        }
        else if (moveBackward && !moveForward)
        {
            speed = -topSpeed;
        }
        else
        {
            speed = 0f;
        }


        Vector3 moveInput = playerKartInput.actions["Move"].ReadValue<Vector2>();
        // Steer
        if (moveInput.x != 0)
        {
            int dir = moveInput.x > 0 ? 1 : -1;
            float amount = Mathf.Abs(moveInput.x);
            Steer(dir, amount);
        }

        if (drift && !drifting && moveInput.x != 0)
        {
            drifting = true;
            driftDirection = moveInput.x > 0 ? 1 : -1;
        }

        if (drifting)
        {
            float control = (driftDirection == 1) ? ExtensionMethods.Remap(moveInput.x, -1, 1, 0, 2) : ExtensionMethods.Remap(moveInput.x, -1, 1, 2, 0);
            float powerControl = (driftDirection == 1) ? ExtensionMethods.Remap(moveInput.x, -1, 1, .2f, 1) : ExtensionMethods.Remap(moveInput.x, -1, 1, 1, .2f);
            Steer(driftDirection, control);
            driftPower += powerControl * Time.deltaTime * 100f;

            wheelsParticle[0].SetActive(true);
            wheelsParticle[1].SetActive(true);

            // Only update the color if it changes
            if (driftPower >= level3Threshold && newColor != driftColor[2])
            {
                newColor = driftColor[2];
                ChangeParticleColor();
                PlayFlareParticle();
            }
            else if (driftPower >= level2Threshold && newColor != driftColor[1] && driftPower < level3Threshold)
            {
                newColor = driftColor[1];
                ChangeParticleColor();
                PlayFlareParticle();
            }
            else if (driftPower >= level1Threshold && newColor != driftColor[0] && driftPower < level2Threshold)
            {
                newColor = driftColor[0];
                ChangeParticleColor();
                PlayFlareParticle();
            }
        }


        if (!drift && drifting)
        {
            Boost();
            wheelsParticle[0].SetActive(false);
            wheelsParticle[1].SetActive(false);
        }

        transform.position = sphere.transform.position - new Vector3(0, kartYPosition, 0);

    }

    private void FixedUpdate()
    {
        if (speed == 0f && boostTimer <= 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.fixedDeltaTime * acceleration);
        }

        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.fixedDeltaTime * 4f);
        rotate = 0f;

        sphere.AddForce(transform.forward * currentSpeed, ForceMode.Acceleration);
        sphere.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        if (currentSpeed != 0)
        {
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + currentRotate, 0), Time.fixedDeltaTime * 5f);
        }

        // Apply Boost Timer
        if (boostTimer > 0)
        {
            boostTimer -= Time.fixedDeltaTime;

            if (boostTimer <= 0)
            {
                currentSpeed = Mathf.Max(currentSpeed - level3Boost, acceleration);
            }
        }

        if (boostTimer > 0)
        {
            boostTimer -= Time.fixedDeltaTime;
            currentSpeed += driftBoostAmount;
        }
        else
        {
            driftBoostAmount = 0;
        }

        // Apply Drift Assist
        if (drifting)
        {
            ApplyDriftAssist();
        }

        AlignKartToGround();
    }

    public void Steer(int direction, float amount)
    {
        rotate = (steering * direction) * amount;
    }

    public void Boost()
    {
        drifting = false;
        bool boosted = false;

        if (driftPower >= level3Threshold)
        {
            driftMode = 3;
            StartBoost(level3Boost);
            boosted = true;
        }
        else if (driftPower >= level2Threshold)
        {
            driftMode = 2;
            StartBoost(level2Boost);
            boosted = true;
        }
        else if (driftPower >= level1Threshold)
        {
            driftMode = 1;
            StartBoost(level1Boost);
            boosted = true;
        }
        else
        {
            driftMode = 0; // No boost
            PlayFlareParticle();
        }

        kartModel.parent.DOLocalRotate(Vector3.zero, .5f).SetEase(Ease.OutBack);

        if (boosted)
        {
            newColor = driftColor[3];
            ChangeParticleColor();
            PlayBoostParticle();
        }

        driftPower = 0;
    }
    public void ReceiveBoost(float boostAmount, float duration)
    {
        driftBoostAmount = boostAmount;
        boostTimer = duration;
        if (virtualCam != null)
        {
            StopAllCoroutines();
            StartCoroutine(ZoomIn());
        }
    }
    public void StartBoost(float boostAmount)
    {
        driftBoostAmount += boostAmount;
        boostTimer = boostDuration;
        if (virtualCam != null)
        {
            StopAllCoroutines();
            StartCoroutine(ZoomIn());
        }
    }
    IEnumerator ZoomIn()
    {
        float startFOV = virtualCam.m_Lens.FieldOfView;
        float targetFOV = boostFOV;
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.fixedDeltaTime * ZoomInSpeed;
            virtualCam.m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsed);
            yield return null;
        }

        virtualCam.m_Lens.FieldOfView = targetFOV; // Ensure it reaches the exact value
        yield return new WaitForSeconds(boostDuration); // Wait for the boost duration

        StartCoroutine(ZoomOut());
    }

    IEnumerator ZoomOut()
    {
        float startFOV = virtualCam.m_Lens.FieldOfView;
        float targetFOV = defaultFOV;
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.fixedDeltaTime * zoomOutSpeed;
            virtualCam.m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsed);
            yield return null;
        }

        virtualCam.m_Lens.FieldOfView = targetFOV;
    }


    private void ApplyDriftAssist()
    {
        float driftAngle = Vector3.Angle(transform.forward, sphere.velocity);
        if (driftAngle > driftAssistAngle)
        {
            float assistForce = Mathf.Clamp(driftAssistStrength * (driftAngle / 90f), 0f, 1f);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + assistForce * steering, 0), Time.fixedDeltaTime * 4f);
        }
    }

    //void MoveForward(InputAction.CallbackContext context)
    //{
    //    moveForward = context.ReadValueAsButton();
    //}

    //void MoveBackWard(InputAction.CallbackContext context)
    //{
    //    moveBackward = context.ReadValueAsButton();
    //}

    //void Drift(InputAction.CallbackContext context)
    //{
    //    drift = context.ReadValueAsButton();
    //}

    private void OnEnable()
    {
        playerKartInput.actions.Enable();
    }

    private void OnDisable()
    {
        playerKartInput.actions.Disable();
    }

    private void AlignKartToGround()
    {
        RaycastHit hitOn, hitNear;

        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitOn, 1.1f, layerMask);
        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitNear, .5f, layerMask);

        kartNormal.up = Vector3.Lerp(kartNormal.up, hitNear.normal, Time.fixedDeltaTime * 8.0f);
        kartNormal.Rotate(0, transform.eulerAngles.y, 0);
    }

    void ChangeParticleColor()
    {
        foreach (var ps in particleSystem)
        {
            if (ps == null) continue;

            var colorOverLifetime = ps.colorOverLifetime;
            colorOverLifetime.enabled = true;

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(newColor, 0.0f), new GradientColorKey(newColor, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
            );
            colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
        }
    }

    public void PlayFlareParticle()
    {
        foreach( ParticleSystem flare in flareParticle)
        {
            flare.Play();
        }
    }

    public void PlayBoostParticle()
    {
        foreach (ParticleSystem boost in boostParticle)
        {
            boost.Play();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + transform.up, transform.position - (transform.up * 2));
    }
}
