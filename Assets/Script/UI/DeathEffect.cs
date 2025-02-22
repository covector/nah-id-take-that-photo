using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class DeathEffect : MonoBehaviour
{
    public Volume volume;
    ColorAdjustments colorAdjustments;
    Animator animator;

    private void Start()
    {
        volume.profile.TryGet(out colorAdjustments);
        animator = GetComponent<Animator>();
    }

    public void PlayDeath()
    {
        StartCoroutine(DeathSequence());
        animator.ResetTrigger("Death");
        animator.SetTrigger("Death");
    }

    IEnumerator DeathSequence()
    {
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        for (float t = 0; t < 1f; t += Time.unscaledDeltaTime)
        {
            colorAdjustments.saturation.value = Mathf.Lerp(0, -65f, t);
            colorAdjustments.contrast.value = Mathf.Lerp(0, 60f, t);
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }

        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
