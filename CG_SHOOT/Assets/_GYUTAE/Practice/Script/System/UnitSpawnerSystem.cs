using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;


// ComponentSystems run on the main thread.
// Use these when you to do work that cannot be called from a job.
public class UnitSpawnerSystem : ComponentSystem
{
    private EntityQuery m_SpawnerEntityQuery;

    protected override void OnCreate()
    {
        m_SpawnerEntityQuery = GetEntityQuery(typeof(UnitSpawner), typeof(Translation));
    }

    protected override void OnUpdate()
    {
        // Get all the spawners in the scene.
        using (var spawners = m_SpawnerEntityQuery.ToEntityArray(Allocator.TempJob))
        {
            foreach (var spawner in spawners)
            {
                // Create an entity from the prefab set on the spawner component.
                var prefab = EntityManager.GetSharedComponentData<UnitSpawner>(spawner).Prefab;
                var entity = EntityManager.Instantiate(prefab);

                // Copy the translation of the spawner to the new entity.
                var translation = EntityManager.GetComponentData<Translation>(spawner);
                EntityManager.SetComponentData(entity, translation);

                // Destroy the spawner so this system only runs once.
                EntityManager.DestroyEntity(spawner);
            }
        }
    }

}
