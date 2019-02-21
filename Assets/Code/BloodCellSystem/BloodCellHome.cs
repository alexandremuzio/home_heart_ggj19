using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BloodCellHome : MonoBehaviour
{
  [SerializeField]
  private GameObject prefab;

  [SerializeField]
  private FloatValue CellCount;

  void SpawnNewCell()
  {
    StartCoroutine(CellFadeIn());
    CellCount.Value++;
  }

  // Start is called before the first frame update
  void Start()
  {
    CellCount.Value = 0;
    SpawnNewCell();
    GameEvents.ActiveCellDied += SpawnNewCell;
  }

  // Update is called once per frame
  void Update()
  {
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    var bloodCell = collision.gameObject.GetComponent<BloodCellMovement>();
    if (bloodCell && bloodCell.leftHomeBase && bloodCell.Mode == CellMoveMode.Play)
    {
      bloodCell.OnCellBack();
      collision.enabled = false;
      SpawnNewCell();
      this.DoAfterTime(2f, () => collision.enabled = true);
    }
  }

  IEnumerator CellFadeIn()
  {
    var totalTime = 2f;
    var renderer = GetComponent<SpriteRenderer>();

    return CoroutineHelpers.InterpolateByTime(totalTime, (x =>
    {
      var newScale = totalTime * Mathf.Min(0.5f, x);
      transform.localScale = new Vector3(newScale, newScale);
    }), () =>  Instantiate(prefab, this.transform));
  }
}
