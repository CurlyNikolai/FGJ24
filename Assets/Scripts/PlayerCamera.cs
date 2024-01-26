using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Vector3 targetOffset;
    public Transform target;

    private void LateUpdate()
    {
        transform.position = target.position + targetOffset;
    }
}
