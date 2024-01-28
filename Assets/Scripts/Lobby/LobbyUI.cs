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

    private VisualElement _creditsScreen;

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
        _creditsScreen = root.Q("CreditsScreen");

        SetupDefaultView();

        SetupMainController();
        SetupCreditsController();
        SetupHostJoinController();
        SetupHostListController();
        SetupClientJoinController();
        SetupClientListController();

        SetupConnectionListeners();
    }

    private void SetupConnectionListeners()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += (clientId) =>
        {
            if (IsHost)
            {
                Debug.Log($"Disconnect {clientId} on Host");
                List<string> connectedPlayerNames = new List<string>();
                foreach (var client in NetworkManager.ConnectedClients.Values)
                {
                    if (client.ClientId == clientId) continue;
                    string clientUsername = client.PlayerObject.GetComponent<LobbyPlayerData>().playerName.Value.ToString();
                    Debug.Log("Send this to client: " + client.ClientId + " " + clientUsername);
                    connectedPlayerNames.Add(clientUsername);
                }
                hostListController.ClearList();
                hostListController.SetClientList(connectedPlayerNames);

                ClearClientListClientRpc();
                for (int i = 0; i < connectedPlayerNames.Count; i++)
                {
                    UpdateClientListClientRpc(i, connectedPlayerNames[i]);
                }
            }
        };

    }

    private void ShowScreen(bool main = false, bool hostJoin = false, bool hostList = false, bool clientJoin = false, bool clientList = false, bool credits = false)
    {
        _lobbyMainScreen.Display(main);
        _hostJoinScreen.Display(hostJoin);
        _hostListScreen.Display(hostList);
        _clientJoinScreen.Display(clientJoin);
        _clientListScreen.Display(clientList);
        _creditsScreen.Display(credits);
    }

    private void SetupDefaultView()
    {
        ShowScreen(true, false, false, false, false, false);
    }

    private void SetupMainController()
    {
        LobbyMainController controller = new LobbyMainController(_lobbyMainScreen);
        controller.CreateGame = () =>
        {
            ShowScreen(false, true, false, false, false, false);
        };
        controller.JoinGame = () =>
        {
            ShowScreen(false, false, false, true, false);
        };
        controller.Credits = () =>
        {
            ShowScreen(false, false, false, false, false, true);
        };
    }

    private void SetupCreditsController()
    {
        CreditsController controller = new CreditsController(_creditsScreen);
        controller.Back = () =>
        {
            ShowScreen(true, false, false, false, false, false);
        };
    }

    private void SetupHostJoinController()
    {
        LobbyHostJoinController controller = new LobbyHostJoinController(_hostJoinScreen);
        controller.Back = () =>
        {
            ShowScreen(true, false, false, false, false, false);
        };
        controller.Join = () =>
        {
            ShowScreen(false, false, true, false, false, false);
            Debug.Log($"host: username={controller.Username()}");
            lobbyManager.StartHost(controller.Username());
        };
    }

    private void SetupHostListController()
    {
        hostListController = new LobbyHostListController(_hostListScreen);

        hostListController.Back = () =>
        {
            ShowScreen(false, true, false, false, false, false);

            hostListController.ClearList();

            ClearClientListClientRpc();
            BackClientListClientRpc();

            lobbyManager.Disconnect();
        };
        hostListController.Start = () =>
        {
            NetworkManager.Singleton.SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += (sceneName, loadSceneMode, clientsCompleted, clientsTimedOut) =>
            {
                Dictionary<ulong, string> clientNames = new Dictionary<ulong, string>();

                foreach (NetworkObject networkObject in GameObject.FindObjectsOfType<NetworkObject>())
                {
                    if (networkObject.name == "PlayerLobby(Clone)")
                    {
                        string name = networkObject.GetComponent<LobbyPlayerData>().playerName.Value.ToString();
                        ulong id = networkObject.OwnerClientId;
                        clientNames.Add(id, name);
                    }
                };

                foreach (var clientId in clientsCompleted)
                {
                    GameObject newPlayer = null;
                    newPlayer = Instantiate(playerPrefabA, new Vector3(0, 0, 0), Quaternion.identity);
                    newPlayer.transform.name = clientNames[clientId];
                    newPlayer.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);

                    if (IsServer)
                    {
                        var p = newPlayer.GetComponent<Player>();
                        p.playerName.Value = clientNames[clientId];
                    }
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
            ShowScreen(true, false, false, false, false, false);
        };
        controller.Join = () =>
        {
            ShowScreen(false, false, false, false, true, false);

            Debug.Log($"host: username={controller.Username()} connecting to {controller.HostAddress()}");
            lobbyManager.ConnectClient(controller.Username(), controller.HostAddress());
        };
    }

    private void SetupClientListController()
    {
        clientListController = new LobbyClientListController(_clientListScreen);

        clientListController.Back = () =>
        {
            ShowScreen(false, false, false, true, false, false);

            clientListController.ClearList();

            lobbyManager.Disconnect();
        };
    }


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
        for (int i = 0; i < connectedPlayerNames.Count; i++)
        {
            UpdateClientListClientRpc(i, connectedPlayerNames[i]);
        }
    }

    [ClientRpc]
    public void ClearClientListClientRpc()
    {
        clientListController.ClearList();
    }

    [ClientRpc]
    public void UpdateClientListClientRpc(int index, string name)
    {
        clientListController.SetClient(index, name);
    }

    [ClientRpc]
    public void BackClientListClientRpc()
    {
        ShowScreen(false, false, false, true, false, false);

        clientListController.ClearList();

        lobbyManager.Disconnect();
    }
}
