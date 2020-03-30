using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class RTSBoostrap
{
    public static EntityArchetype PlayerUnitArchetype;

    private static RenderMesh m_CubeRenderer;
    private static EntityManager m_EntityManager;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        // This method creates archetypes for entities we will spawn frequently in this game.
        // Archetypes are optional but can speed up entity spawning substantially.
        m_EntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        PlayerUnitArchetype = m_EntityManager.CreateArchetype(
            typeof(LocalToWorld),
            typeof(RenderBounds),

            typeof(Translation),
            typeof(MoveSpeed),
            typeof(PlayerInput)
            );
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void InitializeWithScene()
    {
        m_CubeRenderer = GetLookFromPrototype("CubePrototype");

        StartRTSGame();
    }

    public static void StartRTSGame()
    {
        for (int index = 0; index < 3; index++)
        {
            Entity playerUnit = m_EntityManager.CreateEntity(PlayerUnitArchetype);

            m_EntityManager.SetComponentData(playerUnit, new Translation { Value = new float3(index * 5, 0.5f, 0) });
            m_EntityManager.AddSharedComponentData(playerUnit, m_CubeRenderer);
        }
    }


    public static RenderMesh GetLookFromPrototype(string protoName)
    {
        var proto = GameObject.Find(protoName);
        var result = proto.GetComponent<RenderMeshProxy>().Value;

        return result;
    }

}