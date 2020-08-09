using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;

public class NavAgentMovementSystem : JobComponentSystem
{
    public struct NavAgentMovementJob : IJobForEach<Translation, UnitNavAgent>
    {
        public float dletaTime;

        public void Execute(ref Translation translation, [ReadOnly] ref UnitNavAgent unitNavAgent)
        {
            float distance = math.distance(unitNavAgent.FinalDestination, translation.Value);
            float3 direction = math.normalize(unitNavAgent.FinalDestination - translation.Value);
            float speed = 5;

            if (distance >= 0.5f && unitNavAgent.AgentStatus == NavAgentStatus.Moving)
            {
                translation.Value += direction * speed * dletaTime;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new NavAgentMovementJob
        {
            dletaTime = Time.DeltaTime
        };

        return job.Schedule(this, inputDeps);
    }
}
