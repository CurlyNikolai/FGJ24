using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class LobbyHostListController
{
    public Action Back { set => _backButton.clicked += value; }
    public Action Start { set => _startButton.clicked += value; }

    private ListView _listView;
    private Button _backButton;
    private Button _startButton;

    // Start is called before the first frame update
    public LobbyHostListController(VisualElement root)
    {
        _listView = root.Q<ListView>("ListViewConnections");
        _backButton = root.Q<Button>("ButtonBack");
        _startButton = root.Q<Button>("ButtonStart");
    }

    public void ClearList()
    {
        // _listView.itemsSource.Clear();
        // _listView.Rebuild();
    }

    public void AddItem(string item) {
        // _listView.itemsSource.Add(item);
        // _listView.Rebuild();
    }
}
