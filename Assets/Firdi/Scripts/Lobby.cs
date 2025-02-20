using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(PlayerInputManager))]
public class Lobby : MonoBehaviour
{
    public List<CharacterSelector> Character = new List<CharacterSelector>();
    private void Awake()
    {
        GameObject[] selectorObjects = GameObject.FindGameObjectsWithTag("Selector");

        foreach (GameObject obj in selectorObjects)
        {
            CharacterSelector characterSelector = obj.GetComponent<CharacterSelector>();

            if (characterSelector != null)
            {
                Character.Add(characterSelector);
            }
        }

        if(GameManager.Instance != null)
        {
            GameManager.Instance.characterSelector = Character;
        }
    }
}
