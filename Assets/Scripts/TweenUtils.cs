using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TweenUtils
{
    public static void KillAndNull(ref Tween tween)
    {
        if (tween != null)
        {
            tween.Kill();
            tween = null;
        }
    }

    public static void KillAndNull(ref Sequence sequence)
    {
        if (sequence != null)
        {
            sequence.Kill();
            sequence = null;
        }
    }
}
