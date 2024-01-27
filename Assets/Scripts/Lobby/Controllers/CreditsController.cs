using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class CreditsController
{
    public Action Back { set => _backButton.clicked += value; }

    private Button _backButton;

    public CreditsController(VisualElement root)
    {
        _backButton = root.Q<Button>("ButtonBack");
    }
}
