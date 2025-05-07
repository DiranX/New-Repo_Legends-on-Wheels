using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorDeselectOnPlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #if(UNITY_EDITOR)
        Selection.objects = null;
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
