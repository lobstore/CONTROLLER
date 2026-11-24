using System;
using UnityEngine;

[Serializable]
public class StepData
{
    [field: SerializeField][field: Range(0f, 1f)] public float StepHeightPercentage { get; private set; } = 0.25f;
    [field: SerializeField][field: Range(0f, 5f)] public float FloatingRayDistance { get; private set; } = 2f;
    [field: SerializeField][field: Range(0f, 300f)] public float StepReachForce { get; private set; } = 25f;
}