using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class AABBMovementSystem : JobComponentSystem
{
    public struct AABBMovementJob : IJobForEach<AABB, Translation>
    {
        // Keep our box collider in sync with the position of the player
        public void Execute(ref AABB aabb, ref Translation translation)
        {
            aabb.Max = translation.Value + 0.5f;
            aabb.Min = translation.Value - 0.5f;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new AABBMovementJob();

        return job.Schedule(this, inputDeps);
    }
}