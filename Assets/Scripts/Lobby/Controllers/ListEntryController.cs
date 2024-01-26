using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ListEntryController
{
    Label usernameLabel;

    public void SetVisualElement(VisualElement visualElement)
    {
        usernameLabel = visualElement.Q<Label>("username");
    }

    public void SetUsername(string username)
    {
        usernameLabel.text = username;
    }
}
