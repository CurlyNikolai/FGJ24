using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(LobbyManager))]
public class LobbyUI : NetworkBehaviour
{
    [SerializeField] private string gameScene;

    [SerializeField] private LobbyManager lobbyManager;

    [SerializeField]
    private GameObject playerPrefabA;
    [SerializeField]
    private GameObject playerPrefabB;

    private VisualElement _lobbyMainScreen;

    private VisualElement _hostJoinScreen;
    private VisualElement _hostListScreen;

    private VisualElement _clientJoinScreen;
    private VisualElement _clientListScreen;

    private LobbyHostListController hostListController;
    private LobbyClientListController clientListController;

    void Awake()
    {
        lobbyManager = GetComponent<LobbyManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        _lobbyMainScreen = root.Q("LobbyMainScreen");
        _hostJoinScreen = root.Q("LobbyHostJoinScreen");
        _hostListScreen = root.Q("LobbyHostListScreen");
        _clientJoinScreen = root.Q("LobbyClientJoinScreen");
        _clientListScreen = root.Q("LobbyClientListScreen");

        SetupDefaultView();

        SetupMainController();
        SetupHostJoinController();
        SetupHostListController();
        SetupClientJoinController();
        SetupClientListController();

        SetupConnectionListeners();
    }

    private void SetupConnectionListeners()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
        {
            UpdateClientListServerRpc();
            // if (IsHost)
            // {
            //     // LobbyPlayerData data = NetworkManager.ConnectedClients[clientId].PlayerObject.GetComponent<LobbyPlayerData>();
            //     // string playerUsername = data.playerName.Value.ToString();
            //     // Debug.Log($"Player {clientId} = \"{playerUsername}\"");
            //     // hostListController.AddItem(clientId, playerUsername);

            //     List<string> connectedPlayerNames = new List<string>();
            //     foreach (var client in NetworkManager.ConnectedClients.Values)
            //     {
            //         string clientUsername = client.PlayerObject.GetComponent<LobbyPlayerData>().playerName.Value.ToString();
            //         Debug.Log("Send this to client: " + client.ClientId + " " + clientUsername);
            //         connectedPlayerNames.Add(clientUsername);
            //     }
            //     hostListController.ClearList();
            //     hostListController.SetClientList(connectedPlayerNames);
            // } else {
                
            // }
            // Debug.Log($"Client {clientId} connected");
        };

    }

    private void SetupDefaultView()
    {
        _lobbyMainScreen.Display(true);
        _hostJoinScreen.Display(false);
        _hostListScreen.Display(false);
        _clientJoinScreen.Display(false);
        _clientListScreen.Display(false);
    }

    private void SetupMainController()
    {
        LobbyMainController controller = new LobbyMainController(_lobbyMainScreen);
        controller.CreateGame = () =>
        {
            _lobbyMainScreen.Display(false);
            _hostJoinScreen.Display(true);
            _hostListScreen.Display(false);
            _clientJoinScreen.Display(false);
            _clientListScreen.Display(false);
        };
        controller.JoinGame = () =>
        {
            _lobbyMainScreen.Display(false);
            _hostJoinScreen.Display(false);
            _hostListScreen.Display(false);
            _clientJoinScreen.Display(true);
            _clientListScreen.Display(false);
        };

        // NetworkManager.Singleton.OnClientConnectedCallback += (clientId) => Debug.Log($"Client {clientId} connected");
        // NetworkManager.Singleton.OnClientDisconnectCallback += (clientId) => Debug.Log($"Client {clientId} disconnect");
    }

    private void SetupHostJoinController()
    {
        LobbyHostJoinController controller = new LobbyHostJoinController(_hostJoinScreen);
        controller.Back = () =>
        {
            _lobbyMainScreen.Display(true);
            _hostJoinScreen.Display(false);
            _hostListScreen.Display(false);
            _clientJoinScreen.Display(false);
            _clientListScreen.Display(false);
        };
        controller.Join = () =>
        {
            _lobbyMainScreen.Display(false);
            _hostJoinScreen.Display(false);
            _hostListScreen.Display(true);
            _clientJoinScreen.Display(false);
            _clientListScreen.Display(false);

            Debug.Log($"host: username={controller.Username()}");
            lobbyManager.StartHost(controller.Username());
        };
    }

    private void SetupHostListController()
    {
        hostListController = new LobbyHostListController(_hostListScreen);

        // NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
        // {
        //     string playerUsername = $"Player {clientId}";
        //     if (IsHost)
        //     {
        //         LobbyPlayerData data = NetworkManager.ConnectedClients[clientId].PlayerObject.GetComponent<LobbyPlayerData>();
        //         Debug.Log(playerUsername);
        //     }
        //     controller.AddItem(clientId, playerUsername);
        //     // Debug.Log($"Client {clientId} connected");
        // };
        // NetworkManager.Singleton.OnClientDisconnectCallback += (clientId) => Debug.Log($"Client {clientId} disconnect");

        hostListController.Back = () =>
        {
            _lobbyMainScreen.Display(false);
            _hostJoinScreen.Display(true);
            _hostListScreen.Display(false);
            _clientJoinScreen.Display(false);
            _clientListScreen.Display(false);

            hostListController.ClearList();

            lobbyManager.Disconnect();
        };
        hostListController.Start = () =>
        {
            NetworkManager.Singleton.SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += (sceneName, loadSceneMode, clientsCompleted, clientsTimedOut) =>
            {
                foreach (var clientId in clientsCompleted)
                {
                    // Debug.Log($"username = {clientUsername}, clientId = {clientId}");

                    GameObject newPlayer = null;
                    // if (clientId == 0)
                    // {
                    newPlayer = Instantiate(playerPrefabA);
                    // }
                    // else
                    // {
                    //     newPlayer = (GameObject)Instantiate(playerPrefabB);
                    // }
                    newPlayer.transform.name = $"Player ({clientId})";
                    newPlayer.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
                }

                foreach (NetworkObject gameObj in GameObject.FindObjectsOfType<NetworkObject>())
                {
                    if (gameObj.name == "PlayerLobby(Clone)")
                    {
                        gameObj.Despawn();
                    }
                };
            };
        };
    }

    private void SetupClientJoinController()
    {
        LobbyClientJoinController controller = new LobbyClientJoinController(_clientJoinScreen);
        controller.Back = () =>
        {
            _lobbyMainScreen.Display(true);
            _hostJoinScreen.Display(false);
            _hostListScreen.Display(false);
            _clientJoinScreen.Display(false);
            _clientListScreen.Display(false);
        };
        controller.Join = () =>
        {
            _lobbyMainScreen.Display(false);
            _hostJoinScreen.Display(false);
            _hostListScreen.Display(false);
            _clientJoinScreen.Display(false);
            _clientListScreen.Display(true);

            Debug.Log($"host: username={controller.Username()} connecting to {controller.HostAddress()}");
            lobbyManager.ConnectClient(controller.Username(), controller.HostAddress());
        };
    }

    private void SetupClientListController()
    {
        clientListController = new LobbyClientListController(_clientListScreen);

        // NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
        // {
        //     controller.AddListEntry(LobbyManager.localUsername);
        //     // Debug.Log($"Client {clientId} connected");
        // };
        // NetworkManager.Singleton.OnClientDisconnectCallback += (clientId) => Debug.Log($"Client {clientId} disconnect");

        clientListController.Back = () =>
        {
            _lobbyMainScreen.Display(false);
            _hostJoinScreen.Display(false);
            _hostListScreen.Display(false);
            _clientJoinScreen.Display(true);
            _clientListScreen.Display(false);

            clientListController.ClearList();

            lobbyManager.Disconnect();
        };
    }


    // [SerializeField]
    // public NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [ServerRpc(RequireOwnership = false)]
    void SetPlayerNameServerRpc(ulong clientId, string playerName)
    {
        SetClientNamesClientRpc(clientId, playerName);
        // TODO: update clients with a ClientRPC call
        // AddUsernameToListClientRpc(playerName);
        // foreach (var client in NetworkManager.ConnectedClients.Values)
        // {
        //     Debug.Log("Send this to client: " + client);
        // }
    }

    [ClientRpc]
    void SetClientNamesClientRpc(ulong clientId, string playerName)
    {
        // if (IsHost)
        // {
        //     hostListController.AddClient(clientId, playerName);
        // }
        // else
        // {
        //     clientListController.AddClient(clientId, playerName);
        // }
    }

    // void SetPlayerName(ulong clientId, string name)
    // {
    //     SetPlayerNameServerRpc(clientId, name);
    // }

    // NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [ServerRpc(RequireOwnership = false)]
    public void UpdateClientListServerRpc()
    {
        List<string> connectedPlayerNames = new List<string>();
        foreach (var client in NetworkManager.ConnectedClients.Values)
        {
            string clientUsername = client.PlayerObject.GetComponent<LobbyPlayerData>().playerName.Value.ToString();
            Debug.Log("Send this to client: " + client.ClientId + " " + clientUsername);
            connectedPlayerNames.Add(clientUsername);
        }
        hostListController.ClearList();
        hostListController.SetClientList(connectedPlayerNames);

        ClearClientListClientRpc();
        for (int i = 0; i < connectedPlayerNames.Count; i++) {
            UpdateClientListClientRpc(i, connectedPlayerNames[i]);
        }
    }

    [ClientRpc]
    public void ClearClientListClientRpc() {
        clientListController.ClearList();
    }

    [ClientRpc]
    public void UpdateClientListClientRpc(int index, string name)
    {
        clientListController.SetClient(index, name);
    }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            // // SetPlayerName(NetworkManager.LocalClientId, LobbyManager.localUsername);
            // playerName.Value = LobbyManager.localUsername;
            // Debug.Log(playerName.Value + " spawned!");
            // UpdateClientListServerRpc();
        }

        // if (IsHost)
        // {
        //     hostListController.AddClient(NetworkManager.LocalClientId, "HOSTHOSTHOST");
        // } else {
        //     clientListController.AddClient(NetworkManager.LocalClientId, "CLIENTCLIENTCLIENT");
        // }
        // else
        // {
        //     // playerName.OnValueChanged += (oldName, newName) =>
        //     // {
        //     //     Debug.Log(playerName.Value + " spawned!");
        //     // };
        // }
    }
}
