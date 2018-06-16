using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackwardsRocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource spaceshipsound;
    [SerializeField] float rotationThrust = 500f;
    [SerializeField] float upwardThrust = 200f;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        spaceshipsound = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;

            default:
                print("Dead");
                break;
        }
    }

    private void Thrust()
    {
        float upwardThrustThisFrame = upwardThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.W)) //can thrust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up * upwardThrustThisFrame);
            if (!spaceshipsound.isPlaying)
            {
                spaceshipsound.Play();
            }
        }
        else
        {
            spaceshipsound.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;

        float rotationThisFrame = rotationThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 0, 1 * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false;
    }
}

