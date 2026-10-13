using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float loadDelay;
    [SerializeField] AudioClip levelWinSFX;
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] ParticleSystem levelWinParticles;
    [SerializeField] ParticleSystem explosionParticles;

    AudioSource audioSource;

    bool isControllable = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (!isControllable) { return; }

        switch (other.gameObject.tag)
        {
            case "Neutral":
                Debug.Log("Neutral Object touched. Safe contact!");
                break;

            case "Finish":
                InitiateFinishSequence();
                break;

            default:
                InitiateRestartSequence();
                break;
        }
    }

    void InitiateFinishSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(levelWinSFX, 0.5f);
        levelWinParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", loadDelay);
    }

    void InitiateRestartSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(explosionSFX, 0.5f);
        explosionParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", loadDelay);
    }

    void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;

        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }

        SceneManager.LoadScene(nextScene);
    }

    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
