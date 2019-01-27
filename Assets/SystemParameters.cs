using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemParameters : MonoBehaviour
{
  private static SystemParameters Instance;
  public static CellSystemParameters CellSystem => Instance._cellSystemParameters;
  [SerializeField]
  private CellSystemParameters _cellSystemParameters;

  public static BodyRectangleSystemParameters BodyRectangleSystem => Instance._bodyRectangleSystemParameters;
  [SerializeField]
  private BodyRectangleSystemParameters _bodyRectangleSystemParameters;
  void Awake()
  {
    var instance = FindObjectOfType<SystemParameters>();
    if (instance != Instance)
    {
      Destroy(instance);
      return;
    }
    Instance = instance;
  }
}
