using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] public GameObject visual;
    [SerializeField, Range(0.1f, 5f)] private float pathCorrectionTime = 1f;

    private Rigidbody cached_RB;
    public Rigidbody RB => cached_RB ??= GetComponent<Rigidbody>();

    public Action<GameObject, Vector3> OnCollideWithObject;

    void OnCollisionEnter(Collision collision)
    {
        launchTime = float.MinValue;
    }

    public void Init(Vector3 launchPosition, Vector3 nozzlePosition, Vector3 direction, float speed)
    {
        transform.position = launchPosition;
        visual.transform.position = nozzlePosition;
        RB.velocity = direction * speed;
        launchTime = Time.time;
    }

    float launchTime;
    public void Update()
    {        
        visual.transform.localPosition = Vector3.Lerp(visual.transform.localPosition, Vector3.zero, (Time.time - launchTime) / pathCorrectionTime);
    }

}
