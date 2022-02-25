using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMotion : MonoBehaviour
{
    [SerializeField]
    private Transform hubTarget;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(hubTarget);
        Vector3 rotation = transform.eulerAngles;
        rotation.x = 0;
        rotation.z = 0;
        transform.eulerAngles = rotation;
    }
}
