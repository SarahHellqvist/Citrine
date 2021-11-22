using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float rotationSpeed;

    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, rotationSpeed);
    }
}
