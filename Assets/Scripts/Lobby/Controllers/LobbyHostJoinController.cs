using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyHostJoinController
{
    public Action Back { set => _backButton.clicked += value; }
    public Action Join { set => _joinButton.clicked += value; }

    private TextField _usernameTextField;
    private Button _backButton;
    private Button _joinButton;

    public LobbyHostJoinController(VisualElement root) {
        _usernameTextField = root.Q<TextField>("TextFieldUsername");
        _backButton = root.Q<Button>("ButtonBack");
        _joinButton = root.Q<Button>("ButtonJoin");
    }

    public string Username() {
        return _usernameTextField.text;
    }
}
