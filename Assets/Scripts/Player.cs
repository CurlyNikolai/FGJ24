using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private Transform moveRoot;
    [SerializeField] private Vector3 cameraOffsetPos;
    [SerializeField] private PlayerCamera playerCameraPrefab;

    PlayerCamera playerCamera;

    Vector3 moveDirection;

    private void Awake()
    {
        // Initialize input controller
        InputController.RequestMove += PlayerMove;

        // Player camera
        playerCamera = Instantiate(playerCameraPrefab);
        playerCamera.target = moveRoot;
    }

    private void LateUpdate()
    {
        var velocity = moveDirection * speed;
        var nextPos = moveRoot.position + velocity * Time.deltaTime;
        moveRoot.position = nextPos;
    }

    void PlayerMove(Vector2 dir)
    {
        moveDirection = new Vector3(dir.x, 0, dir.y).normalized;
    }
}
