using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FixedJoint))]
public class JointDefs : MonoBehaviour
{
    Rigidbody _rb;
    FixedJoint _joint;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _joint = GetComponent<FixedJoint>();
    }


    void OnJointBreak(float power)
    {
        _rb.useGravity = true;
        _joint.connectedBody.useGravity = true;
        Debug.Log("Joint between " + name + " and " + _joint.connectedBody.name + " broke with power " + power);
    }
}

