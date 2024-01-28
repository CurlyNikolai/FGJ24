using Unity.Netcode;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering.UI;

public class TaskManager : NetworkBehaviour
{
    public static TaskManager instance;

    private void OnEnable()
    {
        if (instance == null) instance = this;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        Player.FinishedTask += FinishTask;
    }

    void AssignTask(string playerName, Task task)
    {
        if (!IsServer)
        {
            Debug.Log("Unable to run server authorative command");
            return;
        }

        Debug.Log($"Assigning task {task.type} to player {playerName}");

        var availableTargets = FindObjectsOfType<TaskTarget>().Where(t => !t.occupied).ToArray();

        if (availableTargets.Length == 0)
        {
            Debug.Log("No available tasks");
            return;
        }

        var randomTarget = availableTargets[Random.Range(0, availableTargets.Length - 1)];
        task.targetPos = randomTarget.transform.position;
        task.targetTime = 2.0f;
        randomTarget.occupied = true;

        foreach (Player player in FindObjectsOfType<Player>())
        {
            if (player.playerName.Value.ToString() == playerName)
            {
                player.task.Value = task;
                break;
            }
        }
    }

    void FinishTask(Player player)
    {
        if (!IsServer)
        {
            Debug.Log("Unable to run server authorative command");
            return;
        }

        Debug.Log(player.playerName.Value.ToString() + " finished task!");

        var task = player.task.Value;
        task.type = TaskType.none;
        task.targetPos = Vector3.zero;
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
