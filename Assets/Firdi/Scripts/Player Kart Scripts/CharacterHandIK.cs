using UnityEngine;

public class CharacterHandIK : MonoBehaviour
{
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    public Transform steeringWheel;

    private Vector3 initialLeftHandPos;
    private Vector3 initialRightHandPos;

    void Start()
{
    // Save initial hand positions relative to the steering wheel
    initialLeftHandPos = leftHandTarget.localPosition;
    initialRightHandPos = rightHandTarget.localPosition;
    
    // Flip hands if the offset is incorrect
    leftHandTarget.localPosition += new Vector3(0, 0, 0.1f);
    rightHandTarget.localPosition += new Vector3(0, 0, -0.1f);
}


    void Update()
    {
        // Rotate hands along with the steering wheel
        leftHandTarget.position = steeringWheel.TransformPoint(initialLeftHandPos);
        rightHandTarget.position = steeringWheel.TransformPoint(initialRightHandPos);
    }
}
