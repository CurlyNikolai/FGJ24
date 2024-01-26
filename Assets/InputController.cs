using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    PlayerInput playerInput;

    public static event Action<Vector2> RequestMove;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void OnMove(InputValue input)
    {
        RequestMove?.Invoke(input.Get<Vector2>());
    }
}