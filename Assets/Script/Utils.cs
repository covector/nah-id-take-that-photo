using System;
using System.Collections;
using UnityEngine;

public class Utils
{
    public static Vector2 ToVector2(Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }

    public static Vector3 ToVector3(Vector2 vector, float y = 0)
    {
        return new Vector3(vector.x, y, vector.y);
    }

    public static void RunDelay(MonoBehaviour mb, Action action, float delay, bool unscaledTime = false)
    {
        mb.StartCoroutine(_RunDelay(action, delay, unscaledTime));
    }
    public static IEnumerator _RunDelay(Action action, float delay, bool unscaledTime = false)
    {
        yield return unscaledTime ? new WaitForSecondsRealtime(delay) : new WaitForSeconds(delay);
        action();
    }
}
