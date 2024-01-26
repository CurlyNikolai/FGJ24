using System;
using UnityEngine.UIElements;

public class LobbyMainController
{
    public Action CreateGame { set => _createGameButton.clicked += value; }
    public Action JoinGame { set => _joinGameButton.clicked += value; }

    private Button _createGameButton;
    private Button _joinGameButton;

    public LobbyMainController(VisualElement root)
    {
        _createGameButton = root.Q<Button>("ButtonCreateGame");
        _joinGameButton = root.Q<Button>("ButtonJoinGame");
    }
}
