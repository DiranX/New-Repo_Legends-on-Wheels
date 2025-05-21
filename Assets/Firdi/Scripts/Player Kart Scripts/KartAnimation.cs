using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
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
    public bool ETV;

    [SerializeField] private float maxSteerAngle; // Max wheel steering angle

    public Animator charaAnim;
    public TwoBoneIKConstraint LhandIk;
    public TwoBoneIKConstraint RhandIk;
    public bool itemUsed;
    public bool skilUSed;
    public GameObject[] objek;
    public bool isTimun;
    public bool isJongrang;
    public bool isKeong;
    public bool isMalin;
    public bool isKidul;
    public bool isSangkuriang;
    public bool isPitung;
    public bool isButo;
    public bool isKancil;
    public bool isLutung;

    private void Awake()
    {
        input = GetComponentInParent<PlayerInput>();
        input.actions["Item"].started += ctx => itemUsed = true;
        input.actions["Item"].canceled += ctx => itemUsed = false;
        input.actions["Skill"].started += ctx => skilUSed = true;
        input.actions["Skill"].canceled += ctx => skilUSed = false;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
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

        if (playerKart.drifting && moveInput.x != 0)
        {
            float control = (playerKart.driftDirection == 1) ?
                ExtensionMethods.Remap(moveInput.x, -1, 1, .5f, 2) :
                ExtensionMethods.Remap(moveInput.x, -1, 1, 2, .5f);

            kartModel.parent.localRotation = Quaternion.Euler(
                0,
                Mathf.LerpAngle(kartModel.parent.localEulerAngles.y, (control * 15) * playerKart.driftDirection, .2f),
                0
            );

            float headTilt = (playerKart.driftDirection == 1) ? ExtensionMethods.Remap(moveInput.x, -1, 1, -2, 2) : ExtensionMethods.Remap(moveInput.x, -1, 1, 2, -2);

            if (playerKart.driftDirection == 1)
            {
                if (moveInput.x > 0)
                {
                    charaAnim.SetFloat("L", 1);
                }
            } else if (playerKart.driftDirection == -1)
            {
                if (control > 0)
                {
                    charaAnim.SetFloat("R", 1);
                }
            }
        }

        // Compute wheel rotation angle based on input
        float steerAngle = moveInput.x * maxSteerAngle;

        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, steerAngle, leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, steerAngle, rightFrontWheel.localRotation.eulerAngles.z);

        if (ETV)
        {
            // Rotate steering wheel
            steeringWheel.localEulerAngles = new Vector3(90, (moveInput.x * 45), 0);
        }
        else
        {
            // Rotate steering wheel
            steeringWheel.localEulerAngles = new Vector3(0, 0, (moveInput.x * 45));
        }

        if (moveInput.x == 0)
        {
            charaAnim.SetFloat("L", 0);
            charaAnim.SetFloat("R", 0);
        }

        SkillandItemAnimation();
    }

    void SkillandItemAnimation()
    {
        bool haveItem = playerKart.sphere.GetComponent<PlayerItemHolder>().haveItem;
        int index = playerKart.sphere.GetComponent<PlayerItemHolder>().playerItemIndex;
        int layerInd = charaAnim.GetLayerIndex("Tangan");
        Vector2 moveInput = input.actions["Move"].ReadValue<Vector2>();

        if (haveItem)
        {
            RhandIk.weight = 0;
            if(index == 0)
            {
                charaAnim.SetBool("isHolding", true);
                charaAnim.SetLayerWeight(layerInd, 1);
                objek[0].SetActive(true);
            }
            if(index == 2)
            {
                charaAnim.SetBool("isHolding", true);
                charaAnim.SetLayerWeight(layerInd, 1);
                objek[1].SetActive(true);
            }
            if(index == 1)
            {
                charaAnim.SetLayerWeight(layerInd, 1);
            }
            if(index == 3)
            {
                charaAnim.SetLayerWeight(layerInd, 1);
            }
        }else if(!haveItem)
        {
            //charaAnim.SetLayerWeight(layerInd, 0);
            objek[0].SetActive(false);
            objek[1].SetActive(false);
            RhandIk.weight = 1;
        }

        if (itemUsed)
        {
            //handIk.weight = 1;
            if (index == 0 || index == 2)
            {
                charaAnim.SetBool("isHolding", false);
                if (moveInput.y >= 0)
                {
                    charaAnim.SetTrigger("front");
                }
                else if (moveInput.y < 0)
                {
                    charaAnim.SetTrigger("back");
                }
            }

            if(index == 1)
            {
                charaAnim.SetTrigger("Hyper");
            }

            if(index == 3)
            {
                charaAnim.SetTrigger("Eclipse");
            }
        }

        if (skilUSed)
        {
            charaAnim.SetLayerWeight(layerInd, 1);
            LhandIk.weight = 0;
            RhandIk.weight = 0;
            if (isTimun)
            {
                charaAnim.SetTrigger("front");
                if (moveInput.y >= 0)
                {
                    charaAnim.SetTrigger("front");
                }
                if (moveInput.y < 0)
                {
                    charaAnim.SetTrigger("back");
                }
            }
            if (isJongrang)
            {
                charaAnim.SetTrigger("Ult");
            }
            if (isKeong)
            {
                charaAnim.SetTrigger("Ult");
            }
            if (isMalin)
            {
                charaAnim.SetTrigger("Ult");
            }
            if (isKidul)
            {
                charaAnim.SetTrigger("Ult");
            }
            if (isSangkuriang)
            {
                charaAnim.SetTrigger("Ult");
            }
            if (isPitung)
            {
                charaAnim.SetTrigger("front");
                if (moveInput.y >= 0)
                {
                    charaAnim.SetTrigger("front");
                }
                else if (moveInput.y < 0)
                {
                    charaAnim.SetTrigger("back");
                }
            }
            if (isButo)
            {
                charaAnim.SetTrigger("Ult");
            }
            if (isKancil)
            {
                charaAnim.SetTrigger("Ult");
            }
            if (isLutung)
            {
                charaAnim.SetTrigger("front");
                if (moveInput.y >= 0)
                {
                    charaAnim.SetTrigger("front");
                }
                else if (moveInput.y < 0)
                {
                    charaAnim.SetTrigger("back");
                }
            }
        }
    }
}
