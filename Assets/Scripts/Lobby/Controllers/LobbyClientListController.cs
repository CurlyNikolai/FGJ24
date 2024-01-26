using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class LobbyClientListController
{
    public Action Back { set => _backButton.clicked += value; }

    private ListView _listView;
    private Button _backButton;

    private List<string> connections;

    public LobbyClientListController(VisualElement root) {
        _listView = root.Q<ListView>("ListViewConnections");
        _backButton = root.Q<Button>("ButtonBack");

        _listView.makeItem = () => new Label("default text");

        connections = new List<string>();

        _listView.itemsSource = connections;
    }

    public void ClearList() {
        _listView.Clear();
    }

    public void AddListEntry(string username) {
        Debug.Log($"adding {username} to list");
        connections.Add(username);
        _listView.Rebuild();
    }
}
