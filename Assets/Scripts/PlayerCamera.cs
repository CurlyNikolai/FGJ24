using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Vector3 targetOffset;
    [SerializeField] private float cameraSpeed = 1;
    public Transform target;

    private void LateUpdate()
    {
        if (target == null) {
            return;
        }
        Vector3 targetPos = target.position + targetOffset;
        transform.position = Vector3.Lerp(transform.position, targetPos, cameraSpeed * Time.deltaTime);
    }
}
