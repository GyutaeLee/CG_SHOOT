using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;

public class PlayerUnitMovementSystem : JobComponentSystem
{
    public struct PlayerUnitMovementJob: IJobForEach<PlayerInput, UnitNavAgent, PlayerUnitSelect>
    {
        public float deltaTime;
        public float3 mousePosition;

        public void Execute([ReadOnly] ref PlayerInput playerInput, ref UnitNavAgent unitNavAgent, [ReadOnly] ref PlayerUnitSelect selected)
        {
            if (playerInput.IsRightClick)
            {
                unitNavAgent.FinalDestination = mousePosition;
                unitNavAgent.AgentStatus = NavAgentStatus.Moving;
            }
        } 
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null)
            {
                mousePosition = new float3(hit.point.x, 0, hit.point.z);
            }
        }

        var job = new PlayerUnitMovementJob
        {
            mousePosition = mousePosition,
        };

        return job.Schedule(this, inputDeps);
    }
}
