using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCellStats : MonoBehaviour
{

  [SerializeField]
  private FloatValue CurrentOxygen;

  [SerializeField]
  private FloatValue CurrentTimeAlive;

  private float timeAlive;
  private BloodCellMovement movement;
  private CellMoveMode currentState => movement.Mode;

  private OxigenationMeter oxigenation;
  // Start is called before the first frame update
  void Start()
  {
    movement = GetComponent<BloodCellMovement>();
    oxigenation = GetComponent<OxigenationMeter>();
  }

  // Update is called once per frame
  void Update()
  {
    timeAlive += Time.deltaTime;
    if (currentState == CellMoveMode.Play)
    {
      CurrentOxygen.Value = oxigenation.OxygenRatio;
      CurrentTimeAlive.Value = timeAlive;
    }
  }
}
