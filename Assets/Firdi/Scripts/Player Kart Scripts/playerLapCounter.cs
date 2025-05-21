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
    public int playerCurrentPlace;
    public bool finish;
    public AudioSource FinishSound;
    public AudioSource CrowdSound;
    public string[] Place;
    public Color[] textColor;
    public Sprite[] characterFace;

    private void Awake()
    {
        GameObject TrackCheckPoint = GameObject.Find("CheckPoint");

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
                FinishSound.Play();
            }

            if (currentLap >= totalLap)
            {
                //Debug.Log("Win");
                winOrLose.text = playerCurrentPlace.ToString() + Place[playerCurrentPlace];
                finish = true;
                CrowdSound.Play();
            }
        }
    }
}
