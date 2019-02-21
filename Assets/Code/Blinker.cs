using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : MonoBehaviour
{
  public FloatValue MaxBlinkDuration;
  public FloatValue MinBlinkDuration;
  public FloatValue MaxBlinkInterval;
  public FloatValue MinBlinkInterval;
  public BoolValue IsBlinking;
  Animator anim;
  // Start is called before the first frame update
  void Start()
  {
    anim = GetComponent<Animator>();
    StartCoroutine(BlinkRandomly());
  }


  IEnumerator BlinkRandomly()
  {
    while (IsBlinking.Value)
    {
      var blinkInterval = Random.Range(MinBlinkInterval.Value, MaxBlinkInterval.Value);
      yield return new WaitForSeconds(blinkInterval);

      anim.SetTrigger("blink");

      var blinkTime = Random.Range(MinBlinkDuration.Value, MaxBlinkDuration.Value);
      yield return new WaitForSeconds(blinkTime);

      anim.SetTrigger("blink");
    }
    yield return null;
  }
}

