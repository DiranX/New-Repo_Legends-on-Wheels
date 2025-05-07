using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    // Throw Boomerang-like weapon in 3D
    // By René Pol 19-11-2022

    public Vector3 StartPos;              // Start position of the weapon
    public float Distance;                // Distance to throw the weapon
    public bool Thrown;                   // Is weapon currently thrown
    public float ThrowSpeed;              // Speed of weapon movement
    public Transform Axe;                 // Weapon model (child transform)
    public float rotateSpeed;             // Rotation speed
    public bool RotateOnOff;              // Is rotation active
    public bool StartRotationPosition;    // Has the weapon returned to its original rotation?

    void Start()
    {
        RotateOnOff = false;
        Thrown = false;
        StartPos = transform.position;
        StartRotationPosition = false;
    }

    void Update()
    {
        // Handle weapon rotation
        if (RotateOnOff)
        {
            Axe.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }
        else if (!StartRotationPosition)
        {
            Axe.rotation = Quaternion.Euler(0, 90f, 0);
            StartRotationPosition = true;
        }

        // Move weapon forward when thrown
        if (Thrown && transform.position.z < StartPos.z + Distance)
        {
            transform.Translate(Vector3.forward * ThrowSpeed * Time.deltaTime);

            if (transform.position.z >= StartPos.z + Distance)
            {
                Axe.rotation = Quaternion.Euler(0, -90f, 0);
                Thrown = false;
            }
        }

        // Return to start position if not thrown
        if (!Thrown && Vector3.Distance(transform.position, StartPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, StartPos, ThrowSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, StartPos) <= 0.01f)
            {
                transform.position = StartPos; // snap to exact
                Thrown = false;
                RotateOnOff = false;
            }
        }

        // Input to throw weapon
        if (Input.GetKeyDown(KeyCode.Space) && !Thrown && Vector3.Distance(transform.position, StartPos) < 0.01f)
        {
            StartRotationPosition = false;
            RotateOnOff = true;
            Thrown = true;
        }
    }
}
