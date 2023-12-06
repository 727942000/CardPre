using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderCtrl : MonoBehaviour
{
    public string key;
    private Slider slider;
    private float num;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        num = PlayerPrefs.GetFloat(key, 1.0f);
        slider.value = num;
    }

    // Update is called once per frame
    void Update()
    {
        if(slider.value != num)
        {
            num = slider.value;
            PlayerPrefs.SetFloat(key, num);
        }
    }
}
