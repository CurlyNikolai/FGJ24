using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    private UnityTransport unityTransport;

    public static string localUsername;

    private void Start()
    {
        unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
    }

    public void StartHost(string userName)
    {
        Debug.Log("Starting host");
        localUsername = userName;
        unityTransport.SetConnectionData("127.0.0.1", 7777, "0.0.0.0");
        NetworkManager.Singleton.StartHost();
    }

    public void ConnectClient(string userName, string hostAddress)
    {
        localUsername = userName;
        string[] hostAddressSplit = hostAddress.Split(':');

        string address = hostAddressSplit[0];
        var port = ushort.Parse(hostAddressSplit[1]);

        unityTransport.SetConnectionData(address, port);

        Debug.Log($"Connecting {userName} to: {unityTransport.ConnectionData.Address}:{unityTransport.ConnectionData.Port}");
        NetworkManager.Singleton.StartClient();
    }

    public void Disconnect()
    {
        NetworkManager.Singleton.Shutdown();
    }

}
