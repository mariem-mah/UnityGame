using UnityEngine;
using System.Collections;

public class RotateTowards : MonoBehaviour
{
    public Transform target;

    private bool lookedAtTarget;

    void OnEnable()
    {
        lookedAtTarget = false;
    }

    void Update()
    {
        if (!lookedAtTarget)
        {
            transform.LookAt(target.position);
            lookedAtTarget = true;
        }
    }
}
