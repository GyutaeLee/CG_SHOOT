using System;
using Unity.Entities;
using UnityEngine;

// register component type at compile time
[assembly: RegisterGenericComponentType(typeof(UnitSpawner))]

// ISharedComponentData can have struct members with managed types.
[Serializable]
public struct UnitSpawner : ISharedComponentData
{
    public GameObject Prefab;
}

public class UnitSpawnerComponent : SharedComponentDataProxy<UnitSpawner> {  }