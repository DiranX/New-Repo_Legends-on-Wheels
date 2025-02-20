using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItemHolder : MonoBehaviour
{
    public bool haveItem = false;
    public bool ItemUsed;
    public float throwForce;
    public float upwardForce;
    public int playerItemIndex;
    public GameObject[] ItemPrefabs;
    public GameObject[] playerItemUI;
    public Transform itemFront;
    public Animator animator;
    public PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();

        playerInput.actions["Item"].started += ctx => ItemUsed = true;
        playerInput.actions["Item"].canceled += ctx => ItemUsed = false;
    }
    private void Update()
    {
        if (playerItemUI[playerItemIndex].activeSelf == true && ItemUsed)
        {
            UseItem();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ItemLoot"))
        {;
            if (!haveItem) // Only assign a new item if the player doesn't have one
            {
                animator.Play("Scrambling");
            }

            other.gameObject.SetActive(false);
        }
    }

    public void AssignNewItem()
    {
        playerItemIndex = Random.Range(0, playerItemUI.Length);
        playerItemUI[playerItemIndex].SetActive(true);
        haveItem = true; // Mark that the player now has an item
        Debug.Log("Get Item: " + playerItemUI[playerItemIndex].ToString());
    }

    public void UseItem()
    {
        if (haveItem)
        {
            ItemUsed = false;
            playerItemUI[playerItemIndex].SetActive(false); // Deactivate the UI element
            haveItem = false; // Allow picking up a new item
            Debug.Log("Item Used!");

            GameObject Throw = Instantiate(ItemPrefabs[playerItemIndex], itemFront.position, transform.rotation);
            Rigidbody rb = Throw.GetComponent<Rigidbody>();
            Vector3 throwDirection = itemFront.forward * throwForce + Vector3.up * upwardForce;
            rb.AddForce(throwDirection, ForceMode.Impulse);
        }
    }
}
