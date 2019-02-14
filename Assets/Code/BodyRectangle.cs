using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public enum BodySplitType
{
  Vertical,
  Horizontal,
};

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(OxigenationMeter))]
public class BodyRectangle : MonoBehaviour
{
  private new SpriteRenderer renderer;
  public OxigenationMeter oximeter { get; private set; }
  public SpriteRenderer black;
  public Vector2 position => gameObject.transform.position;
  public float HorizontalSize => renderer.size.x;
  public float VerticalSize => renderer.size.y;
  public bool doesntSplit;
  public float rectangleFadeoutTime;

  public float Area => VerticalSize * HorizontalSize;

  public void Initialize(float horizontalSize, float verticalSize)
  {
    // sets rect size
    Vector2 size = new Vector2(horizontalSize, verticalSize);

    renderer.size = size;
  }

  private void Awake()
  {
    oximeter = GetComponent<OxigenationMeter>();
    renderer = GetComponent<SpriteRenderer>();

    renderer.color += new Color(0, 0, 0, 1);

    var normalColor = black.color;
    normalColor.a = 0;
    black.color = normalColor;

    StartCoroutine(WaitForOxygenToEndThanFadeIn());
  }

  private void Update()
  {
    var lastAlpha = this.renderer.color.a;
    var color = oximeter.GetColor();
    color.a = lastAlpha;
    renderer.color = color;
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
      var newColorTMe = renderer.color;
      var alpha = Mathf.Lerp(0, 1, t / this.rectangleFadeoutTime);
      var alphaMe = Mathf.Lerp(1, 0, t / this.rectangleFadeoutTime);
      newColorT.a = alpha;
      newColorTMe.a = alphaMe;
      black.color = newColorT;
      this.renderer.color = newColorTMe;
      yield return null;
    }

    var lastColorT = black.color;
    var lastColorTMe = renderer.color;
    var lastAlpha = 1;
    var lastAlphaMe = 0;
    lastColorT.a = lastAlpha;
    lastColorTMe.a = lastAlphaMe;
    black.color = lastColorT;
    this.renderer.color = lastColorTMe;
  }
}
