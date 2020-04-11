using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;

public class SelectingSystem : ComponentSystem
{
    private EntityQuery m_Highlights;
    private EntityQuery m_SelectedUnits;

    protected override void OnCreate()
    {
        m_Highlights = GetEntityQuery(typeof(HighlightSpawner));
        m_SelectedUnits = GetEntityQuery(typeof(Selecting));
    }

    protected override void OnUpdate()
    {
        // Get all selected units
        using (var selectedUnits = m_SelectedUnits.ToEntityArray(Allocator.TempJob))
        using (var highlights = m_Highlights.ToEntityArray(Allocator.TempJob))
        {
            // TODO: Find a better way to spawn highlight prefabs
            // Works right now since we know there will be at least one HighlightSpawner.
            var highlight = highlights[0];
            var prefab = EntityManager.GetComponentData<HighlightSpawner>(highlight).Prefab;

            foreach (var selectedUnit in selectedUnits)
            {
                // Remove the component from the unit so this system doesn't continually run.
                EntityManager.RemoveComponent<Selecting>(selectedUnit);

                // Get our prefab from our spawner and set Translation (to produce a LocalToWorld)
                var entity = EntityManager.Instantiate(prefab);
                EntityManager.AddComponent(entity, typeof(Highlight));

                // For a child to successfully have a parent it needs:
                // 1. LocalToWorld (either a translation or rotation)
                // 2. LocalToParent
                // 3. Parent
                EntityManager.SetComponentData(entity, new Translation { Value = new float3(0, -0.5f, 0) });
                var localParent = EntityManager.GetComponentData<LocalToWorld>(selectedUnit).Value;
                EntityManager.AddComponentData(entity, new LocalToParent { Value = localParent });
                EntityManager.AddComponentData(entity, new Parent { Value = selectedUnit });
            }
        }
    }
}
