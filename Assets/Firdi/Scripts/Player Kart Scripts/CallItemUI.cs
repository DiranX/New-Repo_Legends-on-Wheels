using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallItemUI : MonoBehaviour
{
    public PlayerItemHolder itemHolder;

    public void CallItem()
    {
        itemHolder.AssignNewItem();
    }
}
