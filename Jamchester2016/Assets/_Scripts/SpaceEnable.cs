using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class SpaceEnable : MonoBehaviour
{
    Rigidbody _rb;

    // Use this for initialization
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _rb.isKinematic = !_rb.isKinematic;
    }
}
