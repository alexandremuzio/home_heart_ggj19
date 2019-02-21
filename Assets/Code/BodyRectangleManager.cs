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

  public float rectangleFadeoutTime;

  public FloatValue InitialOxygenPerArea;
  public FloatValue OxygenPerArea;

  public FloatValue OxygenPerAreaIncrementerSplit;

  public BodyRectangle bodyRectPrefab;

  public List<BodyRectangle> rectangles = new List<BodyRectangle>();

  private void Start()
  {
    OxygenPerArea.Value = InitialOxygenPerArea.Value;

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
      //SplitRandomRectangle();
    }
  }
  public float pixelPerUnit = 32;

  private void SplitRandomRectangle()
  {
    if(rectangles.Count == 0) return;
    BodyRectangle r = rectangles
                .OrderBy(_ => Guid.NewGuid())
                .OrderByDescending((x1) => Mathf.Min(x1.VerticalSize, x1.HorizontalSize))
                .First();

    // choose random point inside rectangle
    float randXMax = r.HorizontalSize / 2f + r.position.x - 1;
    float randXMin = -r.HorizontalSize / 2f + r.position.x + 1;
    float randYMax = r.VerticalSize / 2f + r.position.y - 1;
    float randYMin = -r.VerticalSize / 2f + r.position.y + 1;


    float x = Random.Range(randXMin, randXMax);
    x = Mathf.Round(x * pixelPerUnit) / pixelPerUnit;
    float y = Random.Range(randYMin, randYMax);
    y = Mathf.Round(y * pixelPerUnit) / pixelPerUnit;

    Array values = Enum.GetValues(typeof(BodySplitType));
    System.Random random = new System.Random();
    BodySplitType randomSplit = (BodySplitType)values.GetValue(random.Next(values.Length - 1));

    float aspectRatio = r.HorizontalSize / r.VerticalSize;

    if (aspectRatio <= 1)
    {
      if (randYMax < randYMin)
      {
        print("not possible h");
        //return;
      }
      SplitRectangle(r, new Vector2(x, y), BodySplitType.Horizontal);
    }
    else
    {
      if (randXMax < randXMin)
      {
        print("not possible v");
        //return;
      }
      SplitRectangle(r, new Vector2(x, y), BodySplitType.Vertical);
    }
  }

  public void SplitRectangle(BodyRectangle r, Vector2 pos, BodySplitType splitType)
  {
    // O2 Info for split rectangle
    var O2 = r.GetComponent<OxigenationMeter>();
    var O2Ratio = O2.OxygenRatio;

    BodyRectangle Ob1 = null;
    BodyRectangle Ob2 = null;

    switch (splitType)
    {
      case BodySplitType.Vertical:
        Vector2 newPos = new Vector2((r.position.x - r.HorizontalSize / 2 + pos.x) / 2, r.position.y);
        Ob1 = Instantiate(this.bodyRectPrefab, newPos, Quaternion.identity, r.transform.parent);
        Ob1.Initialize(Mathf.Abs(pos.x - (r.position.x - r.HorizontalSize / 2)), r.VerticalSize);
        Ob1.transform.position = newPos;

        newPos = new Vector2((r.position.x + r.HorizontalSize / 2 + pos.x) / 2, r.position.y);
        Ob2 = Instantiate(this.bodyRectPrefab, newPos, Quaternion.identity, r.transform.parent);
        Ob2.Initialize(Mathf.Abs(pos.x - (r.position.x + r.HorizontalSize / 2)), r.VerticalSize);
        Ob2.transform.position = newPos;
        break;
      case BodySplitType.Horizontal:
        Vector2 newPosV = new Vector2(r.position.x, (r.position.y - r.VerticalSize / 2 + pos.y) / 2);
        Ob1 = Instantiate(this.bodyRectPrefab, newPosV, Quaternion.identity, r.transform.parent);
        Ob1.Initialize(r.HorizontalSize, Mathf.Abs(pos.y - (r.position.y - r.VerticalSize / 2)));
        Ob1.transform.position = newPosV;

        newPosV = new Vector2(r.position.x, (r.position.y + r.VerticalSize / 2 + pos.y) / 2);
        Ob2 = Instantiate(this.bodyRectPrefab, newPosV, Quaternion.identity, r.transform.parent);
        Ob2.Initialize(r.HorizontalSize, Mathf.Abs(pos.y - (r.position.y + r.VerticalSize / 2)));
        Ob2.transform.position = newPosV;
        break;
    }

    OxygenPerArea.Value += OxygenPerAreaIncrementerSplit.Value;

    {
      var newO2 = Ob1.GetComponent<OxigenationMeter>();
      var maxOxygen = Ob1.Area * OxygenPerArea.Value;
      newO2.SetOxygenData(O2Ratio, maxOxygen);
      StartCoroutine(FadeIn(Ob1));
      rectangles.Add(Ob1);
    }

    {
      var newO2 = Ob2.GetComponent<OxigenationMeter>();
      var maxOxygen = Ob2.Area * OxygenPerArea.Value;
      newO2.SetOxygenData(O2Ratio, maxOxygen);
      StartCoroutine(FadeIn(Ob2));
      rectangles.Add(Ob2);
    }

    // delete rectangle
    rectangles.Remove(r);
    StartCoroutine(FadeOut(r));
  }

  void OnRectangleDied(BodyRectangle r)
  {
    print("Deleting from rectangle list");
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