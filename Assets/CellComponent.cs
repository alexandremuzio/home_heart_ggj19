using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;
using Unity.Mathematics;

[Serializable]
public struct CellData : IComponentData
{
  public float speed;
  public float amplitude;
  public float3[] positionLoop;
}

[UnityEngine.DisallowMultipleComponent]
public class DummyComponent : Unity.Entities.ComponentDataWrapper<CellData> { }
