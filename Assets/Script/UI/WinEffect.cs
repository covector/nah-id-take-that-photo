using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.Collections;

[DisallowMultipleComponent]
public class WinEffect : MonoBehaviour
{
    public Animator animator;
    public int winSceneIndex;
    public Grabbing grabbingLogic;
    bool won = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayWin();
        }
    }

    public void PlayWin()
    {
        if (won) return;
        won = true;
        StartCoroutine(WinSequence());
        animator.ResetTrigger("Win");
        animator.SetTrigger("Win");
        GetComponent<AudioSource>().Play();
    }

    IEnumerator WinSequence()
    {
        grabbingLogic.enabled = false;
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        SceneManager.LoadScene(winSceneIndex);
        MusicManager.Instance.PlayMusic(1);
    }
}
