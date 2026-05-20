using System.Collections;
using UnityEngine;

public class Timer
{
    public static IEnumerator WaitFor(float seconds, System.Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback?.Invoke();
    }

    public static IEnumerator WaitForUnscaled(float seconds, System.Action callback)
    {
        yield return new WaitForSecondsRealtime(seconds);
        callback?.Invoke();
    }
}