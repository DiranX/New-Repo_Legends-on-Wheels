using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class playerLapCounter : MonoBehaviour
{
    [Header("Lap and CheckPoint")]
    public int currentCheckpoint;
    public int currentLap;
    public int totalLap;
    public GameObject[] checkPoint;
    public TextMeshProUGUI lapCounter;
    public TextMeshProUGUI PlacementCounter;
    public TextMeshProUGUI PlacementCounter2;
    public TextMeshProUGUI winOrLose;
    public TextMeshProUGUI WrongWay;
    public int playerCurrentPlace;
    public bool finish;
    public AudioSource FinishSound;
    public AudioSource CrowdSound;
    public AudioSource LoseSound;
    public string[] Place;
    public Color[] textColor;
    public Sprite[] characterFace;
    Rigidbody rb;
    float movementThreshold = 25f;
    private bool isMoving = false;
    public float wrongWayTimer = 0f;
    float delayTimer = 1.5f;

    private void Awake()
    {
        GameObject TrackCheckPoint = GameObject.Find("CheckPoint");

        rb = GetComponent<Rigidbody>();

        if(TrackCheckPoint != null)
        {
            checkPoint = TrackCheckPoint.GetComponent<TrackCheckPointHolder>().CheckPoint;
            totalLap = TrackCheckPoint.GetComponent<TrackCheckPointHolder>().TrackTotalLap;
        }
    }
    private void Start()
    {
        lapCounter.text = (currentLap + 1).ToString();
    }
    private void Update()
    {
        if (rb.velocity.magnitude >= movementThreshold)
        {
            isMoving = true;
        }

        if (isMoving)
        {
            Vector3 toNextWaypoint = (checkPoint[currentCheckpoint].transform.position - transform.position).normalized;
            Vector3 playerForward = rb.velocity.normalized;
            float dot = Vector3.Dot(toNextWaypoint, playerForward);
            if (dot < -0.10f && rb.velocity.magnitude >= movementThreshold)
            {
                wrongWayTimer += Time.deltaTime;
                if(wrongWayTimer > delayTimer)
                {
                    WrongWay.text = "Wrong Way!";
                }
            }
            else
            {
                WrongWay.text = "";
                wrongWayTimer = 0f;
            }
        }
        else
        {
            wrongWayTimer = 0f;
            WrongWay.text = "";
        }

        PlacementCounter.text = playerCurrentPlace.ToString();
        PlacementCounter2.text = Place[playerCurrentPlace];
        PlacementCounter.color = textColor[playerCurrentPlace];
        PlacementCounter2.color = textColor[playerCurrentPlace];
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == checkPoint[currentCheckpoint])
        {
            this.currentCheckpoint++;
            if (currentCheckpoint == checkPoint.Length)
            {
                this.currentCheckpoint = 0;
                this.currentLap++;
                this.lapCounter.text = (currentLap + 1).ToString();
                StartCoroutine(NullText());
                FinishSound.Play();
                StopCoroutine(NullText());
            }

            if (currentLap >= totalLap && playerCurrentPlace == 1)
            {
                //Debug.Log("Win");
                winOrLose.text = playerCurrentPlace.ToString() + Place[playerCurrentPlace];
                finish = true;
                CrowdSound.PlayOneShot(CrowdSound.clip);
                //StartCoroutine(Stop());
            }else if(currentLap >= totalLap && playerCurrentPlace != 1)
            {
                //Debug.Log("Win");
                winOrLose.text = playerCurrentPlace.ToString() + Place[playerCurrentPlace];
                finish = true;
                LoseSound.PlayOneShot(LoseSound.clip);
                //StartCoroutine(Stop());
            }
        }
    }
    IEnumerator NullText()
    {
        if(currentLap == 1)
        {
            this.winOrLose.text = (currentLap + 1).ToString() + Place[currentLap + 1].ToString() + " Lap";
        }else if(currentLap == 2)
        {
            this.winOrLose.text = "Final Lap!";
        }
        yield return new WaitForSeconds(1);
        this.winOrLose.text = "";
    }
    IEnumerator Stop()
    {
        yield return new WaitForSeconds(1);
        GetComponent<PlayerItemHolder>().playerKartController.canMove = false;
        GetComponent<PlayerItemHolder>().playerKartController.sphere.GetComponent<Rigidbody>().isKinematic = true;
    }
}
