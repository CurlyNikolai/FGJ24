using System;
using UnityEngine.UIElements;

public class LobbyMainController
{
    public Action CreateGame { set => _createGameButton.clicked += value; }
    public Action JoinGame { set => _joinGameButton.clicked += value; }

    public Action Credits { set => _creditsButton.clicked += value; }

    private Button _createGameButton;
    private Button _joinGameButton;
    private Button _creditsButton;

    public LobbyMainController(VisualElement root)
    {
        _createGameButton = root.Q<Button>("ButtonCreateGame");
        _joinGameButton = root.Q<Button>("ButtonJoinGame");
        _creditsButton = root.Q<Button>("ButtonCredits");
    }
}
