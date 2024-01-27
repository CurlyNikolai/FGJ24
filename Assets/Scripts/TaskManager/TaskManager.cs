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
        if (!IsServer)
        {
            Debug.Log("Unable to run server authorative command");
            return;
        }

        Debug.Log($"Assign task {task.type} to player {playerName}");
        foreach (Player player in FindObjectsOfType<Player>())
        {
            if (player.playerName.Value.ToString() == playerName)
            {
                player.task.Value = task;
                break;
            }
        }
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
