using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;

    //todo remove from inspector later
    [Range(0, 1)]
    [SerializeField]
    float movementFactor; //0 for not moved, 1 for fully moved

    [SerializeField] Vector3 startingPostion;

	// Use this for initialization
	void Start ()
    {
        startingPostion = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (period <= Mathf.Epsilon)
        {
            return;
        }
        float cycles = Time.time / period; 
        const float tau = Mathf.PI * 2f; //about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);
        

        movementFactor = rawSinWave / 2f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPostion + offset;
	}
}
