using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using System;
using Unity.Netcode;

public class DummyNetwork : MonoBehaviour
{
    public NetworkManager networkManager;

    // Start is called before the first frame update
    void Start()
    {
        networkManager.StartHost();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
