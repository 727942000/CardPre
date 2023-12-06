using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCtrl : MonoBehaviour
{
    RectTransform rt;
    public float s1, s2;
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rt.rect.Set(rt.rect.x, rt.rect.y, s1, s2);
    }
}
