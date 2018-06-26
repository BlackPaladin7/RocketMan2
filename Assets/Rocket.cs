
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] float rotationThrust = 500f;
    [SerializeField] float upwardThrust = 200f;

    Rigidbody rigidBody;
    AudioSource spaceshipsound;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

	// Use this for initialization
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        spaceshipsound = GetComponent<AudioSource>();
        
	}

    // Update is called once per frame
    void Update ()
    { // to do, stop sound on death

        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly": //do nothing
                print("OK");
                break;

            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f); //Make Designer lever (parameterize)
                break;

            default: //death
                print ("hit something deadly");
                state = State.Dying;
                Invoke ("LoadFirstLevel", 3f); 
                break;
        }
    }

    private void LoadFirstLevel() //do this on player death
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
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
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, 0, 1 * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false;
    }
}
