using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenImage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;  // Anchors to bottom-left
        rt.anchorMax = Vector2.one;   // Anchors to top-right
        rt.offsetMin = Vector2.zero;  // No offset from edges
        rt.offsetMax = Vector2.zero;  // No offset from edges   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
