using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxScore : MonoBehaviour
{
    public Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = PlayerPrefs.GetFloat("minTime", 1000f).ToString("0.00") + "s";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
