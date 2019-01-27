using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public enum BodySplitType
{
  Vertical,
  Horizontal,
  Cross,
};

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(OxigenationMeter))]
public class BodyRectangle : MonoBehaviour
{
  private new SpriteRenderer renderer;
  private OxigenationMeter oximeter;
  public SpriteRenderer black;
  public Vector2 position => gameObject.transform.position;
  public float HorizontalSize => renderer.size.x;
  public float VerticalSize => renderer.size.y;
  public bool doesntSplit;
  public float rectangleFadeoutTime;

  public void Initialize(float horizontalSize, float verticalSize)
  {
    // sets rect size
    Vector2 size = new Vector2(horizontalSize, verticalSize);

    renderer.size = size;
  }

  private void Awake()
  {
    renderer = GetComponent<SpriteRenderer>();
    oximeter = GetComponent<OxigenationMeter>();

    renderer.color += new Color(0,0,0,1);

    var normalColor = black.color;
    normalColor.a = 0;
    black.color = normalColor;

    StartCoroutine(WaitForOxygenToEndThanFadeIn());
  }

  private void Update()
  {
    renderer.color = oximeter.GetColor();
  }

  IEnumerator WaitForOxygenToEndThanFadeIn()
  {
      while (oximeter.IsAlive)
      {
          yield return null;
      }
    
      GameEvents.RectangleGotGangrenous(this);
      StartCoroutine(FadeOut());
  }

  private IEnumerator FadeOut()
  {
    print("Fading out started");
    this.black.GetComponent<Collider2D>().enabled = true;
    for (float t = 0; t < this.rectangleFadeoutTime; t += Time.deltaTime)
    {
        var newColorT = black.color;
        var alpha = Mathf.Lerp(0, 1, t / this.rectangleFadeoutTime);
        newColorT.a = alpha;
        black.color = newColorT;
        yield return null;
    }
  }
}
