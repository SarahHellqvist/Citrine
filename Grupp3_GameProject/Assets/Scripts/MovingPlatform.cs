using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("StoppingPoints")]
    [SerializeField] private Vector3[] points;

    [Header("Fields")]
    [SerializeField] private float speed;
    [SerializeField] private float delayTime;
    [SerializeField] private bool automatic;

    [Header("References")]
    [SerializeField] private Transform platformTransform;


    //Timer field
    private float delayStart;

    //Targeting fields
    private int pointIndex;
    private float tolerance;
    private Vector3 targetPoint;
    private Vector3 heading;

    void Start()
    {
        if (points.Length > 0)
        {
            targetPoint = points[0];
        }
        tolerance = speed * Time.deltaTime;
    }

    void Update()
    {
        if (platformTransform.position != targetPoint)
        {
            Move();
        }
        else
        {
            UpdateTarget();
        }
    }

    private void Move()
    {
        heading = targetPoint - platformTransform.position;
        platformTransform.position += heading.normalized * speed * Time.deltaTime;
        if (heading.magnitude < tolerance)
        {
            platformTransform.position = targetPoint;
            delayStart = Time.time;
        }
        Physics.SyncTransforms();
    }

    private void UpdateTarget()
    {
        if(automatic)
        {
            if (Time.time - delayStart > delayTime)
            {
                NextTarget();
            }
        }
    }

    private void NextTarget()
    {
        pointIndex++;
        if (pointIndex >= points.Length)
        {
            pointIndex = 0;
        }
        targetPoint = points[pointIndex];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.AddComponent<Rigidbody>();
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            other.transform.parent = platformTransform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject.GetComponent<Rigidbody>());
            other.transform.parent = null;
        }
    }

    public void SetAutomatic(bool b){automatic = b;}
}
