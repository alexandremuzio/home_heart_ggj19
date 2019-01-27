﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BloodCellHome : MonoBehaviour
{
  [SerializeField]
  private GameObject prefab;

  void SpawnNewCell()
  {
    Instantiate(prefab, this.transform);
  }

  // Start is called before the first frame update
  void Start()
  {
    SpawnNewCell();
  }

  // Update is called once per frame
  void Update()
  {
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    var bloodCell = collision.gameObject.GetComponent<BloodCellMovement>();
    if (bloodCell && bloodCell.leftHomeBase && bloodCell.mode == CellMoveMode.Play)
    {
      bloodCell.OnCellBack();
      SpawnNewCell();
    }
  }
}