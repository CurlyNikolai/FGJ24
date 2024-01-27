using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TaskTarget : MonoBehaviour
{
    public bool occupied = false;
    public float targetRadius = 1.0f; 

    public static event Action<Player, Vector3> PlayerEnteredTarget;
    public static event Action<Player, Vector3> PlayerExitedTarget;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, targetRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("COLLISION");
        if (other.transform.root.TryGetComponent<Player>(out var player))
        {
            PlayerEnteredTarget.Invoke(player, transform.position);
            Debug.Log(player.playerName.Value.ToString() + " entered target!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.TryGetComponent<Player>(out var player))
        {
            PlayerExitedTarget.Invoke(player, transform.position);
            Debug.Log(player.playerName.Value.ToString() + " exited target!");
        }
    }
}
