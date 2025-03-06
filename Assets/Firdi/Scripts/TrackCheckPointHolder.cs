using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrackCheckPointHolder : MonoBehaviour
{
    public static TrackCheckPointHolder instance;

    [Header("Checkpoints & Laps")]
    public GameObject[] CheckPoint; // Array of checkpoints in the track
    public int TrackTotalLap; // Total laps required to complete the race

    [Header("Players")]
    public List<GameObject> playerBatch = new List<GameObject>(); // List of players

    [Header("Leader Board")]
    public TextMeshProUGUI[] leaderBoard;
    public GameObject leaderBoardCanvas; // Assign in Inspector

    private List<GameObject> finishedPlayers = new List<GameObject>(); // Stores finished players

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        playerBatch.AddRange(players);

        // Hide leaderboard UI at the start
        leaderBoardCanvas.SetActive(false);
    }

    private void Update()
    {
        // Sort players based on lap, checkpoint, and distance
        playerBatch.Sort((a, b) =>
        {
            playerLapCounter playerA = a.GetComponent<playerLapCounter>();
            playerLapCounter playerB = b.GetComponent<playerLapCounter>();

            int lapComparison = playerB.currentLap.CompareTo(playerA.currentLap);
            if (lapComparison != 0) return lapComparison;

            int checkpointComparison = playerB.currentCheckpoint.CompareTo(playerA.currentCheckpoint);
            if (checkpointComparison != 0) return checkpointComparison;

            Transform nextCheckpointA = CheckPoint[playerA.currentCheckpoint].transform;
            Transform nextCheckpointB = CheckPoint[playerB.currentCheckpoint].transform;

            float distanceA = Vector3.Distance(a.transform.position, nextCheckpointA.position);
            float distanceB = Vector3.Distance(b.transform.position, nextCheckpointB.position);

            return distanceA.CompareTo(distanceB);
        });

        UpdatePlayerPositions();
        CheckRaceCompletion();
    }

    private void UpdatePlayerPositions()
    {
        for (int i = 0; i < playerBatch.Count; i++)
        {
            GameObject player = playerBatch[i];
            playerLapCounter playerCode = player.GetComponent<playerLapCounter>();

            // Update player's position
            playerCode.playerCurrentPlace = i + 1;
        }
    }

    private void CheckRaceCompletion()
    {
        foreach (GameObject player in playerBatch)
        {
            playerLapCounter playerInfo = player.GetComponent<playerLapCounter>();

            if (playerInfo.finish && !finishedPlayers.Contains(player))
            {
                finishedPlayers.Add(player);
                AddToLeaderBoard(player.name);
            }
        }

        // If all players have finished, show leaderboard
        if (finishedPlayers.Count == playerBatch.Count)
        {
            leaderBoardCanvas.SetActive(true);
        }
    }

    private void AddToLeaderBoard(string playerName)
    {
        for (int i = 0; i < leaderBoard.Length; i++)
        {
            if (string.IsNullOrEmpty(leaderBoard[i].text))
            {
                leaderBoard[i].text = playerName;
                break;
            }
        }
    }
}
