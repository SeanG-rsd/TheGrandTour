using System;
using UnityEngine;

public class Bee : MonoBehaviour
{
    public Transform start {get;private set;}
    public Transform destination {get;private set;}
    private bool sent;

    [SerializeField] private float moveSpeed;

    public void Send(Transform s, Transform d)
    {
        start = s;
        destination = d;

        // transform.LookAt(d);

        sent = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!sent) return;
        
        transform.position = Vector3.MoveTowards(transform.position, destination.position, moveSpeed * Time.deltaTime);
    }
}
