using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FixedJoint))]
public class JointDefs : MonoBehaviour
{
    [SerializeField]
    int points = 100;

    [SerializeField]
    int cost = 100;

    Rigidbody _rb;
    FixedJoint _joint;
    Multiplier _multi;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _joint = GetComponent<FixedJoint>();
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

        var gm = GameMachine.Instance;

        gm.Damage += cost;
        var pts = _multi != null ? (int)(_multi.Amount * points) : points;
        gm.Points += pts;

        var numFloater = NumberFloater.Get();
        numFloater.SetText(pts);
        numFloater.transform.position = transform.position;

        var newMulti = _multi != null ? _multi.Amount + 0.5f : 1.5f;
        transform.parent.GetComponentsInChildren<Rigidbody>().Select(rb =>
        {
            rb.useGravity = true;

            var m = rb.gameObject.GetComponent<Multiplier>();

            if (m == null)
                m = rb.gameObject.AddComponent<Multiplier>();

            m.Amount = newMulti;
            return true;
        }).ToArray();
    }
}

