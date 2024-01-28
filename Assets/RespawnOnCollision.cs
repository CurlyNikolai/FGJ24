using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnOnCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.transform.position = new Vector3(0, 0.5f, 0);
        collision.gameObject.transform.rotation = Quaternion.identity;
    }
}
