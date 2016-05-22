using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FixedJoint))]
public class JointDefs : MonoBehaviour
{
    [SerializeField]
    int points = 100;

    [SerializeField]
    int cost = 100;

    [SerializeField]
    AudioClip audioClip;

    Rigidbody _rb;
    FixedJoint _joint;
    Multiplier _multi;

	public float fakeMass = 0.5F; 

	float realMass;
	float connectedBodyRealMass;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _joint = GetComponent<FixedJoint>();

		if(_joint.connectedBody != null)
		{
			connectedBodyRealMass = _joint.connectedBody.mass;
			_joint.connectedBody.mass = fakeMass;
		}


		realMass = _rb.mass;
		_rb.mass =   fakeMass;
    }

    void Start()
    {
        _rb.Sleep();
    }

    void OnCollisionEnter(Collision c)
    {
        _multi = c.gameObject.GetComponent<Multiplier>();

        if (_multi != null)
            Debug.Log("_multi value " + _multi.Amount);
    }

    void OnJointBreak(float power)
    {
        //_rb.useGravity = true;
        //_joint.connectedBody.useGravity = true;
        Debug.Log("Joint between " + name + " and " + _joint.connectedBody.name + " broke with power " + power);

		var pts = _multi != null ? (int) (_multi.Amount * points) : points;

		var gm = GameMachine.Instance;
		if(gm != null)
		{

			gm.Damage += cost;
			gm.Points += pts;
		}

		var numFloater = NumberFloater.Get();
        numFloater.transform.position = transform.position;
		numFloater.SetText(pts);

		if(_joint.connectedBody != null)
		{
			_joint.connectedBody.mass = connectedBodyRealMass;
		}


		realMass = _rb.mass;
		_rb.mass = fakeMass;


		_rb.mass = realMass;

		var newMulti = _multi != null ? _multi.Amount + 0.5f : 1.5f;
		foreach(var rb in transform.parent.GetComponentsInChildren<Rigidbody>())
		{
			rb.constraints = RigidbodyConstraints.None;

			rb.useGravity = true;

			var m = rb.gameObject.GetComponent<Multiplier>();

			if(m == null)
				m = rb.gameObject.AddComponent<Multiplier>();

			m.Amount = newMulti;
		};
    }
}

