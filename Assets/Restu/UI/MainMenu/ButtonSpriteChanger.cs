using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSpriteChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public Sprite normalSprite;
    public Sprite highlightedSprite;

    private Image buttonImage;
    private Animator animator;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ChangeToHighlighted();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ChangeToNormal();
    }

    public void OnSelect(BaseEventData eventData)
    {
        ChangeToHighlighted();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        ChangeToNormal();
    }

    private void ChangeToHighlighted()
    {
        buttonImage.sprite = highlightedSprite;
        animator.SetTrigger("Highlighted");
    }

    private void ChangeToNormal()
    {
        buttonImage.sprite = normalSprite;
        animator.SetTrigger("Normal");
    }
}
