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
  private static SystemParameters Instance
  {
    get
    {
      var instance = FindObjectOfType<SystemParameters>();
      if (instance != Instance)
      {
        Destroy(instance);
        return _instance;
      }
      _instance = instance;
      return _instance;
    }
  }
}
