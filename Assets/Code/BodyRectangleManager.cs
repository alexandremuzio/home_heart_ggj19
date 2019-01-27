using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;
using System.Collections;

public class BodyRectangleManager : MonoBehaviour
{

  private float timeSinceLastSplit;

  private float splitCooldown;

  public float splitCooldownBase;

  public float splittingMultiplier;

  public float rectangleFadeoutTime;

  public BodyRectangle bodyRectPrefab;

  public List<BodyRectangle> rectangles = new List<BodyRectangle>();

  private void Start()
  {
    // Create initial rectangles for body
    foreach (var rectangle in FindObjectsOfType<BodyRectangle>())
    {
      if (rectangle.doesntSplit) continue;
      rectangles.Add(rectangle);
    }

    splitCooldown = splitCooldownBase;

    GameEvents.RectangleGotGangrenous += OnRectangleDied;
  }

  private void Update()
  {
    timeSinceLastSplit += Time.deltaTime;

    if (timeSinceLastSplit > splitCooldown)
    {
      // Split event
      SplitRandomRectangle();
      splitCooldown = splitCooldownBase + Random.RandomRange(-1f, 1f);
      timeSinceLastSplit = 0f;
    }

    if (Input.GetKeyDown(KeyCode.Space))
    {
      SplitRandomRectangle();
    }
  }

  private void SplitRandomRectangle()
  {
    BodyRectangle r = rectangles
                .OrderBy(_ => Guid.NewGuid())
                .OrderByDescending((x1) => Mathf.Min(x1.VerticalSize, x1.HorizontalSize))
                .First();

    // choose random point inside rectangle
    float randXMax = r.HorizontalSize / 2f + r.position.x;
    float randXMin = -r.HorizontalSize / 2f + r.position.x;
    float randYMax = r.VerticalSize / 2f + r.position.y;
    float randYMin = -r.VerticalSize / 2f + r.position.y;

    float x = Random.Range(randXMin + r.HorizontalSize / 7, randXMax - r.HorizontalSize / 7);
    float y = Random.Range(randYMin + r.VerticalSize / 7, randYMax - r.VerticalSize / 7);

    Array values = Enum.GetValues(typeof(BodySplitType));
    System.Random random = new System.Random();
    BodySplitType randomSplit = (BodySplitType)values.GetValue(random.Next(values.Length - 1));

    float aspectRatio = r.HorizontalSize / r.VerticalSize;

    if (aspectRatio <= 1)
        SplitRectangle(r, new Vector2(x, y), BodySplitType.Horizontal);
    else
        SplitRectangle(r, new Vector2(x, y), BodySplitType.Vertical);
  }

  public void SplitRectangle(BodyRectangle r, Vector2 pos, BodySplitType splitType)
  {
      // O2 Info for split rectangle
      var O2 = r.GetComponent<OxigenationMeter>();
      var O2lvl = O2.oxygenLevel;
      var O2MaxLvl = O2.maxOxygenLevel;

    switch (splitType)
    {
      case BodySplitType.Vertical:
        Vector2 newPos = new Vector2((r.position.x - r.HorizontalSize / 2 + pos.x) / 2, r.position.y);
        BodyRectangle Ob1 = Instantiate(this.bodyRectPrefab, newPos, Quaternion.identity, r.transform.parent);
        Ob1.Initialize(Mathf.Abs(pos.x - (r.position.x - r.HorizontalSize / 2)), r.VerticalSize);
        Ob1.transform.position = newPos;

        float proportionRatio = Mathf.Abs(pos.x - (r.position.x - r.HorizontalSize / 2)) / r.HorizontalSize;
        var newO2 = Ob1.GetComponent<OxigenationMeter>();
        newO2.SetOxygenData(splittingMultiplier * proportionRatio * O2lvl, splittingMultiplier * proportionRatio * O2MaxLvl);
        StartCoroutine(FadeIn(Ob1));
        rectangles.Add(Ob1);

        newPos = new Vector2((r.position.x + r.HorizontalSize / 2 + pos.x) / 2, r.position.y);
        Ob1 = Instantiate(this.bodyRectPrefab, newPos, Quaternion.identity, r.transform.parent);
        Ob1.Initialize(Mathf.Abs(pos.x - (r.position.x + r.HorizontalSize / 2)), r.VerticalSize);
        Ob1.transform.position = newPos;

        proportionRatio = 1 - proportionRatio;
        newO2 = Ob1.GetComponent<OxigenationMeter>();
        newO2.SetOxygenData(splittingMultiplier * proportionRatio * O2lvl, splittingMultiplier * proportionRatio * O2MaxLvl);
        StartCoroutine(FadeIn(Ob1));
        rectangles.Add(Ob1);
        break;
      case BodySplitType.Horizontal:
        Vector2 newPosV = new Vector2(r.position.x, (r.position.y - r.VerticalSize / 2 + pos.y) / 2);
        BodyRectangle Ob1V = Instantiate(this.bodyRectPrefab, newPosV, Quaternion.identity, r.transform.parent);
        Ob1V.Initialize(r.HorizontalSize, Mathf.Abs(pos.y - (r.position.y - r.VerticalSize / 2)));
        Ob1V.transform.position = newPosV;

        float proportionRatioV = Mathf.Abs(pos.y - (r.position.y - r.VerticalSize / 2)) / r.VerticalSize;
        var newO2V = Ob1V.GetComponent<OxigenationMeter>();
        newO2V.SetOxygenData(splittingMultiplier * proportionRatioV * O2lvl, splittingMultiplier * proportionRatioV * O2MaxLvl);
        StartCoroutine(FadeIn(Ob1V));
        rectangles.Add(Ob1V);

        newPosV = new Vector2(r.position.x, (r.position.y + r.VerticalSize / 2 + pos.y) / 2);
        Ob1V = Instantiate(this.bodyRectPrefab, newPosV, Quaternion.identity, r.transform.parent);
        Ob1V.Initialize(r.HorizontalSize, Mathf.Abs(pos.y - (r.position.y + r.VerticalSize / 2)));
        Ob1V.transform.position = newPosV;

        proportionRatioV = 1 - proportionRatioV;
        newO2V = Ob1V.GetComponent<OxigenationMeter>();
        newO2V.SetOxygenData(splittingMultiplier * proportionRatioV * O2lvl, splittingMultiplier * proportionRatioV * O2MaxLvl);
        StartCoroutine(FadeIn(Ob1V));
        rectangles.Add(Ob1V);
        break;
      case BodySplitType.Cross:
        Vector2 newPosC = new Vector2((r.position.x - r.HorizontalSize / 2 + pos.x) / 2, (r.position.y - r.VerticalSize / 2 + pos.y) / 2);
        BodyRectangle Ob1C = Instantiate(this.bodyRectPrefab, newPosC, Quaternion.identity, r.transform.parent);
        Ob1C.Initialize(Mathf.Abs(pos.x - (r.position.x - r.HorizontalSize / 2)), Mathf.Abs(pos.y - (r.position.y - r.VerticalSize / 2)));
        Ob1C.transform.position = newPosC;
        rectangles.Add(Ob1C);

        newPosC = new Vector2((r.position.x + r.HorizontalSize / 2 + pos.x) / 2, (r.position.y - r.VerticalSize / 2 + pos.y) / 2);
        Ob1C = Instantiate(this.bodyRectPrefab, newPosC, Quaternion.identity, r.transform.parent);
        Ob1C.Initialize(Mathf.Abs(pos.x - (r.position.x + r.HorizontalSize / 2)), Mathf.Abs(pos.y - (r.position.y - r.VerticalSize / 2)));
        Ob1C.transform.position = newPosC;
        rectangles.Add(Ob1C);

        newPosC = new Vector2((r.position.x - r.HorizontalSize / 2 + pos.x) / 2, (r.position.y + r.VerticalSize / 2 + pos.y) / 2);
        Ob1C = Instantiate(this.bodyRectPrefab, newPosC, Quaternion.identity, r.transform.parent);
        Ob1C.Initialize(Mathf.Abs(pos.x - (r.position.x - r.HorizontalSize / 2)), Mathf.Abs(pos.y - (r.position.y + r.VerticalSize / 2)));
        Ob1C.transform.position = newPosC;
        rectangles.Add(Ob1C);

        newPosC = new Vector2((r.position.x + r.HorizontalSize / 2 + pos.x) / 2, (r.position.y + r.VerticalSize / 2 + pos.y) / 2);
        Ob1C = Instantiate(this.bodyRectPrefab, newPosC, Quaternion.identity, r.transform.parent);
        Ob1C.Initialize(Mathf.Abs(pos.x - (r.position.x + r.HorizontalSize / 2)), Mathf.Abs(pos.y - (r.position.y + r.VerticalSize / 2)));
        Ob1C.transform.position = newPosC;
        rectangles.Add(Ob1C);
        break;
    }

    // delete rectangle
    rectangles.Remove(r);
    StartCoroutine(FadeOut(r));
  }

  void OnRectangleDied(BodyRectangle r)
  {
      print ("Deleting from rectangle list");
      rectangles.Remove(r);
  }

  private IEnumerator FadeOut(BodyRectangle r)
  {
    var renderer = r.GetComponent<SpriteRenderer>();
    for (float t = 0; t < this.rectangleFadeoutTime; t += Time.deltaTime)
    {
        var newColorT = renderer.color;
        var alpha = Mathf.Lerp(1, 0, t / this.rectangleFadeoutTime);
        newColorT.a = alpha;
        renderer.color = newColorT;
        yield return null;
    }

    Destroy(r.gameObject);
  }


  private IEnumerator FadeIn(BodyRectangle r)
  {
    var renderer = r.GetComponent<SpriteRenderer>();
    for (float t = 0; t < this.rectangleFadeoutTime; t += Time.deltaTime)
    {
        var newColorT = renderer.color;
        var alpha = Mathf.Lerp(0, 1, t / this.rectangleFadeoutTime);
        newColorT.a = alpha;
        renderer.color = newColorT;
        yield return null;
    }
  }
}