using Mono.Cecil.Cil;
using System;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : NetworkBehaviour
{
    public NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<Task> task = new NetworkVariable<Task>(new Task(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] private float speed = 1;
    [SerializeField] private Transform moveRoot;
    [SerializeField] private Rigidbody stickRigidBody;
    [SerializeField] private float impulseSize = 7.48f;
    [SerializeField] private Vector3 cameraOffsetPos;
    [SerializeField] private GameObject playerCameraPrefab;

    private bool hasFallen = false;
    private float fallCooldown;
    [SerializeField] private float fallCooldownTime = 2.0f;

    PlayerCamera playerCamera;

    Vector3 moveDirection;

    public Task currentTask;
    public bool insideTarget = false;
    public float taskTimer;

    public static event Action<Player> FinishedTask;

    private void SetupNetworkPlayer()
    {
        // if (IsOwner)
        // {
        //     playerName.Value = LobbyManager.localUsername;
        //     Debug.Log(playerName.Value + " spawned!");
        // }
        // else
        // {
        //     playerName.OnValueChanged += (oldName, newName) =>
        //     {
        //         Debug.Log(playerName.Value + " spawned!");
        //     };
        // }

        NetworkManager.OnClientDisconnectCallback += (clientId) =>
        {
            if (clientId == NetworkManager.ServerClientId)
            {
                Debug.LogWarning("server shutting down");
                SceneManager.LoadScene("Lobby");
            }
        };
    }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        SetupNetworkPlayer();

        transform.position += Vector3.up * 2;

        Debug.Log($"player {playerName.Value.ToString()} spawned!");

        task.OnValueChanged += (prevTask, newTask) =>
        {
            var pName = playerName.Value.ToString();
            Debug.Log($"{pName} has new task {newTask.type} at position {newTask.targetPos}");

            if (IsOwner)
            {
                taskTimer = newTask.targetTime;
            }
        };

        if (!IsOwner) return;

        TaskTarget.PlayerEnteredTarget += (player, position) =>
        {
            if (player.playerName.Value == playerName.Value && position.Equals(player.task.Value.targetPos))
            {
                Debug.Log("It's me!");
                insideTarget = true;
            }
        };

        TaskTarget.PlayerExitedTarget += (player, position) =>
        {
            if (player.playerName.Value == playerName.Value && position.Equals(player.task.Value.targetPos))
            {
                Debug.Log("I'm out!");
                insideTarget = false;
            }
        };

        // Initialize input controller
        InputController.RequestMove += PlayerMove;
        InputController.RequestReset += PlayerReset;

        // Player camera
        playerCamera = Instantiate(playerCameraPrefab).GetComponent<PlayerCamera>();
        playerCamera.target = moveRoot;
    }

    private void Update()
    {
        if (!IsOwner) return;
        
        if (task.Value.type != TaskType.none && taskTimer > 0)
        {
            if (insideTarget)
            {
                taskTimer -= Time.deltaTime;
                if (taskTimer <= 0)
                {
                    FinishedTask.Invoke(this);
                    Debug.Log("Task done!");   
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (!IsOwner) return;

        var velocity = moveDirection * speed;
        var nextPos = moveRoot.position + velocity * Time.deltaTime;
        moveRoot.position = nextPos;

        if (hasFallen) {
            fallCooldown -= Time.deltaTime;
        }
    }

    void PlayerMove(Vector2 dir)
    {
        if (!hasFallen) {
        moveDirection = new Vector3(dir.x, 0, dir.y).normalized;
        } else if (fallCooldown < 0) {
            stickRigidBody.AddForce(new Vector3(0, impulseSize, 0), ForceMode.Impulse);
            hasFallen = false;
        }
    }

    void PlayerReset(float value)
    {
        // Disabled. Needed only for development
        // stickRigidBody.AddForce(new Vector3(0, impulseSize, 0), ForceMode.Impulse);
    }

    public void OnCollision(Collision collision)
    {
        //if (!hasFallen)
        //{
        //    hasFallen = true;
        //    fallCooldown = fallCooldownTime;
        //    moveDirection = new Vector3(0, 0, 0).normalized;
        //}
    }

}
