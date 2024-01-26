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
    private int lastUsedIndex = 0;

    // Start is called before the first frame update
    public LobbyHostListController(VisualElement root)
    {
        _backButton = root.Q<Button>("ButtonBack");
        _startButton = root.Q<Button>("ButtonStart");

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
        // _listView.itemsSource.Clear();
        // _listView.Rebuild();
    }

    public void AddItem(string username)
    {
        Debug.Log($"adding {username} to list");
        connectedPlayers[lastUsedIndex].text = username;
        lastUsedIndex++;
        // _listView.itemsSource.Add(item);
        // _listView.Rebuild();
    }
}
