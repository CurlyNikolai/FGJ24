using Unity.Netcode;
using UnityEngine;

public enum TaskType
{
    none,
    gather,
    move
}

/// <summary> Data container for player task </summary>
public struct Task: INetworkSerializable
{
    public TaskType type;
    public Vector3 targetPos;
    public float targetTime;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        if (serializer.IsReader)
        {
            var reader = serializer.GetFastBufferReader();
            reader.ReadValueSafe(out type);
            reader.ReadValueSafe(out targetPos);
            reader.ReadValueSafe(out targetTime);
        }
        else
        {
            var writer = serializer.GetFastBufferWriter();
            writer.WriteValueSafe(type);
            writer.WriteValueSafe(targetPos);
            writer.WriteValueSafe(targetTime);
        }
    }
}