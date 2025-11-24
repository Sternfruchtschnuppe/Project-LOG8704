using System;
using UnityEngine;

public class EdgeParticleMover : MonoBehaviour
{
    private void Update()
    {
        var rgt = Vector3.Cross(transform.up, Vector3.up);
        var fwd = -Vector3.Cross(rgt.normalized, transform.up);

        if (fwd == Vector3.zero) return;
        transform.rotation = Quaternion.LookRotation(fwd, transform.up);
    }
}
