using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

// ComponentSystems run on the main thread.
// Use these when you have to do work that cannot be called from a job.
public class DeselectSystem : ComponentSystem
{
    private EntityQuery m_Highlights;

    // TODO: Not an efficient system, it will always run as long as Highlights as active.
    // The Problem is only the child knows of it's parent and not the otherway around.
    // So we have to go backwards from child -> parent to effectively destroy the entity.
    protected override void OnCreate()
    {
        m_Highlights = GetEntityQuery(typeof(Highlight));
    }

    protected override void OnUpdate()
    {
        using (var attachedHighlights = m_Highlights.ToEntityArray(Allocator.TempJob))
        {
            foreach (var highlight in attachedHighlights)
            {
                var parent = EntityManager.GetComponentData<Parent>(highlight).Value;

                if (EntityManager.HasComponent<Deselecting>(parent))
                {
                    EntityManager.RemoveComponent<Deselecting>(parent);
                    EntityManager.DestroyEntity(highlight);
                }
            }
        }
    }


}