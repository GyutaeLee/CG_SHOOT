using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct PlayerInput : IComponentData
{
    public BlittableBool IsLeftClick;
    public BlittableBool IsRightClick;
    public float3 MousePosition;
}
