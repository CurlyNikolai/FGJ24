using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class TaskManager : NetworkBehaviour
{
    public static TaskManager instance;

    private void OnEnable()
    {
        if (instance == null) instance = this;
    }

    void AssignTask(string playerName, Task task)
    {
        Debug.Log($"Assign task {task.type} to player {playerName}");
    }

    [Command]
    public static void AssignTask(string taskName, string playerName)
    {
        var task = new Task();
        taskName = taskName.ToLower();

        try
        {
            task.type = (TaskType)System.Enum.Parse(typeof(TaskType), taskName);
        }
        catch
        {
            Debug.Log("Did not recognise given task");
            return;
        }

        instance.AssignTask(playerName, task);
    }
}
