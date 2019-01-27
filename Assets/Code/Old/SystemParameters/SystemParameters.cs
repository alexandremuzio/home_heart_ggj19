using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemParameters : MonoBehaviour
{
  public static CellSystemParameters CellSystem => Instance._cellSystemParameters;
  [SerializeField]
  private CellSystemParameters _cellSystemParameters;

  public static BodyRectangleSystemParameters BodyRectangleSystem => Instance._bodyRectangleSystemParameters;
  [SerializeField]
  private BodyRectangleSystemParameters _bodyRectangleSystemParameters;
  private static SystemParameters _instance;
  public static SystemParameters Instance
  {
    get
    {
      if (_instance == null)
      {
          _instance = FindObjectOfType<SystemParameters>();
      }

      return _instance;
    }
  }
}
