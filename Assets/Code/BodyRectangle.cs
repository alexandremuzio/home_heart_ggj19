using System;
using System.Collections.Generic;
using UnityEngine;

public enum BodySplitType
{
  Vertical,
  Horizontal,
  Cross,
};

[RequireComponent(typeof(SpriteRenderer))]
public class BodyRectangle : MonoBehaviour
{
  public float oxgyenLevel;
  public float oxygenReduceSpeed;
  public bool isAlive;

  private new SpriteRenderer renderer;

  public Vector2 position => gameObject.transform.position;
  public float HorizontalSize => renderer.size.x;
  public float VerticalSize => renderer.size.y;

  public void Initialize(float horizontalSize, float verticalSize)
  {
    // sets rect size
    Vector2 size = new Vector2(horizontalSize, verticalSize);

    renderer.size = size;
  }

  private void Awake()
  {
    renderer = GetComponent<SpriteRenderer>();
  }

  private void Update()
  {
    this.oxgyenLevel -= Time.deltaTime * oxygenReduceSpeed;

    if (this.oxgyenLevel < 0f)
    {
      isAlive = false;
      //print("Body Rectangle is dead!");
    }
  }
}
