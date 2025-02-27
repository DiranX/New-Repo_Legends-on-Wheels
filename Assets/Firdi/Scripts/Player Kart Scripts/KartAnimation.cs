using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class KartAnimation : MonoBehaviour
{
    PlayerInput input;
    Animator anim;
    PlayerKartController playerKart;

    public Transform kartModel;
    public Transform leftFrontWheel;  // Separate left front wheel
    public Transform rightFrontWheel; // Separate right front wheel
    public Transform steeringWheel;

    [SerializeField] private float maxSteerAngle; // Max wheel steering angle

    private void Awake()
    {
        input = GetComponentInParent<PlayerInput>();
    }

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        playerKart = GetComponent<PlayerKartController>();
    }

    void Update()
    {
        // Update acceleration and backward animation
        if (playerKart.moveForward)
        {
            anim.SetBool("Accelerate", true);
            anim.SetBool("Backward", false);
        }
        else
        {
            anim.SetBool("Accelerate", false);
        }

        if (playerKart.moveBackward)
        {
            anim.SetBool("Accelerate", false);
            anim.SetBool("Backward", true);
        }
        else
        {
            anim.SetBool("Backward", false);
        }

        // Read input from player
        Vector3 moveInput = input.actions["Move"].ReadValue<Vector2>();

        if(playerKart.drifting && moveInput.x != 0)
        {
            float control = (playerKart.driftDirection == 1) ? 
                ExtensionMethods.Remap(moveInput.x, -1, 1, .5f, 2) : 
                ExtensionMethods.Remap(moveInput.x, -1, 1, 2, .5f);
                
            kartModel.parent.localRotation = Quaternion.Euler(
                0, 
                Mathf.LerpAngle(kartModel.parent.localEulerAngles.y, (control * 15) * playerKart.driftDirection, .2f), 
                0
            );
        }

        // Compute wheel rotation angle based on input
        float steerAngle = moveInput.x * maxSteerAngle;

        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, steerAngle, leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, steerAngle, rightFrontWheel.localRotation.eulerAngles.z);


        // Rotate steering wheel
        steeringWheel.localEulerAngles = new Vector3(0, 0, (moveInput.x * 45));
    }
}
