using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    PlayerInput playerInput;

    public static event Action<Vector2> RequestMove;
    public static event Action<float> RequestReset;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void OnMove(InputValue input)
    {
        RequestMove?.Invoke(input.Get<Vector2>());
    }

    public void OnReset()
    {
        RequestReset?.Invoke(0f);
    }
}
