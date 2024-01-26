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
    private int lastUsedIndex = 0;

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
        connectedPlayers.Add(root.Q<Label>("label-player-6"));
    }

    public void ClearList()
    {
        // _listView.Clear();
    }

    public void AddListEntry(string username)
    {
        Debug.Log($"adding {username} to list");
        connectedPlayers[lastUsedIndex].text = username;
        lastUsedIndex++;
        // connections.Add(username);
        // _listView.Rebuild();
    }
}
