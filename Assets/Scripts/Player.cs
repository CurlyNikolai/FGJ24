using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    PlayerCamera playerCamera;

    Vector3 moveDirection;

    public Task currentTask;

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
            Debug.Log($"{playerName.Value.ToString()} has new task {newTask.type} at position {newTask.targetPos}");
        };

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
