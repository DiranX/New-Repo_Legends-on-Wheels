using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHolder : MonoBehaviour
{
    public GameObject[] secondP;
    public GameObject thirdP;
    public GameObject fourthP;
    private void Awake()
    {
        if (GameManager.Instance.playerCount == 2)
        {
            secondP[0].SetActive(true);
            secondP[1].SetActive(true);
        }
        else if(GameManager.Instance.playerCount == 3)
        {
            secondP[0].SetActive(true);
            secondP[1].SetActive(true);
            thirdP.SetActive(true);
        }
        else if(GameManager.Instance.playerCount == 4)
        {
            secondP[0].SetActive(true);
            secondP[1].SetActive(true);
            thirdP.SetActive(true);
            fourthP.SetActive(true);
        }
    }
}
