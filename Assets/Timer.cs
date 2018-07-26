using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Text TimerText;
    private float startTime;

	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(transform.gameObject);
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
    {
        float t = Time.time - startTime;

        string minutes = ((int) t / 60).ToString();
        string seconds = (t % 60).ToString("f1");

        TimerText.text = minutes + ":" + seconds;
	}
}
