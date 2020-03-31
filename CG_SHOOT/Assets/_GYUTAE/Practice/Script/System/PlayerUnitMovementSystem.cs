using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;

public class PlayerUnitMovementSystem : JobComponentSystem
{
    public struct PlayerUnitMovementJob: IJobForEach<PlayerInput, Translation>
    {
        public void Execute(ref PlayerInput playerInput, ref Translation translation)
        {
            if (playerInput.IsRightClick)
            {
                translation.Value = playerInput.MousePosition;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new PlayerUnitMovementJob();

        return job.Schedule(this, inputDeps);
    }
}
