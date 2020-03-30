using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Jobs;

public class PlayerInputSystem : JobComponentSystem
{
    struct PlayerInputJob : IJobForEach<PlayerInput>
    {
        public BlittableBool IsLeftClick;
        public BlittableBool IsRightClick;
        public float3 MousePosition;

        public void Execute(ref PlayerInput data)
        {
            data.IsLeftClick = IsLeftClick;
            data.IsRightClick = IsRightClick;
            data.MousePosition = MousePosition;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var mousePosition = Input.mousePosition;
        var ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            mousePosition = new float3(hit.point.x, 0, hit.point.z);
        }

        var job = new PlayerInputJob
        {
            IsLeftClick = Input.GetMouseButtonDown(0),
            IsRightClick = Input.GetMouseButtonDown(1),
            MousePosition = Input.mousePosition,
        };

        return job.Schedule(this, inputDeps);
    }
}
