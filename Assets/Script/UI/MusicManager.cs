using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance = null;
    public static MusicManager Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public AudioClip[] music;
    public void PlayMusic(int index)
    {
        GetComponent<AudioSource>().clip = music[index];
        GetComponent<AudioSource>().Play();
    }
}
