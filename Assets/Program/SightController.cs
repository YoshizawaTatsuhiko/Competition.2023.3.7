using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]

public class SightController : MonoBehaviour
{
    /// <summary>çıìGópÇÃSphereCollider</summary>
    private SphereCollider _sphere = default;

    void Start()
    {
        _sphere = GetComponent<SphereCollider>();
        _sphere.isTrigger = true;
    }

    void Update()
    {
        
    }
}
