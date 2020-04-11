using Unity.Entities;
using Unity.Mathematics;

public struct AABB : IComponentData
{
    public float3 Min;
    public float3 Max;
} 