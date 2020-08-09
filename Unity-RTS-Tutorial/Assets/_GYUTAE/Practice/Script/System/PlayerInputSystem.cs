using UnityEngine;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

public class PlayerInputSystem : JobComponentSystem
{
    struct PlayerInputJob : IJobForEach<PlayerInput>
    {
        public bool IsLeftClick;
        public bool IsRightClick;

        public void Execute(ref PlayerInput data)
        {
            data.IsLeftClick = IsLeftClick;
            data.IsRightClick = IsRightClick;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {      

        var job = new PlayerInputJob
        {
            IsLeftClick = Input.GetMouseButtonDown(0),
            IsRightClick = Input.GetMouseButtonDown(1),
        };

        return job.Schedule(this, inputDeps);
    }
}
