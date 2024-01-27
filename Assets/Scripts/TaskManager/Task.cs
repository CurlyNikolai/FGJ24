using UnityEngine;

public enum TaskType
{
    gather,
    move
}

/// <summary> Data container for player task </summary>
public class Task
{
    public TaskType type;
    public Vector3 targetPos;
    public float targetTime;
}