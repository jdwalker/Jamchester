using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[RequireComponent(typeof(Rigidbody))]
public class Multiplier : MonoBehaviour
{
    public static float Highest { get; set; }

    private float _amount;
    public float Amount
    {
        get { return _amount; }
        set
        {
            _amount = value;
            if (value > Highest)
                Highest = value;
        }
    }
}

