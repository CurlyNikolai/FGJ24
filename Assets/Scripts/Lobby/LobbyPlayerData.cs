using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyPlayerData : NetworkBehaviour
{
    private LobbyUI lobbyUI;

    [SerializeField]
    public NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    // [ServerRpc]
    // void SetPlayerNameServerRpc(FixedString32Bytes playerName) { /* ... */ }

    // void SetPlayerName(string name)
    // {
    //     SetPlayerNameServerRpc(new FixedString32Bytes(name));
    // }

    void Start()
    {

        lobbyUI = GameObject.FindObjectOfType<LobbyUI>();
        lobbyUI.UpdateClientListServerRpc();

        // if (IsOwner)
        // {
        //     playerName.Value = LobbyManager.localUsername;
        //     Debug.Log("owner: " + playerName.Value + " spawned!");
        //     // lobbyUI.UpdateClientListServerRpc();
        // }
        // else
        // {
        //     playerName.OnValueChanged += (oldName, newName) =>
        //     {
        //         Debug.Log("valuechanged: " + playerName.Value + " spawned!");
        //         // lobbyUI.UpdateClientListServerRpc();
        //     };
        // }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        lobbyUI = GameObject.FindObjectOfType<LobbyUI>();

        if (IsOwner)
        {
            playerName.Value = LobbyManager.localUsername;
            Debug.Log("owner: " + playerName.Value + " spawned!");
            // lobbyUI.UpdateClientListServerRpc();
        }
        else
        {
            playerName.OnValueChanged += (oldName, newName) =>
            {
                Debug.Log("valuechanged: " + playerName.Value + " spawned!");
                lobbyUI.UpdateClientListServerRpc();
            };
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        lobbyUI.UpdateClientListServerRpc();
    }

}
