using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private Transform moveRoot;
    [SerializeField] private Vector3 cameraOffsetPos;
    [SerializeField] private PlayerCamera playerCameraPrefab;

    PlayerCamera playerCamera;

    Vector3 moveDirection;

    private void Awake()
    {
        if (!IsOwner) return;

        // Initialize input controller
        InputController.RequestMove += PlayerMove;

        // Player camera
        playerCamera = Instantiate(playerCameraPrefab);
        playerCamera.target = moveRoot;
    }

    private void LateUpdate()
    {
        if (!IsOwner) return;

        var velocity = moveDirection * speed;
        var nextPos = moveRoot.position + velocity * Time.deltaTime;
        moveRoot.position = nextPos;
    }

    void PlayerMove(Vector2 dir)
    {
        moveDirection = new Vector3(dir.x, 0, dir.y).normalized;
    }
}
