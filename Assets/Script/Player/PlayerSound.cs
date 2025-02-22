using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip grabSound;
    public AudioClip releaseSound;
    public AudioClip collisionSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayGrabSound()
    {
        audioSource.PlayOneShot(grabSound);
    }

    public void PlayReleaseSound()
    {
        audioSource.PlayOneShot(releaseSound);
    }

    public void PlayCollisionSound(float intensity)
    {
        audioSource.PlayOneShot(collisionSound, intensity);
    }
}
