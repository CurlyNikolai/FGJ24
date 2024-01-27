using System;
using System.Collections;
using UnityEngine;

public static class Wait
{

    static IEnumerator ForSeconds(float t, Action callback)
    {
        while (t >= 0)
        {
            t -= Time.deltaTime;
            yield return null;
        }

        callback?.Invoke();
    }

    static IEnumerator ForEndOfFrame(Action callback)
    {
        yield return new WaitForEndOfFrame();
        callback?.Invoke();
    }

}
