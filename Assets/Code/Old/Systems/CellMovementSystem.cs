using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Transforms;

public class CellMovementSystem : JobComponentSystem
{
  struct CellMovementJob : IJobProcessComponentData<CellData, Position>
  {
    [ReadOnly] public float dT;
    [ReadOnly] public float totalTime;

    public void Execute(ref CellData c0, ref Position c1)
    {
      var pos = c1.Value;
      pos.x = c0.speed;
      c1.Value = pos;
    }
  }

  protected override JobHandle OnUpdate(JobHandle inputDeps)
  {
    var job = new CellMovementJob()
    {
      dT = Time.deltaTime,
      totalTime = Time.time,
    };

    var g = SystemParameters.CellSystem.CellGradient;

    return job.Schedule(this, inputDeps);
  }
}