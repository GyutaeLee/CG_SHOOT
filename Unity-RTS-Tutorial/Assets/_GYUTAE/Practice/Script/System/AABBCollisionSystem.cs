using UnityEngine;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;

public class AABBCollisionSystem : JobComponentSystem
{
    [BurstCompile]
    public struct AABBCollisionJob : IJobParallelFor//IJobChunk
    {
        [ReadOnly] public NativeArray<AABB> Colliders;

        //For IJobParallelFor
        public void Execute(int i)
        {
            for (int j = i + 1; j < Colliders.Length; j++)
            {
                if (RTSPhysics.Intersect(Colliders[i], Colliders[j]))
                {
                    Debug.Log("Collision Detected " + i + " with " + j);
                }
            }
        }

        //// For IJobChunk
        //public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        //{
        //    for (int i = 0; i < Colliders.Length; i++)
        //    {
        //        for (int j = i + 1; j < Colliders.Length; j++)
        //        {
        //            if (RTSPhysics.Intersect(Colliders[i], Colliders[j]))
        //            {
        //                Debug.Log("COLLISION OCCURED");
        //            }
        //        }
        //    }
        //}

    }

    private EntityQuery m_AABBQuery;

    protected override void OnCreate()
    {
        var query = new EntityQueryDesc
        {
            All = new ComponentType[] { typeof(AABB) }
        };

        m_AABBQuery = GetEntityQuery(query);
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var colliders = m_AABBQuery.ToComponentDataArray<AABB>(Allocator.TempJob);
        var aabbCollisionJob = new AABBCollisionJob
        {
            Colliders = colliders,
        };
        var collisionJobHandle = aabbCollisionJob.Schedule(colliders.Length, 32); // For IJobParallelFor
        //var collisionJobHandle = aabbCollisionJob.Schedule(m_AABBQuery, inputDeps); // For IJobChunk
        collisionJobHandle.Complete();

        colliders.Dispose();
        return collisionJobHandle;
    }
}