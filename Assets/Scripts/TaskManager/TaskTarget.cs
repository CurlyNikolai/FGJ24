using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TaskTarget : NetworkBehaviour
{
    public NetworkVariable<bool> occupied = new NetworkVariable<bool>(false);

    public float targetRadius = 1.0f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, targetRadius);
    }
}
