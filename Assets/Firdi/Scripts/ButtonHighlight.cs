using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHighlightChecker : MonoBehaviour
{
    public Button Button;
    public GameObject Image;

    private void Start()
    {
        Button = gameObject.GetComponent<Button>();
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == Button.gameObject)
        {
            Image.SetActive(true);
        }
        else
        {
            Image.SetActive(false);
        }
    }
}
