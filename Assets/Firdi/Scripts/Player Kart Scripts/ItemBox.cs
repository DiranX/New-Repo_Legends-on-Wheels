using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemBox : MonoBehaviour
{
    public List<GameObject> itemBoxes = new List<GameObject>();

    public float itemTimer;

    private void Start()
    {
        GetAllItemBoxes();
    }

    private void Update()
    {
        StartCoroutine(TimerActive());
    }

    private void GetAllItemBoxes()
    {
        itemBoxes.Clear(); // Clear the list before adding (in case it's called multiple times)

        foreach (Transform child in transform) // Loop through all children
        {
            itemBoxes.Add(child.gameObject);
        }

        Debug.Log("Total Item Boxes: " + itemBoxes.Count);
    }

    public IEnumerator TimerActive()
    {
        yield return new WaitForSecondsRealtime(itemTimer);
        foreach (GameObject itemBox in itemBoxes)
        {
            if (!itemBox.activeSelf)
            {
                itemBox.SetActive(true);
            }
        }
        StopAllCoroutines();
    }

}
