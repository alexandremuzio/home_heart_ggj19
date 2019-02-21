using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellMoveMode
{
  Play,
  Replay,
  Dead,
}

public class BloodCellState : MonoBehaviour
{
  public CellMoveMode Mode { get; private set; }
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
}
