using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BloodCellOxigenation : MonoBehaviour
{
  public BloodCellData data;

  private SpriteRenderer spriterenderer;

  public BoolValue CellDiesWhenOutOfOxygen;

  private OxigenationMeter oxigenationMeter;

  private void Awake()
  {
    oxigenationMeter = GetComponent<OxigenationMeter>();
    spriterenderer = GetComponent<SpriteRenderer>();
  }

  private void Update()
  {
    spriterenderer.color = oxigenationMeter.GetColor();
    if (CellDiesWhenOutOfOxygen.Value)
    {
      print(oxigenationMeter.OxygenRatio + " " + oxigenationMeter.IsAlive);
      if (!oxigenationMeter.IsAlive)
      {
        print("dead");
        Die();
      }
    }
  }

  private void Die()
  {
    if (GetComponent<BloodCellMovement>().Mode == CellMoveMode.Play)
    {
      GameEvents.ActiveCellDied();
    }
    Destroy(this.gameObject);
  }

  private void OnTriggerStay2D(Collider2D other)
  {
    var rect = other.gameObject.GetComponent<BodyRectangle>();
    if (rect)
    {
      var rectMeter = rect.GetComponent<OxigenationMeter>();
      if (!rectMeter.IsAlive) return;
      var fillAmmount = data.OxigenDonationRate * Time.deltaTime;

      fillAmmount
      .Forward(oxigenationMeter.Take)
      .Forward(rectMeter.Give)
      .Forward(oxigenationMeter.Give);
    }

    var home = other.gameObject.GetComponent<BloodCellHome>();
    if (home)
    {
      oxigenationMeter.Give(oxigenationMeter.MaxOxygenLevel);
    }
  }
}

public static partial class FuncExtensions
{
  public static TResult Forward<T, TResult>(this T value, Func<T, TResult> function) =>
      function(value);
}
