using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 2f;
    public float StoppingDistance = 5f;


    private bool isMoving;
    private Vector3 stoppingPosition;

    public Vector3 StoppingPosition
    {
        get
        {
            return stoppingPosition; }
    }
    void OnEnable()
    {
        isMoving = true;
        stoppingPosition = GetStoppingPosition();

    }
    void Update()
    {
        if (isMoving)
        {
            if(Vector3.Distance(transform.position, StoppingPosition) >= 0)
            {
                MoveTowardsTarget();
            } else
            {
                transform.position = StoppingPosition;
                isMoving = false;
            }
        }
    }

    private Vector3 GetStoppingPosition()
    {
        Ray ray = new Ray(target.position, transform.position);
        return ray.GetPoint (StoppingDistance);

    }
    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, StoppingPosition, moveSpeed * Time.deltaTime);
    }
}
