using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class LobbyClientJoinController
{
    public Action Back { set => _backButton.clicked += value; }
    public Action Join { set => _joinButton.clicked += value; }

    private TextField _usernameTextField;
    private TextField _hostAddressTextField;
    private Button _backButton;
    private Button _joinButton;

    public LobbyClientJoinController(VisualElement root)
    {
        _usernameTextField = root.Q<TextField>("TextFieldUsername");
        _hostAddressTextField = root.Q<TextField>("TextFieldHostAddress");
        _backButton = root.Q<Button>("ButtonBack");
        _joinButton = root.Q<Button>("ButtonJoin");
    }

    public string Username() {
        return _usernameTextField.text;
    }

    public string HostAddress() {
        return _hostAddressTextField.text;
    }
}
