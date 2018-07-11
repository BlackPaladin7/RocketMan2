
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackwardsRocket : MonoBehaviour
{

    [SerializeField] float rotationThrust = 500f;
    [SerializeField] float upwardThrust = 200f;
    [SerializeField] float levelLoadDelay = 3f;

    [SerializeField] AudioClip MainEngine;
    [SerializeField] AudioClip CollisionExplosion;
    [SerializeField] AudioClip WinLevel;

    [SerializeField] ParticleSystem rocketEngineParticles;
    [SerializeField] ParticleSystem collisionParticles;
    [SerializeField] ParticleSystem winParticles;

    Rigidbody rigidBody;
    AudioSource spaceshipsound;
    bool collisionsAreDisabled = false;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        spaceshipsound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    { // to do, stop sound on death

        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionsAreDisabled)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly": //do nothing
                break;

            case "Finish":
                RunWinSequence();
                break;

            default: //death
                RunDeathSequence();
                break;
        }
    }

    private void RunWinSequence()
    {
        state = State.Transcending;
        spaceshipsound.Stop();
        rocketEngineParticles.Stop();
        spaceshipsound.PlayOneShot(WinLevel);
        winParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void RunDeathSequence()
    {
        print("hit something deadly");
        state = State.Dying;
        spaceshipsound.Stop();
        rocketEngineParticles.Stop();
        spaceshipsound.PlayOneShot(CollisionExplosion);
        collisionParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void LoadFirstLevel() //do this on player death
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + (1);
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        if (currentSceneIndex + 1 < totalScenes)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else if (currentSceneIndex + 1 == totalScenes)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void Thrust()
    {
        float upwardThrustThisFrame = upwardThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.W)) //can thrust while rotating
        {
            ApplyThrust(upwardThrustThisFrame);
        }
        else
        {
            spaceshipsound.Stop();
            rocketEngineParticles.Stop();
        }
    }

    private void ApplyThrust(float upwardThrustThisFrame)
    {
        rigidBody.AddRelativeForce(Vector3.up * upwardThrustThisFrame);
        if (!spaceshipsound.isPlaying)
        {
            spaceshipsound.PlayOneShot(MainEngine);
        }
        rocketEngineParticles.Play();
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

    private void RespondToDebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsAreDisabled = !collisionsAreDisabled;
        }
    }
}
