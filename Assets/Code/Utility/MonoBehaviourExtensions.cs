using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourExtensions
{
  public static void DoAfterTime(this MonoBehaviour obj, float time, Action action)
  {
    obj.StartCoroutine(CoroutineHelpers.DoAfterTimeCoroutine(time, action));
  }
}
