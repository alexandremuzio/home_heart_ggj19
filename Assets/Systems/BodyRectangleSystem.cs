using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using Unity.Rendering;

public class BodyRectangleSystem : ComponentSystem
{
    struct DecreaseOxygenComponents
    {
        public Position position;
        public BodyRectangleComponent bodyRect;
    }

    struct ChangeColorOxygenComponents
    {
        public SpriteRenderer renderer;
        public BodyRectangleComponent body;
    }

    protected override void OnUpdate()
    {
        foreach (var e in GetEntities<DecreaseOxygenComponents>()){ 
            e.bodyRect.OxygenLevel -= Time.deltaTime * e.bodyRect.OxygenDecreaseSpeed;
        }

        foreach (var e in GetEntities<ChangeColorOxygenComponents>()) {
            e.renderer.col
        }
    }
}    