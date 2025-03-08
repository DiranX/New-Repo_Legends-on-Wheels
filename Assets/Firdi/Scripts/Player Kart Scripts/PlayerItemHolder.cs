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
    public Transform itemBack;
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
        Vector2 throwInput = playerInput.actions["Move"].ReadValue<Vector2>();
        if (playerItemUI[playerItemIndex].activeSelf == true && ItemUsed)
        {
            if(throwInput.y >= 0f)
            {
                FrontThrow();
            }else if(throwInput.y < 0f)
            {
                BackThrow();
            }
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

    public void FrontThrow()
    {
        if (haveItem)
        {
            ItemUsed = false;
            playerItemUI[playerItemIndex].SetActive(false); // Deactivate the UI element
            haveItem = false; // Allow picking up a new item
            Debug.Log("Item Used!");

            // Instantiate the item
            GameObject Throw = Instantiate(ItemPrefabs[playerItemIndex], itemFront.position, itemFront.rotation);
            Rigidbody rb = Throw.GetComponent<Rigidbody>();

            // Get player velocity
            Vector3 playerVelocity = GetComponent<Rigidbody>().velocity;

            // Calculate force multiplier based on speed
            float speedFactor = Mathf.Clamp(playerVelocity.magnitude / 10f, 0.5f, 2f); // Adjust range as needed

            // Apply dynamic force
            Vector3 throwDirection = itemFront.forward * (throwForce * speedFactor) + Vector3.up * (upwardForce);
            rb.AddForce(throwDirection, ForceMode.Impulse);
        }
    }
    public void BackThrow()
    {
        if (haveItem)
        {
            ItemUsed = false;
            playerItemUI[playerItemIndex].SetActive(false); // Deactivate the UI element
            haveItem = false; // Allow picking up a new item
            Debug.Log("Item Used!");

            // Instantiate the item
            GameObject Throw = Instantiate(ItemPrefabs[playerItemIndex], itemBack.position, itemBack.rotation);
            Rigidbody rb = Throw.GetComponent<Rigidbody>();

            // Get player velocity
            Vector3 playerVelocity = GetComponent<Rigidbody>().velocity;

            // Apply dynamic force
            Vector3 throwDirection = -itemBack.forward * (throwForce/2) + Vector3.up * (upwardForce);
            rb.AddForce(throwDirection, ForceMode.Impulse);
        }
    }

}
