using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public class BodyRectangleComponent : MonoBehaviour
{
    public float3 Position;
    public float OxygenLevel;
    public float OxygenDecreaseSpeed;
    public Sprite Sprite;
}
