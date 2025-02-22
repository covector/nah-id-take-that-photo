using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    public void Back()
    {
        SceneManager.LoadScene("Menu");
        MusicManager.Instance.PlayMusic(0);
    }
}
