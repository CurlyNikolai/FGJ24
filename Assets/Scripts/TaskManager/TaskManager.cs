using Unity.Netcode;
using UnityEngine;

public class TaskManager : NetworkBehaviour
{
    [Command]
    public static void AssignTask(string taskName, string playerName)
    {
        Debug.Log($"Assign task {taskName} to {playerName}");
    }
}
