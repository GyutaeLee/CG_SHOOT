using Unity.Entities;

public struct UnitSpawner : IComponentData
{
    public int CountX;
    public int CountY;
    public Entity Prefab;
}