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

    public static event Action<int> RequestFire;

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

    public void OnFire(InputValue input)
    {
        RequestFire?.Invoke(input.Get<int>());
    }
}
