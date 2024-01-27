using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private Transform moveRoot;
    [SerializeField] private Rigidbody stickRigidBody;
    [SerializeField] private float impulseSize = 7.48f;
    [SerializeField] private Vector3 cameraOffsetPos;
    [SerializeField] private GameObject playerCameraPrefab;

    PlayerCamera playerCamera;

    Vector3 moveDirection;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        transform.position += Vector3.up * 2;

        if (!IsOwner) return;

        // Initialize input controller
        InputController.RequestMove += PlayerMove;
        InputController.RequestReset += PlayerReset;

        // Player camera
        playerCamera = Instantiate(playerCameraPrefab).GetComponent<PlayerCamera>();
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

    void PlayerReset(float value)
    {
        stickRigidBody.AddForce(new Vector3(0, impulseSize, 0), ForceMode.Impulse);
        // stickRigidBody.AddForce(new Vector3(0, impulseSize, 0), ForceMode.VelocityChange);
    }

}
