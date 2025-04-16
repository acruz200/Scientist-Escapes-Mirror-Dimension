using System.Collections;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    [Header("Background Music")]
    public AudioClip backgroundMusic;
    private AudioSource audioSource;
    private const float BGM_DURATION = 22.5f;
    private bool hasPlayedInitial = false;

    void Start()
    {
        // Add AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.volume = 0.08f;

        // Start playing background music
        if (backgroundMusic != null)
        {
            StartCoroutine(PlayBackgroundMusic());
        }
    }

    IEnumerator PlayBackgroundMusic()
    {
        // Play the initial 22.5 seconds
        audioSource.clip = backgroundMusic;
        audioSource.Play();
        yield return new WaitForSeconds(BGM_DURATION);
        
        // After initial playthrough, set up looping
        hasPlayedInitial = true;
        audioSource.loop = true;
        audioSource.time = 0f; // Reset to start
        audioSource.Play();
    }
} 