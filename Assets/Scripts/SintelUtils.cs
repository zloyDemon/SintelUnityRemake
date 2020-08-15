using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SintelUtils
{
    public static void KillAndNullCoroutine(MonoBehaviour monoBehaviour, ref Coroutine coroutine)
    {
        if (coroutine != null)
        {
            monoBehaviour.StopCoroutine(coroutine);
            coroutine = null;
        }
    }
}
