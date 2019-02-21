using System;
using System.Collections;
using UnityEngine;

public static class CoroutineHelpers
{
  public static IEnumerator InterpolateByTime(float time, System.Action<float> interpolator, Action callback = null)
  {
    for (float t = 0f; t < time; t += Time.deltaTime)
    {
      var k = t / time;
      interpolator(k);
      yield return null;
    }
    interpolator(1);
    callback?.Invoke();
  }

  public static IEnumerator DoAfterTimeCoroutine(float time, Action action)
  {
      yield return new WaitForSeconds(time);

      action();
  }
}