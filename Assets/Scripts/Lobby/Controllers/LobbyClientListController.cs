using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class LobbyClientListController
{
    public Action Back { set => _backButton.clicked += value; }

    private Button _backButton;

    private List<Label> connectedPlayers;
    // private int lastUsedIndex = 0;

    public LobbyClientListController(VisualElement root)
    {
        _backButton = root.Q<Button>("ButtonBack");

        connectedPlayers = new List<Label>();

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

        for (int i = 0; i < connectedPlayers.Count; i++)
        {
            connectedPlayers[i].text = connectedClientNames[i];
        }
    }

    public void SetClient(int index, string name) {
        connectedPlayers[index].text = name;
    }

    // public void AddClient(ulong clientId, string username)
    // {
    //     Debug.Log($"adding {username} to list");
    //     connectedPlayers[lastUsedIndex].text = username;
    //     lastUsedIndex++;
    //     // connections.Add(username);
    //     // _listView.Rebuild();
    // }
}
