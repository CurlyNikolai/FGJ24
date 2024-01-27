using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyPlayerData : NetworkBehaviour
{
    [SerializeField]
    public NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    // [ServerRpc]
    // void SetPlayerNameServerRpc(FixedString32Bytes playerName) { /* ... */ }

    // void SetPlayerName(string name)
    // {
    //     SetPlayerNameServerRpc(new FixedString32Bytes(name));
    // }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            playerName.Value = LobbyManager.localUsername;
            Debug.Log("owner: " + playerName.Value + " spawned!");
        }
        else
        {
            playerName.OnValueChanged += (oldName, newName) =>
            {
                Debug.Log("valuechanged: " + playerName.Value + " spawned!");
            };
        }

        Debug.Log("A new LobbyPlayerData was spawned");
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
    }

}
