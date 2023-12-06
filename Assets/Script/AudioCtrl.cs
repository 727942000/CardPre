using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCtrl : MonoBehaviour
{
    public string key;
    private AudioSource au;
    // Start is called before the first frame update
    void Start()
    {
        au = GetComponent<AudioSource>();
        au.volume = PlayerPrefs.GetFloat(key, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        au.volume = PlayerPrefs.GetFloat(key);
    }
}
