﻿using System;
using Unity.Entities;
using Unity.Mathematics;

public struct PlayerInput : IComponentData
{
    public bool IsLeftClick;
    public bool IsRightClick;
}