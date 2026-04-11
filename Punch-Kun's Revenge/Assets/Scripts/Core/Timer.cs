using System.Collections;
using UnityEngine;

public class Timer
{
    public static IEnumerator WaitFor(float seconds, System.Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback?.Invoke();
    }
}