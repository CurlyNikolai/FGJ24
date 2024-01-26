using System.Collections.Generic;
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

    void Awake() {
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
        LobbyHostListController controller = new LobbyHostListController(_hostListScreen);

        NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
        {
            controller.AddItem(LobbyManager.localUsername);
            // Debug.Log($"Client {clientId} connected");
        };
        // NetworkManager.Singleton.OnClientDisconnectCallback += (clientId) => Debug.Log($"Client {clientId} disconnect");


        controller.Back = () =>
        {
            _lobbyMainScreen.Display(false);
            _hostJoinScreen.Display(true);
            _hostListScreen.Display(false);
            _clientJoinScreen.Display(false);
            _clientListScreen.Display(false);

            controller.ClearList();

            lobbyManager.Disconnect();
        };
        controller.Start = () =>
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

                foreach (NetworkObject gameObj in GameObject.FindObjectsOfType<NetworkObject>()) {
                    if (gameObj.name == "PlayerLobby(Clone)") {
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
        LobbyClientListController controller = new LobbyClientListController(_clientListScreen);

        NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
        {
            controller.AddListEntry(LobbyManager.localUsername);
            // Debug.Log($"Client {clientId} connected");
        };
        // NetworkManager.Singleton.OnClientDisconnectCallback += (clientId) => Debug.Log($"Client {clientId} disconnect");

        controller.Back = () =>
        {
            _lobbyMainScreen.Display(false);
            _hostJoinScreen.Display(false);
            _hostListScreen.Display(false);
            _clientJoinScreen.Display(true);
            _clientListScreen.Display(false);

            controller.ClearList();

            lobbyManager.Disconnect();
        };
    }

    
    private List<string> connectedUsernames = new List<string>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        Debug.Log($"Spawned player {LobbyManager.localUsername}");

    }

}
