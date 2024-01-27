using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TurnTowardsClosestsPlayer : NetworkBehaviour
{
    [SerializeField]
    private float speed = 1.0f;

    private Player[] players;
    private Transform closestPlayer;

    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectsOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        GetClosestPlayerTransform();

        UpdateRotation();
    }

    private void GetClosestPlayerTransform()
    {
        float closest = Mathf.Infinity;
        foreach (Player player in players)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance < closest)
            {
                closest = distance;
                closestPlayer = player.transform;
            }
        }
    }

    private void UpdateRotation() {
        Vector3 direction = closestPlayer.transform.position - transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = speed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);

    }
}
