using Unity.Netcode;
using UnityEngine;
using System.Linq;

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

        Debug.Log($"Assigning task {task.type} to player {playerName}");

        var availableTargets = FindObjectsOfType<TaskTarget>().Where(t => !t.occupied.Value).ToArray();
        var randomTarget = availableTargets[Random.Range(0, availableTargets.Length - 1)];

        task.targetPos = randomTarget.transform.position;

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
