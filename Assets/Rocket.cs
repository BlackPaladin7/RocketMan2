using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource fartSound;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        fartSound = GetComponent<AudioSource>();
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        Thrust();
        Rotate();
	}

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.W)) //can thrust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up);
            if (!fartSound.isPlaying)
            {
                fartSound.Play();
            }
        }
        else
        {
            fartSound.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, 0, 1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward);
        }
        rigidBody.freezeRotation = false;
    }
}
