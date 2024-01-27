using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyHostListController
{
    public Action Back { set => _backButton.clicked += value; }
    public Action Start { set => _startButton.clicked += value; }

    private Button _backButton;
    private Button _startButton;

    private List<Label> connectedPlayers;
    // private Dictionary<ulong, Label> playerIdMap;

    // private int lastUsedIndex = 0;

    // Start is called before the first frame update
    public LobbyHostListController(VisualElement root)
    {
        _backButton = root.Q<Button>("ButtonBack");
        _startButton = root.Q<Button>("ButtonStart");

        connectedPlayers = new List<Label>();
        // playerIdMap = new Dictionary<ulong, Label>();

        connectedPlayers.Add(root.Q<Label>("label-player-1"));
        connectedPlayers.Add(root.Q<Label>("label-player-2"));
        connectedPlayers.Add(root.Q<Label>("label-player-3"));
        connectedPlayers.Add(root.Q<Label>("label-player-4"));
        connectedPlayers.Add(root.Q<Label>("label-player-5"));
        connectedPlayers.Add(root.Q<Label>("label-player-6"));
        connectedPlayers.Add(root.Q<Label>("label-player-7"));
        connectedPlayers.Add(root.Q<Label>("label-player-8"));

        ClearList();
    }

    public void ClearList()
    {
        // _listView.Clear();
        foreach (Label label in connectedPlayers)
        {
            label.text = "";
        }
    }

    public void SetClientList(List<string> connectedClientNames)
    {
        if (connectedClientNames.Count > connectedPlayers.Count)
        {
            Debug.LogError($"The number of connected players has exceeded the max value {connectedClientNames.Count}/{connectedPlayers.Count}");
        }

        for (int i = 0; i < connectedClientNames.Count; i++)
        {
            connectedPlayers[i].text = connectedClientNames[i];
        }
    }

    // public void AddClient(ulong clientId, string username)
    // {
    //     Debug.Log($"adding {username} to list");
    //     connectedPlayers[lastUsedIndex].text = username;
    //     playerIdMap.Add(clientId, connectedPlayers[lastUsedIndex]);
    //     lastUsedIndex++;
    //     // _listView.itemsSource.Add(item);
    //     // _listView.Rebuild();
    // }
}
